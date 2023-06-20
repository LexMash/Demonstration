using FromTheBasement.Data.NotepadSystem;
using FromTheBasement.Domain.NotepadSystem;
using Lukomor.DIContainer;
using Lukomor.Presentation.Models;
using Lukomor.Presentation.Views.Widgets;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FromTheBasement.View.UserInterfaces.NotesUI
{
    public class WidgetNotes : Widget
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private Button _nextPageBtn;
        [SerializeField] private Button _prevPageBtn;
        [SerializeField] private NotepadPageView _pageViewPrefab;

        public int LastOpenPagesIndex => _lastOpenedPagesIndex;
        public int PagesCount => _drawedPages.Count;

        private readonly DIVar<NotepadFeature> _notepadFeature = new DIVar<NotepadFeature>();
        private int _lastOpenedPagesIndex;
        private List<NotepadPageView[]> _drawedPages; //TODO сделать класс типо "страничный разворот" и упростить код

        private int _oldCountDrawedPages;

        protected override void Install()
        {
            base.Install();

            _drawedPages = new List<NotepadPageView[]>();
        }

        protected override void Subscribe(Model model)
        {
            base.Subscribe(model);

            _nextPageBtn.onClick.AddListener(OnNextPageButtonClicked);
            _prevPageBtn.onClick.AddListener(OnPreviousButtonClicked);

            SetPageButtonsState();
        }

        protected override void Unsubscribe(Model model)
        {
            base.Unsubscribe(model);

            _nextPageBtn.onClick.RemoveListener(OnNextPageButtonClicked);
            _prevPageBtn.onClick.RemoveListener(OnPreviousButtonClicked);
        }

        protected override void Refresh(Model model)
        {
            base.Refresh(model);

            var pages = PreparePages();

            if (pages == null)
                return;

            DrawPages(pages);

            var index = _notepadFeature.Value.IsHaveUnreadNotes ? FindFirstPageContainsUnreadMessageIndex() : _lastOpenedPagesIndex;

            ShowPages(index);
        }

        private void OnNextPageButtonClicked()
        {
            ShowNextPage();
        }

        private void OnPreviousButtonClicked()
        {
            ShowPreviousPage();
        }

        private void ShowNextPage()
        {
            Debug.Log("show next");
            ShowPages(_lastOpenedPagesIndex+1);
        }
        
        private void ShowPreviousPage()
        {
            Debug.Log("show prev");
            ShowPages(_lastOpenedPagesIndex-1);
        }

        private void ShowPages(int value)
        {
            value = Mathf.Clamp(value, 0, _drawedPages.Count - 1);

            for (int i = 0; i < _drawedPages.Count; i++)
            {
                for(int j = 0; j < _drawedPages[i].Length; j++)
                {
                    var page = _drawedPages[i][j];
                    page.gameObject.SetActive(value == i);

                    _notepadFeature.Value.PageMarkAsViewed.Execute(page.Index);
                }
            }

            _lastOpenedPagesIndex = value;

            SetPageButtonsState();
        }

        private NotepadPage[][] PreparePages()
        {
            var allPages = _notepadFeature.Value.GetPage.All();

            if (allPages.Count == 0)
                return null;

            var modificator = allPages.Count % 2;
            var iterations = (allPages.Count + modificator) / 2;

            var prepairedPages = new NotepadPage[iterations][];

            for (int i = 0, j = 0; i < iterations; i++, j += 2)
            {
                NotepadPage first = allPages[j];
                NotepadPage second;

                if (j + 1 > allPages.Count - 1)
                    second = null;
                else
                    second = allPages[j + 1];

                prepairedPages[i] = new NotepadPage[] { first, second };
            }

            return prepairedPages;
        }

        private void DrawPages(NotepadPage[][] pages)
        {  
            DrawNewPages(pages);

            if (!_drawedPages.IsNullOrEmpty())
                ReFillExistsPages(pages);
        }

        private void DrawNewPages(NotepadPage[][] pages)
        {
            if (pages.Length - _drawedPages.Count <= 0)
            {
                if (pages.IsNullOrEmpty() && _drawedPages.IsNullOrEmpty())
                    return;

                if (pages.Last().Last() == null || pages.Last().Last().Index == _drawedPages.Last().Last().Index)
                    return;

                else //если индексы последних страниц в последней паре не совпадают, то надо создать последнюю страницу в паре
                {
                    var lastPage = pages[pages.Length - 1].Last();
                    var lastPageView = _drawedPages[_drawedPages.Count - 1].Last();

                    lastPageView.SetIndex(lastPage.Index);
                    FillPageView(lastPageView, lastPage);

                    var isChanged = lastPage.IsChanged; //костыль что бы не отрисовывало дважды страницу
                }
            }
            else
            {               
                for (int i = _drawedPages.Count; i < pages.Length; i++)
                {
                    var pageViews = new NotepadPageView[pages[i].Length];

                    for (int j = 0; j < pages[i].Length; j++)
                    {
                        pageViews[j] = GetDrawedPage(pages[i][j]);
                        if (pages[i][j] != null)
                        {
                            pageViews[j].SetIndex(pages[i][j].Index);
                            var isChanged = pages[i][j].IsChanged; //костыль что бы не отрисовывало дважды страницу
                        }
                    }

                    _drawedPages.Add(pageViews);
                }

                _oldCountDrawedPages = _drawedPages.Count;
            }
        }

        private void ReFillExistsPages(NotepadPage[][] pagesSpread)
        {          
            for(int i = 0; i < _oldCountDrawedPages; i++)
            {
                var pages = pagesSpread[i];

                for (int k = 0; k < pages.Length; k++)
                {
                    if (pages[k] != null && pages[k].IsChanged)
                    {
                        Debug.Log($"page {pages[k].Index} is changed");

                        var pageView = _drawedPages[i][k];
                        ReFillPageView(pageView, pages[k]);
                    }
                }
            }

            _oldCountDrawedPages = _drawedPages.Count;
        }


        private NotepadPageView GetDrawedPage(NotepadPage page)
        {
            var pageView = Instantiate(_pageViewPrefab, _parent);

            if(page != null)
            {
                FillPageView(pageView, page);
            }

            return pageView;
        }

        private void FillPageView(NotepadPageView pageView, NotepadPage page)
        {           
            for (int i = 0; i < page.Notes.Count; i++)
            {
                var notePrefab = page.Notes[i].Meta.Prefab;
                Instantiate(notePrefab, pageView.Content);
            }

            Debug.Log($"page {pageView.Index} filled");
        }

        private void ReFillPageView(NotepadPageView pageView, NotepadPage page)
        {
            ClearPageView(pageView);
            FillPageView(pageView, page);

            Debug.Log($"page {pageView.Index} REfilled");
        }

        private void ClearPageView(NotepadPageView pageView)
        {
            var notesView = pageView.Content.GetComponentsInChildren<NoteView>();

            foreach (var note in notesView)
            {
                Destroy(note.gameObject);
            }

            Debug.Log($"page {pageView.Index} cleared");
        }

        private int FindFirstPageContainsUnreadMessageIndex()
        {
            var pageWithUnreadNote = _notepadFeature.Value.GetPage.ContainsFirstUnreadNote();
            var pageIndex = pageWithUnreadNote.Index / 2;
            return pageIndex;
        }

        private void SetPageButtonsState()
        {
            _nextPageBtn.gameObject.SetActive(_lastOpenedPagesIndex < _drawedPages.Count - 1);
            _prevPageBtn.gameObject.SetActive(_lastOpenedPagesIndex > 0);
        }
    }
}

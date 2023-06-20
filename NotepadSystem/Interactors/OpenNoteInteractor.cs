using FromTheBasement.Application.NotepadSystem.Signals;
using FromTheBasement.Data.NotepadSystem;
using FromTheBasement.Domain.NotepadSystem;
using FromTheBasement.Domain.NotepadSystem.Interactors;
using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FromTheBasement.Application.NotepadSystem.Interactors
{
    public class OpenNoteInteractor : IOpenNoteInteractor
    {
        private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();
        private NotepadModel _model;
        private IGetNotesInteractor _getNotesInteractor;
        INotepadContainsNoteInteractor _notepadContains;

        public OpenNoteInteractor(NotepadModel model, IGetNotesInteractor getNotesInteractor, INotepadContainsNoteInteractor notepadContainsNoteInteractor)
        {
            _model = model;
            _getNotesInteractor = getNotesInteractor;
            _notepadContains = notepadContainsNoteInteractor;
        }

        public void Execute(string id)
        {
            var note = _getNotesInteractor.ByID(id); //программа упадёт если её нет
            var contains = _notepadContains.Execute(id);

            if(!contains)
            {
                note.IsRead = false;

                var page = GetPage(note.Meta.Prefab.Size);

                page.Notes.Add(note);

                var signal = new NoteOpenedSignal(this, note.Id);
                _signalTower.Value.FireSignal(signal);

                _model.UnreadNotes.Add(note);

                Debug.Log($"Note whith ID:{id} added to page {page.Index}");
            }
            else
                Debug.Log($"Note whith ID:{id} already exist in Notepad");
        }

        private NotepadPage GetPage(int size)
        {
            var page = _model.Pages.Values.FirstOrDefault(pair => !pair.IsFull && pair.CurrentPageSize + size <= pair.MaxPageSize);

            if (page != null)
                return page;
            else
            {
                var index = GetNewPageIndex();

                _model.Pages.Add(index, new NotepadPage(index, new List<Note>()));
                return _model.Pages[index];
            }
        }

        private int GetNewPageIndex()
        {
            return _model.Pages.Count;
        }
    }
}
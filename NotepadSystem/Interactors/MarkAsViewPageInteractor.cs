using FromTheBasement.Domain.NotepadSystem.Interactors;

namespace FromTheBasement.Application.NotepadSystem.Interactors
{
    public class MarkAsViewPageInteractor : IMarkAsViewPageInteractor
    {
        private IGetNotepadPageInteractor _getPage;
        private IMarkAsViewedInteractor _markAsView;

        public MarkAsViewPageInteractor(IGetNotepadPageInteractor getPage, IMarkAsViewedInteractor markAsView)
        {
            _getPage = getPage;
            _markAsView = markAsView;
        }

        public void Execute(int index)
        {
            var page = _getPage.ByIndex(index);

            if (!page.IsRead)
            {
                var notes = page.Notes;

                for (int i = 0; i < notes.Count; i++)
                {
                    if (!notes[i].IsRead)
                        _markAsView.Execute(notes[i].Id);
                }
            }
        }
    }
}

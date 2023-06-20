using FromTheBasement.Data.NotepadSystem;
using FromTheBasement.Domain.NotepadSystem;
using FromTheBasement.Domain.NotepadSystem.Interactors;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace FromTheBasement.Application.NotepadSystem.Interactors
{
    public class GetNotepadPageInteractor : IGetNotepadPageInteractor
    {
        private NotepadModel _model;

        public GetNotepadPageInteractor(NotepadModel model)
        {
            _model = model;
        }

        public IReadOnlyList<NotepadPage> All() //не уверен что нужно, но пока оставлю
        {
            return _model.Pages.Values.ToList();
        }

        public NotepadPage ByIndex(int index)
        {
            return _model.Pages[index];
        }

        public NotepadPage ContainsFirstUnreadNote()
        {
            var pages = _model.Pages.Values.ToList().FindAll(page => !page.IsEmpty);

            for(int i = 0; i < pages.Count; i++)
            {
                var notes = pages[i].Notes;

                for (int k = 0; k < notes.Count; k++)
                {
                    if (!notes[k].IsRead)
                        return pages[i];
                }
            }

            return null;
        }
    }
}

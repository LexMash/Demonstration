using FromTheBasement.Domain.NotepadSystem;
using FromTheBasement.Domain.NotepadSystem.Interactors;

namespace FromTheBasement.Application.NotepadSystem.Interactors
{
    public class NotepadContainsNoteInteractor : INotepadContainsNoteInteractor
    {
        private NotepadModel _model;
        public NotepadContainsNoteInteractor(NotepadModel model)
        {
            _model = model;
        }

        public bool Execute(string noteId)
        {
            foreach(var pair in _model.Pages)
            {
                if (pair.Value.Notes.Find(n => n.Id == noteId) != null)
                    return true;
            }

            return false;
        }
    }
}

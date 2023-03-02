using FromTheBasement.Data.NotepadSystem;

namespace FromTheBasement.Domain.NotepadSystem.Interactors
{
    public interface IGetNotesInteractor
    {
        Note ByID(string noteId);
    }
}

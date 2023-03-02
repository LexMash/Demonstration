using FromTheBasement.Data.NotepadSystem;
using System.Collections.Generic;

namespace FromTheBasement.Domain.NotepadSystem.Interactors
{
    public interface IGetNotepadPageInteractor
    {
        NotepadPage ByIndex(int index);
        NotepadPage ContainsFirstUnreadNote();
        IReadOnlyList<NotepadPage> All();
    }
}

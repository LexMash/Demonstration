using FromTheBasement.Data.NotepadSystem;
using FromTheBasement.View.UserInterfaces.NotesUI;
using System.Collections.Generic;

namespace FromTheBasement.Domain.NotepadSystem
{
    public class NotepadModel
    {
        public IReadOnlyDictionary<string, Note> NotesMap => _notepad.NotesMap;
        public Dictionary<int, NotepadPage> Pages => _notepad.Pages;
        public List <Note> UnreadNotes => _notepad.UnreadNotes;

        private Notepad _notepad;

        public NotepadModel(Notepad notepad)
        {
            _notepad = notepad;
        }
    }
}


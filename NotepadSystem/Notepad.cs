using System.Collections.Generic;

namespace FromTheBasement.Data.NotepadSystem
{
    public class Notepad
    {
        public Dictionary<string, Note> NotesMap = new Dictionary<string, Note>(); //типо база данных

        public Dictionary<int, NotepadPage> Pages = new Dictionary<int, NotepadPage>();  

        public List<Note> UnreadNotes = new List<Note>();
    }
}


using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FromTheBasement.Data.NotepadSystem
{
    public class NotepadPage
    {
        public int Index { get; private set; }
        public int MaxPageSize { get; private set; }
        public int CurrentPageSize => Notes.Sum(n => n.Meta.Prefab.Size);
        public bool IsFull => CurrentPageSize >= MaxPageSize;
        public bool IsEmpty => CurrentPageSize == 0;
        public bool IsRead => Notes.FirstOrDefault(note => note.IsRead == false) == null;

        public bool IsChanged => PageIsChanged();

        public List<Note> Notes;

        private int _oldCount;

        public NotepadPage(int index, List<Note> notes, int maxPageSize = 310) //допустим столько пикселей
        {
            Index = index;           
            Notes = notes;
            MaxPageSize = maxPageSize;

            _oldCount = notes.Count;
        }

        public bool PageIsChanged()
        {
            if (_oldCount == Notes.Count)
                return false;

            _oldCount = Notes.Count;
            return true;
        }
    }
}

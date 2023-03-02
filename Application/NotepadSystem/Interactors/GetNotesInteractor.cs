using System;
using FromTheBasement.Data.NotepadSystem;
using FromTheBasement.Domain.NotepadSystem;
using FromTheBasement.Domain.NotepadSystem.Interactors;

namespace FromTheBasement.Application.NotepadSystem.Interactors
{
    public class GetNotesInteractor : IGetNotesInteractor
    {
        private NotepadModel _model;

        public GetNotesInteractor(NotepadModel model)
        {
            _model = model;
        }

        public Note ByID(string id)
        {
            if (_model.NotesMap.ContainsKey(id))
                return _model.NotesMap[id];
            else
                throw new Exception($"Cannot find note with ID {id}");
        }
    }
}
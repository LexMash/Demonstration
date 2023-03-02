using FromTheBasement.Application.NotepadSystem.Signals;
using FromTheBasement.Domain.NotepadSystem;
using FromTheBasement.Domain.NotepadSystem.Interactors;
using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using System;
using UnityEngine;

namespace FromTheBasement.Application.NotepadSystem.Interactors
{
    public class MarkAsViewed : IMarkAsViewedInteractor
    {
        private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();

        private NotepadModel _model;
        private IGetNotesInteractor _getNotesInteractor;

        public MarkAsViewed(NotepadModel model, IGetNotesInteractor getNotesInteractor)
        {
            _model = model;
            _getNotesInteractor = getNotesInteractor;
        }

        public void Execute(string id)
        {
            var note = _getNotesInteractor.ByID(id);

            if (note != null)
            {              
                if (_model.UnreadNotes.Contains(note))
                {
                    note.IsRead = true;
                    _model.UnreadNotes.Remove(note);

                    var signal = new MarkAsViewedSignal(this, note.Id);
                    _signalTower.Value.FireSignal(signal);

                    Debug.Log($"Note whith ID:{id} readed");
                }
                else
                {
                    Debug.Log($"Note whith ID:{id} note exist in UnreadNotes");
                }
            }
            else
            {
                throw new NullReferenceException($"Note whith ID:{id} not exist in Notepad");
            }
        }
    }
}

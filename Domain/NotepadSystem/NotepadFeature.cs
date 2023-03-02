using Lukomor.Application.Features;
using FromTheBasement.Data.NotepadSystem;
using FromTheBasement.Domain.NotepadSystem.Interactors;
using System.Threading.Tasks;
using FromTheBasement.Application.NotepadSystem.Interactors;
using UnityEngine;

namespace FromTheBasement.Domain.NotepadSystem 
{
    public class NotepadFeature : Feature
    {
        public static class Keys
        {
            public const string NotesMetaPath = "Meta/Notes";
        }

        public IGetNotesInteractor GetNotes { get; private set; }
        public IOpenNoteInteractor OpenNote { get; private set; }       
        public INotepadContainsNoteInteractor NotepadContains { get; private set; }
        public IGetNotepadPageInteractor GetPage { get; private set; }
        public IMarkAsViewPageInteractor PageMarkAsViewed { get; private set; }

        //не уверен, что так можно, но это упростит дело
        public bool IsHaveUnreadNotes => _model.UnreadNotes.Count > 0;

        private IMarkAsViewedInteractor _notesMarkAsViewed;
        private NotepadModel _model;
        protected override Task InitializeInternal()
        {
            //TODO загрузка пустышки, всех сообщений, сохранённых данных

            var notesMetas = Resources.LoadAll<NoteMeta>(Keys.NotesMetaPath);

            var notepad = new Notepad();

            foreach(var noteMeta in notesMetas)
            {
                notepad.NotesMap.Add(noteMeta.Id, new Note(noteMeta));
            }

            _model = new NotepadModel(notepad);
            
            GetNotes = new GetNotesInteractor(_model);
            NotepadContains = new NotepadContainsNoteInteractor(_model);
            OpenNote = new OpenNoteInteractor(_model, GetNotes, NotepadContains);
            _notesMarkAsViewed = new MarkAsViewed(_model, GetNotes);
            GetPage = new GetNotepadPageInteractor(_model);
            PageMarkAsViewed = new MarkAsViewPageInteractor(GetPage, _notesMarkAsViewed);

            return Task.CompletedTask;
        }
    }
}



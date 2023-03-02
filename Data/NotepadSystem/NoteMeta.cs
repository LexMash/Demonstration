using FromTheBasement.Data.InteractableObjects;
using FromTheBasement.View.UserInterfaces.NotesUI;
using UnityEngine;

namespace FromTheBasement.Data.NotepadSystem
{
    [CreateAssetMenu(menuName = "Application/Notepad/NoteMeta", fileName = "New NoteMeta")]
    public class NoteMeta : InteractableObjectMeta
    {
        [SerializeField] private NoteView _prefab;
        public NoteView Prefab => _prefab;
    }
}
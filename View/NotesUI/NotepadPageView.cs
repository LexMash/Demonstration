using UnityEngine;

namespace FromTheBasement.View.UserInterfaces.NotesUI
{
    public class NotepadPageView : MonoBehaviour
    {
        [field: SerializeField] public Transform Content { get; private set; } 

        public int Index { get; private set; }

        public bool IsEmpty => Content.childCount == 0;

        public void SetIndex(int index)
        {
            Index = index;
        }
    }
}

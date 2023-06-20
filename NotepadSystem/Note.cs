namespace FromTheBasement.Data.NotepadSystem
{
    public class Note
    {
        public string Id => Meta.Id;
        public NoteMeta Meta { get; }
        public bool IsRead { get; set; }

        public Note(NoteMeta meta, bool isAvailable = false, bool isRead = false)
        {
            Meta = meta;
            IsRead = isRead;
        }
    }
}

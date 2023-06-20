using Lukomor.Application.Signals;

namespace FromTheBasement.Application.NotepadSystem.Signals
{
    public struct NoteOpenedSignal : ISignal
    {
        public object Sender { get; }
        public string Id { get; }

        public NoteOpenedSignal(object sender, string id)
        {
            Sender = sender;
            Id = id;
        }
    }
}

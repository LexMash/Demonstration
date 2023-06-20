using Lukomor.Application.Signals;

namespace FromTheBasement.Application.NotepadSystem.Signals
{
    public class MarkAsViewedSignal : ISignal
    {
        public object Sender { get; }
        public string Id { get; }

        public MarkAsViewedSignal(object sender, string id)
        {
            Sender = sender;
            Id = id;
        }
    }
}

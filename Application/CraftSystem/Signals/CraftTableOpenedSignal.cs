using Lukomor.Application.Signals;

namespace FromTheBasement.Application.CraftSystem.Signals
{
    public struct CraftTableOpenedSignal : ISignal
    {
        public object Sender { get; }
        public string Id { get; }

        public CraftTableOpenedSignal(object sender, string id)
        {
            Sender = sender;
            Id = id;
        }
    }
}

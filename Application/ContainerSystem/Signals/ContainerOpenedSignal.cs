using Lukomor.Application.Signals;

namespace FromTheBasement.Application.ContainerSystem.Signals
{
    public struct ContainerOpenedSignal : ISignal
    {
        public object Sender { get; }
        public string ID { get; }

        public ContainerOpenedSignal(object sender, string id)
        {
            Sender = sender;
            ID = id;
        }
    }
}
using FromTheBasement.Data.InventorySystem;
using Lukomor.Application.Signals;

namespace FromTheBasement.Application.ContainerSystem.Signals
{
    public struct ContainerItemAddedSignal : ISignal
    {
        public object Sender { get; }
        public string ID { get; }
        public ItemMeta Meta { get; }

        public ContainerItemAddedSignal(object sender, string id, ItemMeta meta)
        {
            Sender = sender;
            ID = id;
            Meta = meta;
        }
    }
}

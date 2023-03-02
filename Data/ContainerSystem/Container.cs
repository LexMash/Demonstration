using FromTheBasement.Data.InventorySystem;

namespace FromTheBasement.Data.ContainerSystem
{
    public class Container
    {
        public string Id { get; }
        public int Capacity { get; }

        public ItemSlot[] ItemCells => _itemCells;

        private ItemSlot[] _itemCells;

        public Container(string containerId, int capacity = 12)
        {
            _itemCells = new ItemSlot[capacity];

            for (int i = 0; i < capacity; i++)
            {
                _itemCells[i] = new ItemSlot();
            }
            
            Id = containerId;
            Capacity = capacity;
        }
    }
}

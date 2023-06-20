using FromTheBasement.Data.InventorySystem;

namespace FromTheBasement.Domain.ContainerSystem.Interactors
{
    public interface IGetContainerContentInteractor
    {
        ItemSlot[] Execute(string containerId);
    }
}

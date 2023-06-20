using FromTheBasement.Data.InventorySystem;

namespace FromTheBasement.Domain.CraftSystem.Interactors
{
    public interface IGetCraftResultInteractor
    {
        ItemMeta Execute(string factoryId, string ingridentId);
    }
}

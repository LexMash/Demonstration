using FromTheBasement.Data.CraftSystem;

namespace FromTheBasement.Domain.CraftSystem.Interactors
{
    public interface IGetCraftFactoryInteractor
    {
        CraftFactory Execute(string id);
    }
}

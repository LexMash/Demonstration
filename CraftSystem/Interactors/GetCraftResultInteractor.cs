using FromTheBasement.Data.InventorySystem;
using FromTheBasement.Domain.CraftSystem.Interactors;

namespace FromTheBasement.Application.CraftSystem
{
    public class GetCraftResultInteractor : IGetCraftResultInteractor
    {
        private readonly IGetCraftFactoryInteractor _getFactory;

        public GetCraftResultInteractor(IGetCraftFactoryInteractor getFactory)
        {
            _getFactory = getFactory;
        }

        public ItemMeta Execute(string factoryId, string ingridentId)
        {
            var factory = _getFactory.Execute(factoryId);
            return factory.GetResult(ingridentId);
        }
    }
}

using FromTheBasement.Data.InventorySystem;
using FromTheBasement.Domain.ContainerSystem.Interactors;

namespace FromTheBasement.Application.ContainerSystem.Interactors
{
    public class GetContainerContentInteractor : IGetContainerContentInteractor
    {
        private IGetContainerInteractor _getContainerInteractor;

        public GetContainerContentInteractor(IGetContainerInteractor getContainerInteractor)
        {
            _getContainerInteractor = getContainerInteractor;
        }

        public ItemSlot[] Execute(string containerId)
        {
            var container = _getContainerInteractor.Execute(containerId);

            return container.ItemCells;
        }
    }
}

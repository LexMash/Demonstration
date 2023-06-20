using FromTheBasement.Domain.ContainerSystem.Interactors;
using System.Linq;

namespace FromTheBasement.Application.ContainerSystem.Interactors
{
    public class HasItemInteractor : IHasItemInteractor
    {
        private IGetContainerInteractor _getContainerInteractor;

        public HasItemInteractor(IGetContainerInteractor getContainerInteractor)
        {
            _getContainerInteractor = getContainerInteractor;
        }

        public bool Execute(string containerID, string itemID)
        {
            var container = _getContainerInteractor.Execute(containerID);

            return container.ItemCells.Any(c => c.ItemId == itemID);
        }
    }
}

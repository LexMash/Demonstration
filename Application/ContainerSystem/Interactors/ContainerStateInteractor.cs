using FromTheBasement.Domain.ContainerSystem.Interactors;
using System.Linq;

namespace FromTheBasement.Application.ContainerSystem.Interactors
{
    public class ContainerStateInteractor : IContainerStateInteractor
    {
        private IGetContainerInteractor _getContainerInteractor;

        public ContainerStateInteractor(IGetContainerInteractor getContainerInteractor)
        {
            _getContainerInteractor = getContainerInteractor;
        }

        public bool HasEmptyCells(string containerID)
        {
            var container = _getContainerInteractor.Execute(containerID);

            return container.ItemCells.Any(c => c.IsEmpty);
        }

        public bool IsEmpty(string containerID)
        {
            var container = _getContainerInteractor.Execute(containerID);

            return container.ItemCells.All(c => c.IsEmpty);
        }

        public bool IsFull(string containerID)
        {
            var container = _getContainerInteractor.Execute(containerID);

            return container.ItemCells.All(c => !c.IsEmpty);
        }
    }
}

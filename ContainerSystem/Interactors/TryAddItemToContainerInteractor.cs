using FromTheBasement.Application.ContainerSystem.Signals;
using FromTheBasement.Domain.ContainerSystem.Interactors;
using FromTheBasement.Domain.DataBase;
using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using System.Linq;

namespace FromTheBasement.Application.ContainerSystem.Interactors
{
    public class TryAddItemToContainerInteractor : ITryAddItemToContainerInteractor
    {
        private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();
        private readonly DIVar<ItemsDataBaseFeature> _itemDataBase = new DIVar<ItemsDataBaseFeature>();

        private IGetContainerInteractor _getContainerInteractor;
        private IContainerStateInteractor _containerStateInteractor;

        public TryAddItemToContainerInteractor(IGetContainerInteractor getContainerInteractor, IContainerStateInteractor containerStateInteractor)
        {
            _getContainerInteractor = getContainerInteractor;
            _containerStateInteractor = containerStateInteractor;
        }

        public bool Execute(object sender, string containerId, string itemId)
        {
            var container = _getContainerInteractor.Execute(containerId);

            if (_containerStateInteractor.HasEmptyCells(containerId))
            {
                var itemCell = container.ItemCells.First(cell => cell.IsEmpty);

                itemCell.ItemId = itemId;

                SendSignal(sender, containerId, itemId);

                return true;
            }

            return false;
        }

        public bool Execute(object sender, string containerId, string itemId, int index)
        {
            var container = _getContainerInteractor.Execute(containerId);

            if (container.ItemCells[index].IsEmpty)
            {
                var itemCell = container.ItemCells[index];

                itemCell.ItemId = itemId;

                SendSignal(sender, containerId, itemId);

                return true;
            }
            return false;
        }

        private void SendSignal(object sender, string containerId, string itemId)
        {
            var meta = _itemDataBase.Value.GetItemMeta.ById(itemId);
            var signal = new ContainerItemAddedSignal(sender, containerId, meta);
            _signalTower.Value.FireSignal(signal);
        }
    }
}

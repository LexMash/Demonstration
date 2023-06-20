using FromTheBasement.Application.ContainerSystem.Signals;
using FromTheBasement.Domain.ContainerSystem.Interactors;
using FromTheBasement.Domain.DataBase;
using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using System.Linq;

namespace FromTheBasement.Application.ContainerSystem.Interactors
{
    public class TryRemoveItemFromContainerInteractor : ITryRemoveItemFromContainerInteractor
    {
        private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();
        private readonly DIVar<ItemsDataBaseFeature> _itemDataBase = new DIVar<ItemsDataBaseFeature>();

        private IGetContainerInteractor _getContainerInteractor;

        public TryRemoveItemFromContainerInteractor(IGetContainerInteractor getContainerInteractor)
        {
            _getContainerInteractor = getContainerInteractor;
        }

        public bool Execute(object sender, string containerId, string itemId)
        {
            var container = _getContainerInteractor.Execute(containerId);

            var itemCell = container.ItemCells.FirstOrDefault(cell => cell.ItemId == itemId);

            if (itemCell != null)
            {
                itemCell.ItemId = null;

                SendSignal(sender, containerId, itemId);

                return true;
            }

            return false;
        }

        public bool Execute(object sender, string containerId, string itemId, int index)
        {
            var container = _getContainerInteractor.Execute(containerId);

            if (container.ItemCells[index].ItemId == itemId)
            {
                container.ItemCells[index].ItemId = null;

                SendSignal(sender, containerId, itemId);

                return true;
            }

            return false;
        }

        private void SendSignal(object sender, string containerId, string itemId)
        {
            var meta = _itemDataBase.Value.GetItemMeta.ById(itemId);
            var signal = new ContainerItemRemovedSignal(sender, containerId, meta);
            _signalTower.Value.FireSignal(signal);
        }
    }
}

using FromTheBasement.Domain.InventorySystem;
using Lukomor.DIContainer;
using Lukomor.Presentation.Models;
using Lukomor.Presentation.Views.Widgets;
using UnityEngine;
using FromTheBasement.View.UserInterfaces.Containers;
using UnityEngine.EventSystems;

namespace FromTheBasement.View.UserInterfaces.InventoryUI
{
    public class WidgetInventory : Widget
    {
        [SerializeField] private Transform _grid;
        [SerializeField] DragSlotsHandler _slotHandler;
        [SerializeField] private GameObject _firsSelectedObj;

        public Transform Grid => _grid;

        private readonly DIVar<InventoryFeature> _inventoryFeature = new DIVar<InventoryFeature>();

        protected override void Install()
        {
            _slotHandler.Initialize(_grid);
        }

        protected override void Subscribe(Model model)
        {
            base.Subscribe(model);

            _slotHandler.Subscribe();

            _slotHandler.ItemAdded += HandlerItemAdded;
            _slotHandler.ItemRemoved += HandlerItemRemoved;
        }

        protected override void Unsubscribe(Model model)
        {
            base.Unsubscribe(model);

            _slotHandler.Unsubscribe();

            _slotHandler.ItemAdded -= HandlerItemAdded;
            _slotHandler.ItemRemoved -= HandlerItemRemoved;
        }

        public void RefreshInstant()
        {
            var itemCells = _inventoryFeature.Value.GetItemCell.All();
            _slotHandler.UpdateGrid(itemCells);
        }

        protected override void Refresh(Model model)
        {
            base.Refresh(model);

            RefreshInstant();

            EventSystem.current.SetSelectedGameObject(_firsSelectedObj);
        }

        private void HandlerItemRemoved(string itemId, int index)
        {
            _inventoryFeature.Value.TryRemoveItem.Execute(this, itemId, index);
        }

        private void HandlerItemAdded(string itemId, int index)
        {
            _inventoryFeature.Value.TryAddItem.Execute(this, itemId, index);
        }
    }
}

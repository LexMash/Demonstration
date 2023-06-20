using Lukomor.Presentation.Models;
using Lukomor.Presentation.Views.Widgets;
using System.Linq;
using UnityEngine;

namespace FromTheBasement.View.UserInterfaces.InventoryUI.Craft
{
    public class ButtonSlotsHandler : Widget
    {
        [SerializeField] private UIItemSlot[] _inventorySlots;
        [SerializeField] private SlotButton[] _inventorySlotBtns;
        [Space]
        [SerializeField] private UIItemSlot[] _otherSlots;
        [SerializeField] private SlotButton[] _otherSlotBtns;

        protected override void Subscribe(Model model)
        {
            base.Subscribe(model);

            foreach(var btn in _otherSlotBtns)
            {
                btn.OnClick += CraftSlotOnClick;
            }

            foreach(var btn in _inventorySlotBtns)
            {
                btn.OnClick += InventorySlotOnClick;
            }
        }

        protected override void Unsubscribe(Model model)
        {
            base.Unsubscribe(model);

            foreach (var btn in _otherSlotBtns)
            {
                btn.OnClick -= CraftSlotOnClick;
            }

            foreach (var btn in _inventorySlotBtns)
            {
                btn.OnClick -= InventorySlotOnClick;
            }
        }

        private void InventorySlotOnClick(UIItemSlot slot)
        {
            SlotOnClick(slot, _otherSlots);
        }

        private void CraftSlotOnClick(UIItemSlot slot)
        {
            SlotOnClick(slot, _inventorySlots);
        }

        private void SlotOnClick(UIItemSlot operationSlot, UIItemSlot[] otherSlots)
        {
            if (operationSlot.UIItem == null || operationSlot.UIItem.ItemMeta == null)
                return;

            var freeSlot = otherSlots.FirstOrDefault(slot => slot.IsEmpty);

            if (freeSlot == null)
                return;

            freeSlot.Fill(operationSlot.UIItem);
            freeSlot.SetChildren(operationSlot.UIItem);

            var uiItem = operationSlot.UIItem;
            var itemId = uiItem.ItemMeta.Id;
           
            operationSlot.Clear();
            operationSlot.ClearInvokeEvent(itemId);
            uiItem.NewParentSettedInvoke();
        }
    }
}

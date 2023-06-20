using FromTheBasement.Domain.CraftSystem;
using FromTheBasement.View.UserInterfaces.InventoryUI;
using FromTheBasement.View.UserInterfaces.InventoryUI.InventoryItemUI;
using Lukomor.Common;
using Lukomor.DIContainer;
using Lukomor.Presentation;
using Lukomor.Presentation.Views.Windows;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FromTheBasement.Domain.InventorySystem;
using System;
using FromTheBasement.Data.InventorySystem;

namespace FromTheBasement.View.UserInterfaces.Craft
{
    public static class WindowCraftTableExtension
    {
        public static void ShowWindowCraftTable(this UserInterface ui, string craftFactoryId)
        {
            var payload = new Payload(WindowCraftTable.Keys.CraftTable, craftFactoryId);
            ui.ShowWindow<WindowCraftTable>(payload);
        }
    }

    public class WindowCraftTable : DialogWindow
    {
        public static class Keys
        {
            public static readonly string CraftTable = "CraftTable";
        }

        [SerializeField] private TextMeshProUGUI _labelTxt;
        [SerializeField] private Image _iconImg;
        [Space]
        [SerializeField] private UIItemSlot _slot;
        [SerializeField] private UIItemSlot _resultSlot;       
        [SerializeField] private UIItem _uiItemPrefab;
        [Space]
        [SerializeField] private WidgetInventory _widgetInventory;

        public event Action<ItemMeta> Crafted;
        public event Action CraftFailed;

        private readonly DIVar<CraftFactoryFeature> _craftFeature = new DIVar<CraftFactoryFeature>();
        private readonly DIVar<InventoryFeature> _inventory = new DIVar<InventoryFeature>();

        private UIItem _uiItem;
        private UIItem _resultUIItem;

        private string _craftFactoryId;

        public override void Subscribe()
        {
            base.Subscribe();

            _slot.OnFilled += SlotOnFilled;
            _slot.OnCleared += SlotOnCleared;
            _slot.NotEmpty += SlotNotEmpty;

            _resultSlot.OnCleared += ResultSlotOnCleared;
            _resultSlot.NotEmpty += SlotNotEmpty;

            HoldSlot(_resultSlot);
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            _slot.OnFilled -= SlotOnFilled;
            _slot.OnCleared -= SlotOnCleared;
            _slot.NotEmpty -= SlotNotEmpty;

            _resultSlot.OnCleared -= ResultSlotOnCleared;
            _resultSlot.NotEmpty -= SlotNotEmpty;

            BackToInventory();

            ClearSlot(_slot, _uiItem);
            _uiItem = null;
            ClearSlot(_resultSlot, _resultUIItem);
            _resultUIItem = null;
        }

        public override void Refresh()
        {
            base.Refresh();

            _craftFactoryId = GetPayload<string>(Keys.CraftTable);

            var factory = _craftFeature.Value.GetFactory.Execute(_craftFactoryId);

            _labelTxt.text = factory.Id;
            _iconImg.sprite = factory.Icon;
            _iconImg.preserveAspect = true;
        }

        private void TryCraft()
        {
            var result = _craftFeature.Value.GetResult.Execute(_craftFactoryId, _uiItem.ItemMeta.Id);

            if (result)
            {
                _resultUIItem = Instantiate(_uiItemPrefab, _resultSlot.ItemParent);
                _resultUIItem.Setup(result);
                _resultSlot.SetChildren(_resultUIItem);
                _resultSlot.Fill(_resultUIItem);

                _resultUIItem.NewParentSetted += ResultNewParentSetted;

                Crafted?.Invoke(result);
            }
            else
                CraftFailed?.Invoke();
        }

        private void ResultNewParentSetted(UIItem uiItem)
        {
            uiItem.NewParentSetted -= ResultNewParentSetted;
            SlotOnCleared();
            ClearSlot(_slot, _uiItem);
            _uiItem = null;
        }

        private void SlotOnFilled(UIItem uiItem, int index)
        {
            _uiItem = uiItem;

            TryCraft();
        }

        private void SlotOnCleared(string itemId = null, int index = 0)
        {
            _uiItem = null;
            ClearSlot(_resultSlot, _resultUIItem);
            _resultUIItem = null;
            HoldSlot(_resultSlot);

            Debug.Log(_uiItem == null);

            _widgetInventory.RefreshInstant();
        }

        private void ResultSlotOnCleared(string itemId, int index)
        {           
            ClearSlot(_slot, _uiItem);
            _uiItem = null;
            HoldSlot(_slot);
        }

        private void SlotNotEmpty(UIItem uiItem) => uiItem.ResetParent();

        private void BackToInventory()
        {
            if(_uiItem != null)
                _inventory.Value.TryAddItem.Execute(this, _uiItem.ItemMeta.Id);

            else if(_resultUIItem != null)
                _inventory.Value.TryAddItem.Execute(this, _resultUIItem.ItemMeta.Id);
        }

        private void HoldSlot(UIItemSlot slot)
        {
            slot.SetItem(_uiItemPrefab);
        }

        private void ClearSlot(UIItemSlot slot, UIItem uiItem)
        {
            if (uiItem)
                Destroy(uiItem.gameObject);

            slot.Clear();
        }
    }
}

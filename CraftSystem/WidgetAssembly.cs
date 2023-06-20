using FromTheBasement.Data.InventorySystem;
using FromTheBasement.Domain.DataBase;
using FromTheBasement.Domain.InventorySystem;
using FromTheBasement.View.UserInterfaces.InventoryUI.InventoryItemUI;
using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using Lukomor.Presentation.Models;
using Lukomor.Presentation.Views.Widgets;
using System;
using System.Linq;
using UnityEngine;

namespace FromTheBasement.View.UserInterfaces.InventoryUI.Craft
{
    public class WidgetAssembly : Widget
    {
        [SerializeField] private UIItemSlot[] _slots;
        [SerializeField] private UIItemSlot _resultSlot;
        [Space]
        [SerializeField] private UIItem _uiItemPrefab;
        [SerializeField] private WidgetInventory _widgetInventory;

        public event Action<ItemMeta> Assembled;
        public event Action AssemblyFailed;

        private readonly DIVar<ItemsDataBaseFeature> _itemDataBase = new DIVar<ItemsDataBaseFeature>();
        private readonly DIVar<InventoryFeature> _inventory = new DIVar<InventoryFeature>();
        private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();

        private UIItem[] _componentUIItems;
        private UIItem _resultUIItem;

        protected override void Install()
        {
            base.Install();

            _componentUIItems = new UIItem[_slots.Length];

            for(int i = 0; i < _slots.Length; i++)
            {
                _slots[i].SetIndex(i);
            }
        }

        protected override void Subscribe(Model model)
        {
            base.Subscribe(model);

            foreach (var slot in _slots)
            {
                slot.OnFilled += IngridientSlotOnFilled;
                slot.OnCleared += SlotOnCleared;
                slot.NotEmpty += SlotSlotNotEmpty;
            }

            _resultSlot.NotEmpty += SlotSlotNotEmpty;
            _resultSlot.OnCleared += ResultSlotOnCleared;
        }

        protected override void Unsubscribe(Model model)
        {
            base.Unsubscribe(model);

            foreach (var slot in _slots)
            {
                slot.OnFilled -= IngridientSlotOnFilled;
                slot.OnCleared -= SlotOnCleared;
                slot.NotEmpty += SlotSlotNotEmpty;
            }

            _resultSlot.NotEmpty -= SlotSlotNotEmpty;
            _resultSlot.OnCleared -= ResultSlotOnCleared;

            ResetAll();
        }

        protected override void Refresh(Model model)
        {
            base.Refresh(model);

            HoldSlot(_resultSlot);
        }

        private void IngridientSlotOnFilled(UIItem uiItem, int index)
        {
            _componentUIItems[index] = uiItem;

            TryAssembly();
        }

        private bool TryAssembly()
        {
            if (_slots.All(slot => !slot.IsEmpty))
            {
                ClearSlot(_resultSlot, _resultUIItem);

                var meta = GetResult();

                if (meta)
                {
                    _resultUIItem = Instantiate(_uiItemPrefab, _resultSlot.ItemParent);
                    _resultUIItem.Setup(meta);
                    _resultSlot.Fill(_resultUIItem);

                    _resultUIItem.NewParentSetted += ResultNewParentSetted;

                    Assembled?.Invoke(meta);

                    return true;
                }
                else
                    HoldSlot(_resultSlot);
            }
            else
            {
                HoldSlot(_resultSlot);
            }

            AssemblyFailed?.Invoke();

            return false;
        }

        private void ResultNewParentSetted(UIItem uiItem)
        {
            uiItem.NewParentSetted -= ResultNewParentSetted;

            ClearIngridientSlots();
            ResetIngridients();
            ResetResult();
            HoldSlot(_resultSlot);
        }

        private void HoldSlot(UIItemSlot slot)
        {
            slot.SetItem(_uiItemPrefab);
        }

        private ItemMeta GetResult()
        {
            string[] ids = new string[_componentUIItems.Length];

            for (int i = 0; i < _componentUIItems.Length; i++)
                ids[i] = _componentUIItems[i].ItemMeta.Id;

            return _itemDataBase.Value.GetItemMeta.ByIngridients(ids);
        }

        private void SlotOnCleared(string itemID, int index)
        {
            _componentUIItems[index] = null;

            if(_resultUIItem != null)
            {
                ClearSlot(_resultSlot, _resultUIItem);
                ResetResult();
                HoldSlot(_resultSlot);
            }
        }

        private void ResultSlotOnCleared(string arg1, int index)
        {
            ClearIngridientSlots();
            ResetIngridients();

            for (int i = 0; i < _slots.Length; i++)
                HoldSlot(_slots[i]);
        }

        private void SlotSlotNotEmpty(UIItem uiItem) => uiItem.ResetParent();

        private void ClearIngridientSlots()
        {
            for(int i = 0; i < _componentUIItems.Length; i++)
            {
                ClearSlot(_slots[i], _componentUIItems[i]);
            }
        }

        private void ClearSlot(UIItemSlot uiSlot, UIItem uiItem)
        {
            if (uiItem)
                Destroy(uiItem.gameObject);

            uiSlot.Clear();
        }

        private void ResetIngridients()
        {
            for(int i = 0; i < _componentUIItems.Length; i++)
            {
                _componentUIItems[i] = null;
            }
        }

        private void ResetResult()
        {
            _resultUIItem = null;
        }

        private void ResetAll()
        {
            TryBackToInventory();

            ClearIngridientSlots();
            ClearSlot(_resultSlot, _resultUIItem);
            ResetIngridients();
            ResetResult();

            _widgetInventory.RefreshInstant();
        }

        private void TryBackToInventory()
        {
            if(_componentUIItems.Any(i => i != null))
            {
                for (int i = 0; i < _componentUIItems.Length; i++)
                {
                    if (_componentUIItems[i] != null)
                        BackToInventory(_componentUIItems[i].ItemMeta.Id);
                }
            }
            else if (_resultUIItem)
            {
                BackToInventory(_resultUIItem.ItemMeta.Id);
            }
        }

        private void BackToInventory(string itemId) => _inventory.Value.TryAddItem.Execute(this, itemId);
    }
}

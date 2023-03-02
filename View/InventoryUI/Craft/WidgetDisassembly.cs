using FromTheBasement.Domain.DataBase;
using FromTheBasement.Domain.InventorySystem;
using FromTheBasement.View.UserInterfaces.InventoryUI.InventoryItemUI;
using Lukomor.DIContainer;
using Lukomor.Presentation.Models;
using Lukomor.Presentation.Views.Widgets;
using Sirenix.Utilities;
using System;
using System.Linq;
using UnityEngine;

namespace FromTheBasement.View.UserInterfaces.InventoryUI.Craft
{
    public class WidgetDisassembly : Widget
    {
        [SerializeField] private UIItemSlot _disassembleableSlot;
        [SerializeField] private UIItemSlot[] _resultsSlots;
        [Space]
        [SerializeField] private UIItem _uiItemPrefab;
        [SerializeField] private WidgetInventory _widgetInventory;

        public event Action Disassembled;
        public event Action DisassemblyFailed;

        private readonly DIVar<ItemsDataBaseFeature> _itemDataBase = new DIVar<ItemsDataBaseFeature>();
        private readonly DIVar<InventoryFeature> _inventory = new DIVar<InventoryFeature>();

        private UIItem[] _resultItems;
        private UIItem _disassembleableItem;

        protected override void Install()
        {
            base.Install();

            _resultItems = new UIItem[_resultsSlots.Length];
        }

        protected override void Subscribe(Model model)
        {
            base.Subscribe(model);

            for (int i = 0; i < _resultsSlots.Length; i++)
            {
                _resultsSlots[i].SetIndex(i);
                _resultsSlots[i].OnFilled += ResultSlotOnFilled;
                _resultsSlots[i].NotEmpty += SlotNotEmpty;
                _resultsSlots[i].OnCleared += ResultSlotOnCleared;
            }

            _disassembleableSlot.NotEmpty += SlotNotEmpty;
            _disassembleableSlot.OnFilled += DisassembleableSlotOnFilled;
            _disassembleableSlot.OnCleared += DisassembleableSlotOnCleared;
        }

        protected override void Unsubscribe(Model model)
        {
            base.Unsubscribe(model);

            foreach (var slot in _resultsSlots)
            {
                slot.OnFilled -= ResultSlotOnFilled;
                slot.NotEmpty -= SlotNotEmpty;
                slot.OnCleared -= ResultSlotOnCleared;
            }

            _disassembleableSlot.NotEmpty -= SlotNotEmpty;
            _disassembleableSlot.OnFilled -= DisassembleableSlotOnFilled;
            _disassembleableSlot.OnCleared -= DisassembleableSlotOnCleared;

            ResetAll();          
        }

        protected override void Refresh(Model model)
        {
            base.Refresh(model);

            HoldResultSlots();

            _disassembleableSlot.Clear();
        }

        private bool TryDisassembly()
        {
            ClearResultSlots();

            if (TryGetResults(out string[] componentsIds))
            {              
                for (int i = 0; i < _resultsSlots.Length; i++)
                {
                    _resultItems[i] = Instantiate(_uiItemPrefab, _resultsSlots[i].ItemParent);
                    var meta = _itemDataBase.Value.GetItemMeta.ById(componentsIds[i]);

                    _resultItems[i].Setup(meta);
                    _resultsSlots[i].Fill(_resultItems[i]);

                    _resultItems[i].NewParentSetted += ResultNewParentSetted;
                }

                Disassembled?.Invoke();

                return true;
            }
            else
            {
                HoldResultSlots();

                DisassemblyFailed?.Invoke();

                return false;
            }
        }

        private bool TryGetResults(out string[] componentsIds)
        {
            var meta = _itemDataBase.Value.GetItemMeta.ById(_disassembleableItem.ItemMeta.Id);
            componentsIds = meta.ComponentsIds;

            return !componentsIds.IsNullOrEmpty();
        }

        private void HoldSlot(UIItemSlot slot)
        {
            slot.SetItem(_uiItemPrefab);
        }

        private void HoldResultSlots()
        {
            foreach (var result in _resultsSlots)
            {
                HoldSlot(result);
            }
        }

        private void ClearResultSlots()
        {
            foreach (var result in _resultsSlots)
            {
                result.Clear();
            }
        }

        private void ResultNewParentSetted(UIItem uiItem)
        {
            uiItem.NewParentSetted -= ResultNewParentSetted;

            if (_resultsSlots.All(rS => rS.IsEmpty))
                _disassembleableSlot.Clear();
        }

        private void DisassembleableSlotOnFilled(UIItem uiITem, int index)
        {
            _disassembleableItem = uiITem;

            TryDisassembly();
        }

        private void DisassembleableSlotOnCleared(string itemID, int index)
        {
            ResetDisassembleable();

            ClearResultsSlots();
            ResetResults();
            HoldResultSlots();
        }

        private void ResultSlotOnFilled(UIItem uiItem, int index)
        {
            _resultItems[index] = uiItem;
        }

        private void ResultSlotOnCleared(string itemID, int index)
        {
            _resultItems[index] = null;

            ClearSlot(_disassembleableSlot, _disassembleableItem);
            ResetDisassembleable();
            HoldSlot(_disassembleableSlot);
        }

        private void ResetDisassembleable()
        {
            _disassembleableItem = null;
        }

        private void ResetResults()
        {          
            for (int i = 0; i < _resultItems.Length; i++)
            {
                _resultItems[i] = null;
            }
        }

        private void ClearSlot(UIItemSlot uiSlot, UIItem uiItem)
        {
            if (uiItem)
                Destroy(uiItem.gameObject);

            uiSlot.Clear();
        }

        private void ClearResultsSlots()
        {
            for (int i = 0; i < _resultsSlots.Length; i++)
            {
                ClearSlot(_resultsSlots[i], _resultItems[i]);
            }
        }

        private void ResetAll()
        {
            TryBackToInventory();
            
            ClearResultsSlots();
            ClearSlot(_disassembleableSlot, _disassembleableItem);
            ResetDisassembleable();
            ResetResults();
            _widgetInventory.RefreshInstant();
        }

        private void TryBackToInventory()
        {
            if (_disassembleableItem)
            {
                BackToInventory(_disassembleableItem.ItemMeta.Id);
            }
            else if (_resultItems.Any(i => i != null))
            {
                for (int i = 0; i < _resultItems.Length; i++)
                {
                    if (_resultItems[i] != null)
                        BackToInventory(_resultItems[i].ItemMeta.Id);
                }
            }
        }

        private void BackToInventory(string itemId) => _inventory.Value.TryAddItem.Execute(this, itemId);

        private void SlotNotEmpty(UIItem uiItem) => uiItem.ResetParent();
    }
}

using System.Collections.Generic;
using FromTheBasement.Data.InventorySystem;
using FromTheBasement.Domain.DataBase;
using Lukomor.DIContainer;
using Lukomor.Presentation.Controllers;
using UnityEngine;

namespace FromTheBasement.View.UserInterfaces.InventoryUI
{
    public class WindowInventory : WindowWithEventSystemSelector<WindowInventoryModel> //не используется
    {
        [Space]
        [Header("Special parameters")]
        [SerializeField] private Transform _grid;
        [SerializeField] private UIItemSlot _itemSlotPrefab;

        private readonly DIVar<ItemsDataBaseFeature> _itemsDataBaseFeature = new DIVar<ItemsDataBaseFeature>();
        private List<UIItemSlot> _createdItemSlots;
        
        protected override Controller<WindowInventoryModel> CreateController()
        {
            return new WindowInventoryController(this);
        }

        public override void Install()
        {
            base.Install();
            
            _createdItemSlots = new List<UIItemSlot>();
        }


        public void FillWindow(ItemSlot[] itemCells)
        {
            foreach (var cell in itemCells)
            {
                var createdWidget = Instantiate(_itemSlotPrefab, _grid);
                var itemMeta = _itemsDataBaseFeature.Value.GetItemMeta.ById(cell.ItemId);
                
                //createdWidget.Setup(itemMeta);
                _createdItemSlots.Add(createdWidget);
            }
        }

        public void CleanWindow()
        {
            foreach(var itemSlot in _createdItemSlots)
            {
                Destroy(itemSlot.gameObject);
            }
            
            _createdItemSlots.Clear();
        }
    }
}
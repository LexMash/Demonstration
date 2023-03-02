using FromTheBasement.Domain.InventorySystem;
using Lukomor.DIContainer;
using Lukomor.Presentation.Controllers;

namespace FromTheBasement.View.UserInterfaces.InventoryUI
{
    public class WindowInventoryController : Controller<WindowInventoryModel>
    {
        private readonly WindowInventory _window;
        private readonly DIVar<InventoryFeature> _inventoryFeature = new DIVar<InventoryFeature>();

        public WindowInventoryController(WindowInventory window)
        {
            _window = window;
        }

        public override void Refresh(WindowInventoryModel model)
        {
            UpdateCellsInfo();
        }

        private void UpdateCellsInfo()
        {
            _window.CleanWindow();
            
            var itemCells = _inventoryFeature.Value.GetItemCell.All();

            _window.FillWindow(itemCells);
        }
    }
}

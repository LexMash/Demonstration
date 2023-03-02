using FromTheBasement.Domain.InputSystem;
using Lukomor.DIContainer;
using UnityEngine;

namespace FromTheBasement.View.UserInterfaces.InventoryUI.Craft
{
    public class CraftModeSelectorInputReceiver : InputReceiverBase
    {
        [SerializeField] private WidgetCraftModeSelector _selector;

        private void Awake()
        {
            DIVar<InputFeature> inputFeature = new DIVar<InputFeature>();
            var input = inputFeature.Value.GetGameInput.Execute();

            SetInputControl(input);
        }

        protected override void Subscribe()
        {
            var ui = _gameInput.UI;

            ui.ChangeCraftMode.performed += OnChangeCraftModePerformed;
        }

        protected override void Unsubscribe()
        {
            var ui = _gameInput.UI;
            ui.ChangeCraftMode.performed -= OnChangeCraftModePerformed;
        }

        private void OnChangeCraftModePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _selector.ChangeMode();
        }
    }
}

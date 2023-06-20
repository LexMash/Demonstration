using FromTheBasement.Domain.InputSystem;
using Lukomor.DIContainer;

namespace FromTheBasement.View.UserInterfaces.InventoryUI.Craft
{
    public class CraftInputReceiver : InputReceiverBase
    {
        private ICraftPanel _panel;

        private void Awake()
        {
            _panel = GetComponent<ICraftPanel>();

            DIVar<InputFeature> inputFeature = new DIVar<InputFeature>();
            var input = inputFeature.Value.GetGameInput.Execute();

            SetInputControl(input);
        }

        protected override void Subscribe()
        {
            var ui = _gameInput.UI;
            //ui.DoCraft.performed += OnDoCraftPerformed;
        }

        protected override void Unsubscribe()
        {
            var ui = _gameInput.UI;
            //ui.DoCraft.performed -= OnDoCraftPerformed;
        }

        private void OnDoCraftPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _panel.Craft();
        }
    }
}

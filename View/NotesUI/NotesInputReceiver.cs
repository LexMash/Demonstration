using FromTheBasement.Domain.InputSystem;
using Lukomor.DIContainer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FromTheBasement.View.UserInterfaces.NotesUI
{
    public class NotesInputReceiver : InputReceiverBase
    {
        [SerializeField] private Button _nextBtn;
        [SerializeField] private Button _prevBtn;

        private void Awake()
        {
            DIVar<InputFeature> inputFeature = new DIVar<InputFeature>();
            var input = inputFeature.Value.GetGameInput.Execute();

            SetInputControl(input);
        }

        protected override void Subscribe()
        {
            _gameInput.UI.PrevPage.performed += OnPrevPagePerformed;
            _gameInput.UI.NextPage.performed += OnNextPagePerformed;
        }

        protected override void Unsubscribe()
        {
            _gameInput.UI.PrevPage.performed -= OnPrevPagePerformed;
            _gameInput.UI.NextPage.performed -= OnNextPagePerformed;
        }

        private void OnNextPagePerformed(InputAction.CallbackContext obj)
        {
            if(_nextBtn.isActiveAndEnabled)
                _nextBtn.onClick?.Invoke();
        }

        private void OnPrevPagePerformed(InputAction.CallbackContext obj)
        {
            if (_prevBtn.isActiveAndEnabled)
                _prevBtn.onClick?.Invoke();
        }
    }
}

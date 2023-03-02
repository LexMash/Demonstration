using Lukomor.Presentation.Models;
using Lukomor.Presentation.Views.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace FromTheBasement.View.UserInterfaces.InventoryUI.Craft
{
    public class WidgetCraftModeSelector : Widget
    {
        [SerializeField] private WidgetAssembly _assemblyPanel;
        [SerializeField] private WidgetDisassembly _disassemblyPanel;

        [SerializeField] private Button _assemblyModeBtn;
        [SerializeField] private Image _assemblySelectIcon;

        [SerializeField] private Button _disassemblyModeBtn;
        [SerializeField] private Image _disassemblySelectIcon;

        public void ChangeMode()
        {
            if (_assemblyPanel.gameObject.activeSelf)
            {
                ActivateDisassemblyMode();
            }
            else
            {
                ActivateAssemblyMode();
            }
            
            IconModeSet();
        }

        protected override void Subscribe(Model model)
        {
            base.Subscribe(model);

            _assemblyModeBtn.onClick.AddListener(OnAssemblyButtonPressed);
            _disassemblyModeBtn.onClick.AddListener(OnDisassemblyButtonPressed);
        }

        protected override void Unsubscribe(Model model)
        {
            base.Unsubscribe(model);

            _assemblyModeBtn.onClick.RemoveListener(OnAssemblyButtonPressed);
            _disassemblyModeBtn.onClick.RemoveListener(OnDisassemblyButtonPressed);
        }

        protected override void Refresh(Model model)
        {
            base.Refresh(model);

            ActivateAssemblyMode();
            IconModeSet();
        }

        private void OnAssemblyButtonPressed()
        {
            if (!_assemblyPanel.gameObject.activeInHierarchy)
            {
                ActivateAssemblyMode();
                IconModeSet();
            }
        }

        private void OnDisassemblyButtonPressed()
        {
            if (!_disassemblyPanel.gameObject.activeInHierarchy)
            {
                ActivateDisassemblyMode();
                IconModeSet();
            }
        }

        private void ActivateAssemblyMode()
        {
            _assemblyPanel.gameObject.SetActive(true);
            _disassemblyPanel.gameObject.SetActive(false);
        }

        private void ActivateDisassemblyMode()
        {
            _disassemblyPanel.gameObject.SetActive(true);
            _assemblyPanel.gameObject.SetActive(false);
        }

        private void IconModeSet()
        {
            _assemblySelectIcon.enabled = _assemblyPanel.gameObject.activeInHierarchy;
            _disassemblySelectIcon.enabled = _disassemblyPanel.gameObject.activeInHierarchy;
        }
    }
}

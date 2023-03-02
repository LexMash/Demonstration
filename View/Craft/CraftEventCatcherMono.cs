using FromTheBasement.Data.InventorySystem;
using Lukomor.DIContainer;
using Lukomor.Presentation;
using Lukomor.Presentation.Views.Windows;
using UnityEngine;
using UnityEngine.Events;
using VavilichevGD.Tools.Async;

namespace FromTheBasement.View.UserInterfaces.Craft
{
    public class CraftEventCatcherMono : MonoBehaviour
    {
        public UnityEvent<ItemMeta> CraftedSomething;
        
        private readonly DIVar<UserInterface> _ui = new DIVar<UserInterface>();
        private bool _somethingCrafted = false;
        private ItemMeta _assembledItem;

        private async void OnEnable()
        {
            _somethingCrafted = false;
            
            await UnityAwaiters.WaitUntil(() => _ui.HasValue);
            
            _ui.Value.WindowShown += OnWindowShown;
            _ui.Value.WindowClosed += OnWindowClosed;
        }

        private void OnDisable()
        {
            if (_ui.HasValue)
            {
                _ui.Value.WindowShown -= OnWindowShown;
                _ui.Value.WindowClosed -= OnWindowClosed;
            }
        }
        
        private void OnWindowShown(IWindow window)
        {
            if (window is WindowConstructTable windowConstructTable)
            {
                windowConstructTable.WidgetAssembly.Assembled += OnAssembledSomething;
            }
        }

        private void OnWindowClosed(IWindow window)
        {
            if (window is WindowConstructTable windowConstructTable)
            {
                windowConstructTable.WidgetAssembly.Assembled -= OnAssembledSomething;

                if (_somethingCrafted)
                {
                    CraftedSomething?.Invoke(_assembledItem);
                }
            }
        }
        
        private void OnAssembledSomething(ItemMeta itemMeta)
        {
            _assembledItem = itemMeta;
            _somethingCrafted = true;
        }
    }
}
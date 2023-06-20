using System;
using FromTheBasement.Data.InventorySystem;
using FromTheBasement.Domain.CraftSystem;
using FromTheBasement.View.UserInterfaces.InventoryUI.Craft;
using Lukomor.Common;
using Lukomor.DIContainer;
using Lukomor.Presentation;
using Lukomor.Presentation.Views.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FromTheBasement.View.UserInterfaces.Craft
{

    public static class WindowConstructTableExtension
    {
        public static void ShowWindowConstructTable(this UserInterface ui, string constructID)
        {
            var payload = new Payload(WindowConstructTable.Keys.ConstructTable, constructID);
            ui.ShowWindow<WindowConstructTable>(payload);
        }
    }
    public class WindowConstructTable : DialogWindow
    {
        public static class Keys
        {
            public static readonly string ConstructTable = "ConstructTable";
        }

        [SerializeField] private TextMeshProUGUI _labelTxt;
        [SerializeField] private Image _iconImg;
        [SerializeField] private WidgetAssembly _widgetAssembly;

        public WidgetAssembly WidgetAssembly => _widgetAssembly;

        private readonly DIVar<CraftFactoryFeature> _craftFeature = new DIVar<CraftFactoryFeature>();

        private string _constructID;

        public override void Refresh()
        {
            base.Refresh();

            _constructID = GetPayload<string>(Keys.ConstructTable);

            var factory = _craftFeature.Value.GetFactory.Execute(_constructID);

            _labelTxt.text = factory.Id;
            _iconImg.sprite = factory.Icon;
            _iconImg.preserveAspect = true;
        }
    }
}

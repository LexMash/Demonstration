using FromTheBasement.View.UserInterfaces.InventoryUI.InventoryItemUI;
using Lukomor.Presentation.Views.Widgets;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FromTheBasement.View.UserInterfaces.InventoryUI
{
    public class UIItemSlot : Widget, IDropHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [field:SerializeField] public RectTransform ItemParent { get; private set; }

        public event Action<UIItem, int> OnFilled;
        public event Action<string, int> OnCleared;
        public event Action<UIItem> NotEmpty;

        public event Action<string> OnHovered;
        public event Action OnStopedHovering;

        public int Index { get; private set; }
        public bool IsEmpty => _uiItem == null;

        public UIItem UIItem => _uiItem; //костыль

        private UIItem _uiItem = null;

        public void SetIndex(int index) => Index = index;

        public void SetItem(UIItem uiItem)
        {
            _uiItem = uiItem;

            _uiItem.OnItemBeginDrag += OnItemBeginDrag;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var uiItem = eventData.pointerDrag.GetComponent<UIItem>();
                       
            if (IsEmpty)
            {
                SetChildren(uiItem);

                Fill(uiItem);
            }           
            else
                NotEmpty?.Invoke(uiItem);
        }

        public void Fill(UIItem uiItem)
        {
            SetItem(uiItem);
            OnFilled?.Invoke(uiItem, Index);
        }

        public void Clear()
        {
            if(_uiItem)
                _uiItem.OnItemBeginDrag -= OnItemBeginDrag;

            _uiItem = null;
        }

        public void SetChildren(UIItem uiItem)
        {
            var itemTransform = uiItem.gameObject.GetComponent<RectTransform>();
            itemTransform.SetParent(ItemParent, false);
            itemTransform.localPosition = Vector3.zero;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsEmpty)
            {
                var titleCode = _uiItem.ItemMeta != null ? _uiItem.ItemMeta.DescriptionCode : "";
                
                OnHovered?.Invoke(titleCode);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(!IsEmpty)
                OnStopedHovering?.Invoke();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (!IsEmpty)
            {
                var description = _uiItem.ItemMeta != null ? _uiItem.ItemMeta.DescriptionCode : "";

                OnHovered?.Invoke(description);
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (!IsEmpty)
                OnStopedHovering?.Invoke();
        }

        public void ClearInvokeEvent(string itemId) => OnCleared?.Invoke(itemId, Index);

        private void OnItemBeginDrag(string itemId)
        {
            Clear();
            ClearInvokeEvent(itemId);
        }
    }
}

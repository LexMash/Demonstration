using FromTheBasement.Data.InventorySystem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FromTheBasement.View.UserInterfaces.InventoryUI.InventoryItemUI
{
    public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _icon;

        public event Action<string> OnItemBeginDrag;
        public event Action<UIItem> NewParentSetted;

        public ItemMeta ItemMeta { get; private set; }

        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private Transform _parentTransform;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Setup(ItemMeta itemMeta)
        {
            ItemMeta = itemMeta;
            _icon.sprite = itemMeta.Icon;
            _icon.preserveAspect = true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;

            _parentTransform = _rectTransform.parent;

            OnItemBeginDrag?.Invoke(ItemMeta.Id);

            _rectTransform.SetParent(_canvas.transform);
            _rectTransform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;

            if (!eventData.pointerEnter.GetComponentInParent<UIItemSlot>())
                ResetParent();
            else 
                NewParentSettedInvoke();
        }

        public void NewParentSettedInvoke()
        {
            if (_parentTransform != _rectTransform.parent)
                NewParentSetted?.Invoke(this);
        }

        public void ResetParent()
        {
            _canvasGroup.blocksRaycasts = true;
            var slot = _parentTransform.GetComponentInParent<UIItemSlot>();
            slot.SetChildren(this);
            slot.Fill(this);
            //_rectTransform.transform.SetParent(_parentTransform);
            //_rectTransform.localPosition = Vector3.zero;
        }
    }
}

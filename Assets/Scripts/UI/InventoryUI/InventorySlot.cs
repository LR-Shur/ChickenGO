// InventorySlot.cs
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;
using Inventory;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class InventorySlot : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
    {
        [Header("UI 引用")]
        public Image icon;                 
        public TextMeshProUGUI nameText;   
        public Image highlightFrame;       

        // 当前格子的物品实例
        [HideInInspector] public ItemInstance item;

        // 点击 & 悬浮事件
        public UnityEvent<InventorySlot> onClick;
        public event Action<InventorySlot> onHoverEnter;
        public event Action<InventorySlot> onHoverExit;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => onClick?.Invoke(this));
            highlightFrame.gameObject.SetActive(false);
        }

        public void Setup(ItemInstance inst)
        {
            item = inst;
            icon.sprite = inst.Def.icon;
            icon.enabled = true;
            nameText.text = inst.Def.displayName.ToString();
        }

        public void Clear()
        {
            item = null;
            icon.enabled = false;
            nameText.text = "";
            Unhighlight();
        }

        public void Highlight()   => highlightFrame.gameObject.SetActive(true);
        public void Unhighlight() => highlightFrame.gameObject.SetActive(false);

        // 悬浮回调
        public void OnPointerEnter(PointerEventData e)
            => onHoverEnter?.Invoke(this);
        public void OnPointerExit(PointerEventData e)
            => onHoverExit?.Invoke(this);
    }
}

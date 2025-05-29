// InventoryUI.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryUI : BaseUI
    {
        [Header("槽位列表，索引对应 InventoryHandle 的 key")]
        public List<InventorySlot> slots;

        [Header("操作面板")]
        public GameObject actionPanel;
        public Button useButton;
        public Button discardButton;

        [Header("提示面板")]
        public RectTransform tooltipPanel;
        public TextMeshProUGUI tooltipDescText;
        public Vector2 tooltipOffset = new Vector2(0, 30);

        private InventoryHandle invHandle;
        private int selectedSlotIndex = -1;
        private InventorySlot selectedSlot;

        private void Awake()
        {
            invHandle = FindObjectOfType<InventoryHandle>();
            actionPanel.SetActive(false);
            tooltipPanel.gameObject.SetActive(false);

            foreach (var slot in slots)
            {
                slot.onClick.AddListener(OnSlotClicked);
                slot.onHoverEnter += OnSlotHoverEnter;
                slot.onHoverExit += OnSlotHoverExit;
            }

            // 首次填充
            Refresh(invHandle.GetAllItems());
        }

        private void OnEnable()
        {
            invHandle.OnInventoryChanged += Refresh;
        }

        private void OnDisable()
        {
            invHandle.OnInventoryChanged -= Refresh;
        }

        // itemsBySlot: key = 槽位索引，value = 对应的 ItemInstance
        private void Refresh(Dictionary<int, ItemInstance> itemsBySlot)
        {
            // 按 slots 列表索引，填或清
            for (int i = 0; i < slots.Count; i++)
            {
                if (itemsBySlot.TryGetValue(i, out var inst))
                    slots[i].Setup(inst);
                else
                    slots[i].Clear();
            }

            // 收起面板
            selectedSlotIndex = -1;
            actionPanel.SetActive(false);
            tooltipPanel.gameObject.SetActive(false);
        }

        private void OnSlotClicked(InventorySlot slot)
        {
            int idx = slots.IndexOf(slot);
            if (idx < 0) return;

            // 点击同格子则取消
            if (selectedSlotIndex == idx)
            {
                slot.Unhighlight();
                selectedSlotIndex = -1;
                actionPanel.SetActive(false);
                return;
            }

            // 切换高亮
            selectedSlot?.Unhighlight();
            slot.Highlight();
            selectedSlot = slot;
            selectedSlotIndex = idx;

            // 确保该格有物品
            if (slot.item == null)
            {
                actionPanel.SetActive(false);
                return;
            }

            // 显示操作面板
            actionPanel.SetActive(true);
            useButton.gameObject.SetActive(slot.item.Def.canUse);
            discardButton.gameObject.SetActive(true);

            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(OnUseClicked);
            discardButton.onClick.RemoveAllListeners();
            discardButton.onClick.AddListener(OnDiscardClicked);
        }

        private void OnUseClicked()
        {
            invHandle.UseSlot(selectedSlotIndex);
            actionPanel.SetActive(false);
        }

        private void OnDiscardClicked()
        {
            invHandle.RemoveSlot(selectedSlotIndex);
            actionPanel.SetActive(false);
        }

        private void OnSlotHoverEnter(InventorySlot slot)
        {
            int idx = slots.IndexOf(slot);
            if (idx < 0) return;
            var itemsBySlot = invHandle.GetAllItems();
            if (!itemsBySlot.TryGetValue(idx, out var inst)) return;

            tooltipDescText.text = inst.Def.description;

            // 面板移到格子上方一点
            var slotRect = slot.GetComponent<RectTransform>();
            tooltipPanel.anchoredPosition = slotRect.anchoredPosition + tooltipOffset;
            tooltipPanel.gameObject.SetActive(true);
        }

        private void OnSlotHoverExit(InventorySlot slot)
        {
            tooltipPanel.gameObject.SetActive(false);
        }
    }
}

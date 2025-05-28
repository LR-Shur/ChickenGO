using Inventory;
using TMPro;

namespace UI
{
    using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : BaseUI
{
    [Header("槽位列表")]
    public List<InventorySlot> slots;

    [Header("操作面板")]
    public GameObject actionPanel;     // 弹出面板（包含 Use、Discard 按钮）
    public Button useButton;
    public Button discardButton;

    private InventoryHandle invHandle;
    private int    selectedItemId;
    private InventorySlot selectedSlot;

    private void Awake()
    {
        invHandle = FindObjectOfType<InventoryHandle>();
        actionPanel.SetActive(false);

        // 给每个槽位订阅点击事件
        foreach (var slot in slots)
            slot.onClick.AddListener(OnSlotClicked);
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

    private void Refresh(Dictionary<int, ItemInstance> items)
    {
        // 清空所有
        foreach (var slot in slots)
            slot.Clear();

        // 填充
        int idx=0;
        foreach (var kv in items)
        {
            if (idx >= slots.Count) break;
            var inst = kv.Value;
            slots[idx].Setup(inst.Def.icon, inst.Def.displayName);
            idx++; 
        }

        // 关闭操作面板
        actionPanel.SetActive(false);
    }

    private void OnSlotClicked(InventorySlot slot)
    {
        // 如果点的是当前已选槽，则取消高亮并收起面板
        if (selectedSlot == slot)
        {
            slot.Unhighlight();
            selectedSlot = null;
            CloseActionPanel();
            return;
        }
        
        
        // 取消上次高亮
        selectedSlot?.Unhighlight();
        // 高亮当前
        slot.Highlight();
        selectedSlot = slot;

        // 获取对应的 ItemInstance
        int index = slots.IndexOf(slot);
        var itemsList = invHandle.GetAllItems(); // 假设返回按插入顺序的 List<ItemInstance>
        if (index < 0 || index >= itemsList.Count)
            return;

        var inst = itemsList[index];
        selectedItemId = inst.Def.itemId;

        // 显示操作面板，并根据 activeBuffs 决定 Use 按钮显隐
        actionPanel.SetActive(true);
        useButton.gameObject.SetActive(inst.Def.hasActive);
        discardButton.gameObject.SetActive(true);

        // 绑定按钮回调
        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(OnUseClicked);
        discardButton.onClick.RemoveAllListeners();
        discardButton.onClick.AddListener(OnDiscardClicked);

        // 把面板放到槽位旁边（可选，根据需求调整 RectTransform）
        //actionPanel.transform.position = slot.transform.position + Vector3.up * 30;
    }

    private void OnUseClicked()
    {
        invHandle.Use(selectedItemId);
        CloseActionPanel();
    }

    private void OnDiscardClicked()
    {
        invHandle.RemoveCompletely(selectedItemId); // 需要在 InventoryHandle 提供此方法
        CloseActionPanel();
    }

    private void CloseActionPanel()
    {
        selectedSlot?.Unhighlight();
        actionPanel.SetActive(false);
    }

    public override void Show()
    {
        base.Show();
        
    }
}


}

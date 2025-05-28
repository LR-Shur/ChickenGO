using System;
using System.Collections.Generic;
using System.Linq;
using Buff;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Inventory
{
    [RequireComponent(typeof(BuffHandle))]
    public class InventoryHandle : MonoBehaviour
    {
        public int maxSlots = 5;
        private BuffHandle buffHandle;
        private Dictionary<int, ItemInstance> items = new();
        public event Action<Dictionary<int,ItemInstance>> OnInventoryChanged;

        void Awake()
        {
            buffHandle = GetComponent<BuffHandle>();
        }

        /// <summary> 拾取道具 </summary>
        public void Pickup(ItemInfo info)
        {
            // 1. 通过 Addressables 异步加载 Definition
            Addressables.LoadAssetAsync<ItemDefinition>($"Item/{info.itemId}").Completed += handle =>
            {
                var def = handle.Result;
                if (def == null) return;

                // 2. 如果已存在，叠加数量；否则新建实例
                if (items.TryGetValue(def.itemId, out var inst))
                {
                    inst.AddCount(info.count);
                }
                else
                {
                    inst = new ItemInstance(def, info.count);
                    items[def.itemId] = inst;
                }

                // 3. 如果有被动效果，逐条注册到 BuffHandle
                if (def.hasPassive)
                    foreach (var bi in def.passiveBuffs)
                        buffHandle.Add(bi);

                // 4. 更新 UI（图标/数量）
                OnInventoryChanged?.Invoke(items);
            };
        }

        /// <summary> 使用道具 </summary>
        public void Use(int itemId)
        {
            if (!items.TryGetValue(itemId, out var inst) || inst.count == 0) return;

            var def = inst.Def;
            if (def.hasActive)
            {
                // 触发所有主动 Buff
                foreach (var bi in def.activeBuffs)
                    buffHandle.Add(bi);
            }

            if (def.isConsumable)
            {
                inst.AddCount(-1);
                if (inst.count <= 0)
                    items.Remove(itemId);
            }

            OnInventoryChanged?.Invoke(items);
        }

        public List<ItemInstance> GetAllItemsList() => items.Values.ToList();
        public Dictionary<int, ItemInstance> GetAllItems() => items;

        
        //丢弃物品
        public void RemoveCompletely(int itemId)
        {
            if (items.Remove(itemId))
                OnInventoryChanged?.Invoke(items);
        }

        
    }

}

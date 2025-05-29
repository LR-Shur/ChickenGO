using System;
using System.Collections.Generic;
using System.Linq;
using Buff;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Inventory
{
    [RequireComponent(typeof(BuffHandle))]
    public class InventoryHandle : MonoBehaviour
    {
        public int maxSlots = 20;
        private BuffHandle buffHandle;
        // key = 槽位索引 (0..maxSlots-1)，value = 该槽位里的 ItemInstance
        public Dictionary<int, ItemInstance> items = new Dictionary<int, ItemInstance>();

        // 每个 ItemInstance & 它的被动 buffs
        private Dictionary<ItemInstance, List<BuffInstance>> passiveBuffMap
            = new Dictionary<ItemInstance, List<BuffInstance>>();

        public event Action<Dictionary<int, ItemInstance>> OnInventoryChanged;

        void Awake()
        {
            buffHandle = GetComponent<BuffHandle>();
        }

        /// <summary> 拾取道具：叠满格子后自动开新格 </summary>
        public void Pickup(ItemInfo info)
        {
            var def = ItemDefinitionManager.Instance.Get(info.itemId);
                if (def == null) return;

                int toAdd = info.count;
                int maxStack = Mathf.Max(1, def.maxStack);

                // 1. 先往已有同类型且未满的槽位里填
                foreach (var kv in items.OrderBy(kv => kv.Key))
                {
                    var slot    = kv.Key;
                    var inst    = kv.Value;
                    if (inst.Def.itemId != def.itemId) continue;
                    int space   = maxStack - inst.count;
                    if (space <= 0) continue;
                    int delta   = Mathf.Min(space, toAdd);
                    inst.AddCount(delta);
                    toAdd -= delta;
                    if (toAdd <= 0) break;
                }

                // 2. 如果还有剩余，就分批找空槽开新格
                while (toAdd > 0 && items.Count < maxSlots)
                {
                    int delta = Mathf.Min(maxStack, toAdd);
                    var inst  = new ItemInstance(def, delta);
                    toAdd    -= delta;

                    // 找一个最小的空位 key
                    int slotKey = Enumerable.Range(0, maxSlots)
                                            .First(i => !items.ContainsKey(i));
                    items[slotKey] = inst;

                    // 如果有被动 Buff，记下来
                    if (def.hasPassive)
                    {
                        var buffs = new List<BuffInstance>();
                        foreach (var entry in def.passiveBuffs)
                        {
                            var b = buffHandle.Add(
                                new BuffInfo(entry.definition.buffId, entry.stacks)
                            );
                            if (b != null) buffs.Add(b);
                        }
                        passiveBuffMap[inst] = buffs;
                    }
                }

                // 3. 如果 toAdd>0，说明背包已满，可在这里提示玩家
                if (toAdd > 0)
                    Debug.LogWarning("背包已满，溢出部分已丢弃");

                OnInventoryChanged?.Invoke(new Dictionary<int, ItemInstance>(items));
            
        }

        /// <summary> 使用槽位上的道具（传入槽位索引）</summary>
        public void UseSlot(int slot)
        {
            if (!items.TryGetValue(slot, out var inst)) return;
            var def = inst.Def;
            if (!def.canUse) return;

            // 应用主动 Buff
            foreach (var entry in def.activeBuffs)
                buffHandle.Add(new BuffInfo(entry.definition.buffId, entry.stacks));

            if (def.isConsumable)
            {
                inst.AddCount(-1);
                if (inst.count <= 0)
                    RemoveSlot(slot);
            }

            OnInventoryChanged?.Invoke(new Dictionary<int, ItemInstance>(items));
        }

        /// <summary> 丢弃整个槽位（连带被动 Buff） </summary>
        public void RemoveSlot(int slot)
        {
            if (!items.TryGetValue(slot, out var inst)) return;
            // 1. 清除被动 Buff
            if (passiveBuffMap.TryGetValue(inst, out var buffs))
            {
                foreach (var b in buffs)
                    buffHandle.Remove(b);
                passiveBuffMap.Remove(inst);
            }
            // 2. 清空槽位
            items.Remove(slot);
            OnInventoryChanged?.Invoke(new Dictionary<int, ItemInstance>(items));
        }
        
        public Dictionary<int, ItemInstance> GetAllItems() => items;
    }
}

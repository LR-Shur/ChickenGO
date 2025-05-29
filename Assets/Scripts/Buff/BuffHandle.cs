// BuffHandle.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Player;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Buff
{
    [RequireComponent(typeof(PlayerStats))]
    public class BuffHandle : MonoBehaviour
    {
        private Dictionary<int, BuffInstance> buffs = new Dictionary<int, BuffInstance>();

        void Update()
        {
            float dt = Time.deltaTime;
            var toExpire = new List<int>();

            foreach (var kv in buffs) {
                var inst = kv.Value;
                // 每帧调用 OnTick
                inst.Def.behavior?.OnTick(gameObject, inst.Stacks, dt);
                // 更新倒计时并检测衰减
                inst.Tick(dt);
                if (inst.Expired)
                    toExpire.Add(kv.Key);
            }

            // 统一处理过期回收
            foreach (var id in toExpire)
                Remove(id);
        }

        public BuffInstance Add(BuffInfo info)
        {
            
            var def = BuffDefinitionManager.Instance.Get(info.buffId);
            if (def == null)
                return null;

            // 如果已经有这个 buff，就叠层并再触发一次 OnApply
            if (buffs.TryGetValue(def.buffId, out var inst))
            {
                inst.Add(info.stacks);
                def.behavior?.OnApply(gameObject, inst.Stacks);
            }
            else
            {
                // 新建实例、存字典、触发 OnApply
                inst = new BuffInstance(def, info.stacks);
                buffs[def.buffId] = inst;
                def.behavior?.OnApply(gameObject, inst.Stacks);
            }
            return inst;
        }

        public void Remove(int buffId)
        {
            if (!buffs.TryGetValue(buffId, out var inst)) return;
            // Buff 真正结束时调用 OnExpire
            inst.Def.behavior?.OnExpire(gameObject);
            buffs.Remove(buffId);
        }
        
        //重载
        public void Remove(BuffInstance inst)
        {
            if (inst == null) return;
            // 从字典里确保移除同一个实例
            var id = inst.Def.buffId;
            if (buffs.TryGetValue(id, out var existing) && existing == inst)
            {
                inst.Def.behavior?.OnExpire(gameObject);
                buffs.Remove(id);
            }
            else
            {
                // 要么字典里已经不在，还是调用一次失效
                inst.Def.behavior?.OnExpire(gameObject);
            }
        }

        public bool Has(int buffId) => buffs.ContainsKey(buffId);

        // 暴露当前所有实例，供结算等使用
        public IEnumerable<BuffInstance> GetAllInstances() => buffs.Values;


        //异步添加buff,关键是带返回值
        public async Task<BuffInstance> AddAsync(BuffInfo info)
        {
            var handle = Addressables.LoadAssetAsync<BuffDefinition>(info.buffId);
            var def = await handle.Task;
            if (def == null) return null;

            BuffInstance inst;
            if (buffs.TryGetValue(def.buffId, out inst)) {
                inst.Add(info.stacks);
                def.behavior?.OnApply(gameObject, inst.Stacks);
            }
            else {
                inst = new BuffInstance(def, info.stacks);
                buffs[def.buffId] = inst;
                def.behavior?.OnApply(gameObject, inst.Stacks);
            }

            return inst;
        }
    }
}

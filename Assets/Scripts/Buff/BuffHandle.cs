using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Buff
{
    // 4. BuffHandle：异步加载Definition并管理Buff
    [RequireComponent(typeof(PlayerController))]
    public class BuffHandle : MonoBehaviour
    {
        private Dictionary<int, BuffInstance> buffs = new Dictionary<int, BuffInstance>();
        private PlayerController player;

        void Awake() => player = GetComponent<PlayerController>();
        void Update()
        {
            float dt = Time.deltaTime;
            var toExpire = new List<int>();
            foreach (var kv in buffs)
            {
                var inst = kv.Value;
                inst.Def.behavior?.OnTick(gameObject, inst.Stacks, dt);
                inst.Tick(dt);
                if (inst.Expired) toExpire.Add(kv.Key);
            }
            foreach (var id in toExpire) Remove(id);
        }

        // 添加BuffInfo
        public void Add(BuffInfo info)
        {
            Addressables.LoadAssetAsync<BuffDefinition>(info.buffId).Completed += handle =>
            {
                var def = handle.Result; if (def == null) return;
                if (buffs.TryGetValue(def.buffId, out var inst))
                {
                    inst.Add(info.stacks);
                    def.behavior?.OnApply(gameObject, inst.Stacks);
                }
                else
                {
                    inst = new BuffInstance(def, info.stacks);
                    buffs[def.buffId] = inst;
                    def.behavior?.OnApply(gameObject, inst.Stacks);
                }
            };
        }

        // 移除所有层
        public void Remove(int buffId)
        {
            if (!buffs.TryGetValue(buffId, out var inst)) return;
            inst.Def.behavior?.OnExpire(gameObject);
            buffs.Remove(buffId);
        }
        

        public bool HasBuff(int buffId) => buffs.ContainsKey(buffId);
    }
}
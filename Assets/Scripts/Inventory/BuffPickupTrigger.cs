using Buff;
using Player;
using UnityEngine;

namespace Inventory
{
    [RequireComponent(typeof(Collider2D))]
    public class BuffPickupTrigger : MonoBehaviour
    {
        [Header("要触发的 Buff 列表")]
        public BuffEntry[] buffEntries;

        private void Awake()
        {
            // 确保 Collider2D 为 Trigger
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<PlayerController>();
            if (player == null /*|| !player.isLocalPlayer*/) return;

            var bh = player.GetComponent<BuffHandle>();
            foreach (var entry in buffEntries)
            {
                // 从 entry 构造 BuffInfo 并添加
                var def = entry.definition;
                if (def != null)
                {
                    bh.Add(new BuffInfo(def.buffId, entry.stacks));
                }
            }

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 用于 Inspector 中配置一个 BuffDefinition + 初始层数
    /// </summary>
    [System.Serializable]
    public struct BuffEntry
    {
        [Tooltip("要触发的 BuffDefinition 资产")]
        public BuffDefinition definition;
        [Tooltip("触发时的初始层数")]
        public int stacks;
    }
}

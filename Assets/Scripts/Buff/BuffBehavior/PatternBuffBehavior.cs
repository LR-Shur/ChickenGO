using UnityEngine;
using Player;  // 包含 EggPattern 和 PatternSpawnController

namespace Buff
{
    [CreateAssetMenu(menuName = "Buff/Behaviors/PatternProbability")]
    public class PatternBuffBehavior : BuffBehavior
    {
        [Header("要增强的花纹类型")]
        public EggPattern patternType;

        [Header("每层带来的概率加成（0 ~ 1）")]
        [Range(0f, 1f)]
        public float incrementPct = 0.1f;

        /// <summary>
        /// 当 Buff 生效时，给 PatternSpawnController 增加相应的加成
        /// </summary>
        /// <param name="target">挂有 PatternSpawnController 的 GameObject（通常是玩家）</param>
        /// <param name="stacks">Buff 层数</param>
        public override void OnApply(GameObject target, int stacks)
        {
            var controller = target.GetComponent<PlayerController>().stats.eggStats;
            if (controller == null)
            {
                Debug.LogWarning($"[PatternBuff] 目标 {target.name} 上未找到 PatternSpawnController，无法添加花纹加成。");
                return;
            }
            // 累加 stacks 倍的增量
            controller.AddPatternBonus(patternType, incrementPct * stacks);
        }

        
        /// <summary>
        /// 当 Buff 失效时，移除一次单层的加成
        /// </summary>
        public override void OnExpire(GameObject target)
        {
            var controller = target.GetComponent<PlayerController>().stats.eggStats;
            if (controller == null)
            {
                Debug.LogWarning($"[PatternBuff] 目标 {target.name} 上未找到 PatternSpawnController，无法移除花纹加成。");
                return;
            }
            // 只移除一层的增量
            controller.RemovePatternBonus(patternType, incrementPct);
        }
    }
}

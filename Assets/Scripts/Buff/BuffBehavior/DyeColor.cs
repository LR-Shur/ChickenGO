// DyeBehavior.cs
using UnityEngine;
using Buff;
using Player;



[CreateAssetMenu(menuName = "Buff/Behaviors/DyeBehavior")]
public class DyeBehavior : BuffBehavior
{
    [Header("染料类型")]
    public EggColor color;

    [Header("每层增加的百分比（如 0.05 表示 5%）")]
    public float percentPerStack = 0.05f;
    

    public override void OnApply(GameObject target, int stacks)
    {
        var stats = target.GetComponent<PlayerStats>();
        if (stats == null) return;

        // 计算这次染料的目标百分比
        float newPercent = percentPerStack * stacks;

        if (stats.eggStats.color == color)
        {
            // 同色：取更大百分比
            stats.eggStats.eggColorScore = Mathf.Max(stats.eggStats.eggColorScore, newPercent);
        }
        else
        {
            // 异色：只有当新百分比更大时才覆盖
            if (newPercent > stats.eggStats.eggColorScore)
            {
                stats.eggStats.color    = color;
                stats.eggStats.eggColorScore = newPercent;
            }
        }
    }

    public override void OnTick(GameObject target, int stacks, float dt)
    {
        // 染料永久，无持续效果
    }

    public override void OnExpire(GameObject target)
    {
        // 永久染料一般 isInfinite，所以 OnExpire 不被调用
    }
}

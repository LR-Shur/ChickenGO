namespace Buff
{
    using UnityEngine;
using Buff;
using Player;

public enum StatType { Speed, Attack, Defense, Health }

public enum ModifierOp { Add, Multiply }

[CreateAssetMenu(menuName = "Buff/Behaviors/StatBuff")]
public class StatBuffBehavior : BuffBehavior
{
    [Header("要修改的属性")]
    public StatType    stat;

    [Header("运算方式")]
    public ModifierOp  operation = ModifierOp.Add;

    [Header("操作数：Add 时是加值-1就是减少一点，Multiply 时是加的倍率，如0.2，就会加上base*0.2")]
    public float       value = 1f;
    
    //记录下一开始的加/减值
    float influenceValue; 

    public override void OnApply(GameObject target, int stacks)
    {
        var stats = target.GetComponent<PlayerStats>();
        if (stats == null) return;

        // 计算实际作用值
        if (operation == ModifierOp.Add)
        {
            float influenceValue = value * stacks;
            
        }
        else // Multiply
        {
            switch (stat)
            {
            case StatType.Speed:
                influenceValue = stats.baseSpeed * value;
                break;
            case StatType.Attack:
                influenceValue = stats.baseAttack * value;
                break;
            case StatType.Defense:
                influenceValue = stats.baseDefense * value;
                break;
            case StatType.Health:
                influenceValue = stats.baseMaxHealth * value;
                break;
            }
        }
        
        
        // 应用到 PlayerStats
        switch (stat) {
        case StatType.Speed:
            stats.AddSpeedBonus(Mathf.RoundToInt(influenceValue));
            break;
        case StatType.Attack:
            stats.AddAttackBonus(Mathf.RoundToInt(influenceValue));
            break;
        case StatType.Defense:
            stats.AddDefenseBonus(Mathf.RoundToInt(influenceValue));
            break;
        case StatType.Health:
            stats.AddHealthBonus(Mathf.RoundToInt(influenceValue));
            break;
        }
    }

    public override void OnTick(GameObject target, int stacks, float dt) { }

    public override void OnExpire(GameObject target)
    {
        var stats = target.GetComponent<CharacterStatsBase>();
        if (stats == null) return;

        // 过期时移除刚才加的那段 influenceValue
        switch (stat)
        {
        case StatType.Speed:
            stats.RemoveSpeedBonus(Mathf.RoundToInt(influenceValue));
            break;
        case StatType.Attack:
            stats.RemoveAttackBonus(Mathf.RoundToInt(influenceValue));
            break;
        case StatType.Defense:
            stats.RemoveDefenseBonus(Mathf.RoundToInt(influenceValue));
            break;
        case StatType.Health:
            stats.RemoveHealthBonus(Mathf.RoundToInt(influenceValue));
            break;
        }
    }
}

}

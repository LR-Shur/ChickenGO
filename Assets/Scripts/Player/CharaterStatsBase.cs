// CharacterStatsBase.cs（抽象类方案）
using UnityEngine;

[RequireComponent(typeof(Buff.BuffHandle))]
public abstract class CharacterStatsBase : MonoBehaviour
{
    [Header("基础属性")]
    public int baseSpeed = 1;
    public int baseAttack = 2;
    public int baseDefense = 0;
    public int baseMaxHealth = 20;

    [HideInInspector] public int currentHealth;
    [HideInInspector] public int speedBonus = 0;
    [HideInInspector] public int attackBonus = 0;
    [HideInInspector] public int defenseBonus = 0;
    [HideInInspector] public int healthBonus = 0;

    protected virtual void Awake()
    {
        currentHealth = baseMaxHealth;
    }

    // 只读计算属性
    public int Speed => baseSpeed + speedBonus;
    public int Attack => baseAttack + attackBonus;
    public int Defense => baseDefense + defenseBonus;
    public int MaxHealth => baseMaxHealth + healthBonus;

    // Buff 操作接口
    public void AddSpeedBonus(int a)    => speedBonus += a;
    public void RemoveSpeedBonus(int a) => speedBonus -= a;
    public void AddAttackBonus(int a)        => attackBonus += a;
    public void RemoveAttackBonus(int a)     => attackBonus -= a;
    public void AddDefenseBonus(int v)     => defenseBonus += v;
    public void RemoveDefenseBonus(int v)  => defenseBonus -= v;
    public void AddHealthBonus(int h)      => healthBonus += h;
    public void RemoveHealthBonus(int h)   => healthBonus -= h;
}

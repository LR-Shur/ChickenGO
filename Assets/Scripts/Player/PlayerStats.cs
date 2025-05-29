// PlayerStats.cs

using Buff;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(Buff.BuffHandle))]
    public class PlayerStats : MonoBehaviour
    {
        [Header("基础属性")]
        public int baseSpeed     = 1;
        public int   baseAttack    = 2;
        public int   baseDefense     = 0; 
        public int baseMaxHealth = 20;
        
        
        [Header("当前的蛋属性")]
        public EggStats eggStats = new EggStats();
        


        [HideInInspector] public int currentHealth;

        // 运行时属性缓存，由 BuffBehavior 修改
        [HideInInspector] public int speedBonus = 0;
        [HideInInspector] public int   attackBonus   = 0;
        [HideInInspector] public int   defenseBonus  = 0;
        [HideInInspector] public int healthBonus   = 0;

        private void Awake()
        {
            currentHealth = baseMaxHealth;
        }

        // 外部只能读，不可随意写
        public int Speed    => baseSpeed+speedBonus;
        public int   Attack   => baseAttack + attackBonus;
        public int   Defense    => baseDefense + defenseBonus;
        public int MaxHealth => baseMaxHealth + healthBonus;

        
        //属性外部加值
        public void AddSpeedBonus(int a)    => speedBonus += a;
        public void RemoveSpeedBonus(int r) => speedBonus -= r;
        

        public void AddAttackBonus(int a)        => attackBonus += a;
        public void RemoveAttackBonus(int a)     => attackBonus -= a;
        
        public void AddDefenseBonus(int v)     => defenseBonus += v;
        public void RemoveDefenseBonus(int v)  => defenseBonus -= v;

        public void AddHealthBonus(int h)      => healthBonus += h;
        public void RemoveHealthBonus(int h)   => healthBonus -= h;
    }
}

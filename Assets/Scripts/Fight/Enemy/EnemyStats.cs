using UnityEngine;
using Buff;

namespace Enemy
{
    public class EnemyStats : CharacterStatsBase
    {
        [Header("额外：敌人特有字段，比如种类、AI 模式……")]
        public string enemyName;
        public int experienceOnDeath = 5;

        // 若有敌人专属逻辑可在这里写
    }
}

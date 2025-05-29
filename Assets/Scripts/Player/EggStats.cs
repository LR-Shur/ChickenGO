using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    // 鸡蛋的花纹类型枚举
    public enum EggPattern
    {
        
        LavaCrack,//熔岩裂纹
        // ………
    }


    public enum EggColor
    {
        None, Red, Yellow, Cyan 
    }

    /// <summary>
    /// 表示一个“蛋”的属性：大小、花纹、颜色
    /// </summary>
    [Serializable]
    public class EggStats
    {
        [Tooltip("蛋的大小，影响评分或体积")]
        public float size = 1f;
        

        [Tooltip("蛋的颜色")]
        public EggColor color = EggColor.None;
        public float eggColorScore = 0f;

        [System.NonSerialized]
        public Dictionary<EggPattern, float> patternModifiers 
            = new Dictionary<EggPattern, float>();

        /// <summary>
        /// 给某个花纹加成（可累加）
        /// </summary>
        public void AddPatternBonus(EggPattern pattern, float bonus)
        {
            if (patternModifiers.ContainsKey(pattern))
                patternModifiers[pattern] += bonus;
            else
                patternModifiers[pattern] = bonus;
        }

        /// <summary>
        /// 移除某个花纹加成
        /// </summary>
        public void RemovePatternBonus(EggPattern pattern, float bonus)
        {
            if (!patternModifiers.TryGetValue(pattern, out var cur)) 
                return;
            cur -= bonus;
            if (cur <= 0f)
                patternModifiers.Remove(pattern);
            else
                patternModifiers[pattern] = cur;
        }

        /// <summary>
        /// 读取当前某花纹的加成，没配置时返回 0
        /// </summary>
        public float GetPatternBonus(EggPattern pattern)
            => patternModifiers.TryGetValue(pattern, out var v) ? v : 0f;
        
    }

}

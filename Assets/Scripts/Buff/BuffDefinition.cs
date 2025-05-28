using System;
using System.Collections.Generic;
using UnityEngine;

namespace Buff
{
    public enum BuffStackType
    {
        ResetDuration, // 重置持续时长
        Accumulate     // 叠加层数，持续时长取最优
    }

    public enum BuffDecayType
    {
        DecayOneByOne, // 每次到时减少一层
        DecayAll       // 到时一次性移除所有层
    }
    
    // 2. BuffInfo：调用封装
    public struct BuffInfo
    {
        public string buffId;
        public int stacks;
        public BuffInfo(string id, int count = 1)
        {
            buffId = id;
            stacks = count;
        }
    }
    
    
    
    // 1. BuffDefinition：定义 Buff 数据及逻辑引用
    [CreateAssetMenu(menuName = "Buff/Definition", fileName = "NewBuffDefinition")]
    public class BuffDefinition : ScriptableObject
    {
        [Header("唯一 ID")]
        public int buffId;

        [Header("显示名称")]
        public string displayName;

        [Header("图标，用于 UI 显示")]
        public Sprite icon;

        [Header("持续时间（秒），无限期则勾选 isInfinite")]  
        public bool isInfinite = false;
        public float duration = 5f;
        public int maxStacks = 1;                 // 最大层数
        public BuffStackType stackType = BuffStackType.ResetDuration;
        public BuffDecayType decayType = BuffDecayType.DecayAll;
        

        [Header("行为逻辑引用（可为空）")]
        public BuffBehavior behavior;          // ScriptableObject，封装 OnApply/OnTick/OnExpire
    }
}

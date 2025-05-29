using UnityEngine;

namespace Buff
{
    // 1a. BuffBehavior：ScriptableObject 抽象类，定义具体 Buff 的行为逻辑
    public abstract class BuffBehavior : ScriptableObject
    {
        // Buff 新增
        public virtual void OnApply(GameObject target, int layer)
        {
            
        }
        
        // Buff 进行时，每帧调用（可选）
        public virtual void OnTick(GameObject target,int layer, float deltaTime){}
        
        // Buff 结束时调用
        public virtual void OnExpire(GameObject target){}
    }


}

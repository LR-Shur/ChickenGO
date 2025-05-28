using UnityEngine;

namespace Buff
{
    // 3. BuffInstance：运行时管理持续与层数
    public class BuffInstance
    {
        public BuffDefinition     Def { get; }
        public float               Remain { get; private set; }
        public int                 Stacks { get; private set; }
        public bool                Expired => Remain <= 0 && Stacks <= 0;

        public BuffInstance(BuffDefinition def, int initial)
        {
            Def    = def;
            Stacks = Mathf.Clamp(initial, 1, def.maxStacks);
            Remain = def.duration;
        }

        // 移除持续并按策略减少
        public void Tick(float dt)
        {
            Remain -= dt;
            if (Remain <= 0)
            {
                if (Def.decayType == BuffDecayType.DecayOneByOne && Stacks > 1)
                {
                    Stacks--;
                    Remain += Def.duration;
                }
                else
                {
                    Stacks = 0;
                    Remain = 0;
                }
            }
        }

        // 叠加层数或重置持续
        public void Add(int count)
        {
            if (Def.stackType == BuffStackType.ResetDuration)
            {
                Stacks = Mathf.Clamp(Def.maxStacks, Stacks, Def.maxStacks);
                Remain = Def.duration;
            }
            else // Accumulate
            {
                Stacks = Mathf.Clamp(Stacks + count, 1, Def.maxStacks);
                Remain = Mathf.Max(Remain, Def.duration);
            }
        }
    }

}

using UnityEngine;

namespace Inventory
{
    public class ItemInstance
    {
        public ItemDefinition Def { get; }
        public int count { get; private set; }

        public ItemInstance(ItemDefinition def, int initial)
        {
            Def = def;
            count = initial;
        }
        public void AddCount(int delta)
        {
            count = Mathf.Max(0, count + delta);
        }
    }

}

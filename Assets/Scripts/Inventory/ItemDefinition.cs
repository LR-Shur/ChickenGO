using Buff;
using UnityEngine;

public struct ItemInfo
{
    public int itemId;
    public int count;      // 多堆叠物品的数量
    public ItemInfo(int id, int cnt=1) { itemId = id; count = cnt; }
}

[CreateAssetMenu(menuName = "Item/Definition")]
public class ItemDefinition : ScriptableObject
{
    public int        itemId;           // 唯一整数ID
    public string     displayName;      
    public Sprite     icon;             
    public bool       isConsumable;     // 是否一次性使用
    public bool       hasPassive;       // 放入背包即生效
    public bool       hasActive;        // 手动使用时生效
    public BuffInfo[] passiveBuffs;     // 放入背包后自动添加的 BuffInfo 列表
    public BuffInfo[] activeBuffs;      // 使用时触发的 BuffInfo 列表
    
}

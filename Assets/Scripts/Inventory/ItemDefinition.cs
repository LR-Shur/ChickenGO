using System;
using Buff;
using UnityEngine;

public struct ItemInfo
{
    public int itemId;
    public int count;      // 多堆叠物品的数量
    public ItemInfo(int id, int cnt=1) { itemId = id; count = cnt; }
}

[Serializable]
public class BuffEntry
{
    [Tooltip("要应用的 BuffDefinition")]
    public BuffDefinition definition;
    
    [Tooltip("初始层数")]
    public int stacks = 1;
}

[CreateAssetMenu(menuName = "Item/Definition")]
public class ItemDefinition : ScriptableObject
{
    public int        itemId;           // 唯一整数ID
    public string     displayName;      
    public string     description;
    public Sprite     icon;        
    
    [Header("最大堆叠数量（>1 则可堆叠，否则每件单独占格）")]
    public int maxStack = 1;   
    
    [Header("是不是一次性的")]
    public bool       isConsumable = true;     // 是否一次性使用
    [Header("放到背包是不是有被动效果")]
    public bool       hasPassive=false;       // 放入背包即生效
    [Header("是否是主动道具")]
    public bool  canUse       = true;    // 是否能点击 Use
    [Header("被动 Buff（放包时生效）")]
    public BuffEntry[] passiveBuffs;

    [Header("主动 Buff（Use 时生效）")]
    public BuffEntry[] activeBuffs;
    
}

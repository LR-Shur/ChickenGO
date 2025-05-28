using Inventory;
using UnityEngine;
using UnityEngine.AddressableAssets;

[RequireComponent(typeof(Collider2D))]
public class ItemPickupTrigger : MonoBehaviour
{
    [Header("对应的道具 ID")]
    public int itemId;        // 对应 ItemDefinition.asset 中的 itemId
    [Header("拾取数量")]
    public int count = 1;

    private void Awake()
    {
        // 必须勾选 Is Trigger
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检测是否是玩家
        var player = other.GetComponent<PlayerController>();
        

        // 拾取：调用 InventoryHandle
        var inv = player.GetComponent<InventoryHandle>();
        if (inv != null)
        {
            inv.Pickup(new ItemInfo(itemId, count));
        }

        // 销毁道具物件
        Destroy(gameObject);
    }
}


using UnityEngine;
using Buff; // BuffEntry, BuffInfo

namespace Fight.Weapon
{
    public class Projectile : MonoBehaviour
    {
        [HideInInspector] public int damage;        // 由 WeaponSO 在 Instantiate 时赋值
        [HideInInspector] public bool givesBuff;    // 是否命中后要施加 Buff

        // 从 BowWeapon 赋过来：挂在箭矢上的 BuffEntry[]
        [HideInInspector] public BuffEntry[] buffEntries;

        private void OnTriggerEnter2D(Collider2D col)
        {
            // ◇ 如果碰到敌人
            if (col.CompareTag("Enemy"))
            {
                // 先拿到敌人的 Stats
                var enemyStats = col.GetComponent<CharacterStatsBase>();
                if (enemyStats != null)
                {
                    // 扣血
                    int finalDmg = Mathf.Max(damage - enemyStats.Defense, 0);
                    enemyStats.currentHealth -= finalDmg;

                    // 如果此箭设置了给 Buff，就遍历 buffEntries 构造 BuffInfo 添加
                    if (givesBuff && buffEntries != null && buffEntries.Length > 0)
                    {
                        var bh = col.GetComponent<BuffHandle>();
                        if (bh != null)
                        {
                            foreach (var entry in buffEntries)
                            {
                                if (entry.definition != null)
                                {
                                    var info = new BuffInfo(entry.definition.buffId, entry.stacks);
                                    bh.Add(info);
                                }
                            }
                        }
                    }
                }

                Destroy(gameObject);
                return;
            }

            // ◇ 如果碰到“墙体”图层
            if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Destroy(gameObject);
                return;
            }

            
        }

        private void Start()
        {
            // 自动销毁，防止箭飞出场景后一直存在
            Destroy(gameObject, 5f);
        }
    }
}

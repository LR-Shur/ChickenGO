namespace Fight.Weapon
{
  // DaggerWeapon.cs
using UnityEngine;
using Buff;        // BuffEntry, BuffInfo
using Player;      // CharacterStatsBase
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Weapon/DaggerWeapon", fileName = "DaggerWeapon_")]
public class DaggerWeapon : WeaponBase
{
    [Header("匕首专属配置")]
    [Tooltip("匕首特效预制体（可选，用于播放近战特效，比如刃光）")]
    public GameObject daggerPrefab;

    [Tooltip("匕首近战检测距离（单位：世界坐标）")]
    public float attackRange = 1.0f;

    public override WeaponType Type => WeaponType.Dagger;

    public override void Attack(Transform attacker, Vector2 targetWorldPos)
    {
        // 1. 播放匕首攻击动画（假设 Animator 有 "DaggerAttack"）
        Animator anim = attacker.GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("DaggerAttack");
        }

        // 2. 计算方向
        Vector2 myPos = attacker.position;
        Vector2 dir = (targetWorldPos - myPos).normalized;

        // 3. 近距离 Raycast2D 检测敌人
        RaycastHit2D hit = Physics2D.Raycast(myPos, dir, attackRange, LayerMask.GetMask("Enemy"));
        if (hit.collider != null)
        {
            // 3.1 拿到敌人的 Stats
            CharacterStatsBase enemyStats = hit.collider.GetComponent<CharacterStatsBase>();
            if (enemyStats != null)
            {
                // 3.2 先扣血（Damage 考虑防御）
                CharacterStatsBase attackerStats = attacker.GetComponent<CharacterStatsBase>();
                int baseAtk = (attackerStats != null) ? attackerStats.Attack : damage;
                int finalDamage = Mathf.Max(baseAtk - enemyStats.Defense, 0);
                enemyStats.currentHealth -= finalDamage;

                // 3.3 如果配了 buffEntries 或 givesBuff，就给目标添加 Buff
                if (givesBuff || (buffEntries != null && buffEntries.Length > 0))
                {
                    var bh = hit.collider.GetComponent<BuffHandle>();
                    if (bh != null && buffEntries != null)
                    {
                        foreach (var entry in buffEntries)
                        {
                            if (entry.definition != null)
                            {
                                // 构造 BuffInfo 并添加
                                var info = new BuffInfo(entry.definition.buffId, entry.stacks);
                                bh.Add(info);
                            }
                        }
                    }
                }
            }
        }

        // 4. 生成匕首攻击特效
        if (daggerPrefab != null)
        {
            GameObject fx = Instantiate(daggerPrefab, attacker.position, Quaternion.identity);
            fx.transform.up = dir;
            // 可以在特效脚本里设置销毁时间，或：
            Destroy(fx, 1f);
        }
    }
}


}

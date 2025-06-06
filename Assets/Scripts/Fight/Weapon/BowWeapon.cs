namespace Fight.Weapon
{
    // BowWeapon.cs
    using UnityEngine;
    using Player; // 拿到 CharacterStatsBase
    using UnityEngine.EventSystems;
    

    [CreateAssetMenu(menuName = "Weapon/BowWeapon", fileName = "BowWeapon_")]
    public class BowWeapon : WeaponBase
    {
        [Header("弓箭专属配置")]
        [Tooltip("箭矢预制体，需挂载 Rigidbody2D + Collider2D(isTrigger) + Projectile.cs")]
        public GameObject arrowPrefab;

        [Tooltip("箭矢飞行速度")]
        public float projectileSpeed = 8f;

        public override WeaponType Type => WeaponType.Bow;

        public override void Attack(Transform attacker, Vector2 targetWorldPos)
        {
            // 1. 播放弓箭拉弓动画（假设 Animator 有 "BowAttack"）
            Animator anim = attacker.GetComponent<Animator>();
            if (anim != null)
            {
                anim.Play("BowAttack");
            }

            // 2. 生成箭矢实例，并给它赋予速度 + 伤害 + BuffInfo 列表
            if (arrowPrefab != null)
            {
                GameObject arrowGO = Instantiate(arrowPrefab, attacker.position, Quaternion.identity);
                Vector2 dir = (targetWorldPos - (Vector2)attacker.position).normalized;
                arrowGO.transform.up = dir;

                // 设置 Rigidbody2D 速度
                Rigidbody2D rb2 = arrowGO.GetComponent<Rigidbody2D>();
                if (rb2 != null)
                {
                    rb2.velocity = dir * projectileSpeed;
                }

                // 让箭矢的 Projectile 脚本拿到本次伤害与要触发的 Buff
                var proj = arrowGO.GetComponent<Projectile>();
                if (proj != null)
                {
                    // 2.1 设置伤害值：优先用攻击者当前 stats.Attack，否则用 SO.damage
                    CharacterStatsBase attackerStats = attacker.GetComponent<CharacterStatsBase>();
                    proj.damage = (attackerStats != null) ? attackerStats.Attack : damage;

                    // 2.2 设置给箭矢的 BuffEntry 数组（可供 OnTriggerEnter2D 时使用）
                    proj.buffEntries = buffEntries; // 把 SO 中挂的 BuffEntry[] 直接传给箭矢
                    proj.givesBuff = givesBuff;     // 标记是否要加 Buff
                }
            }
        }
    }


}

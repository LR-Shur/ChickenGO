// WeaponBase.cs
using UnityEngine;

/// <summary>
/// 武器类型枚举（可自行扩展）
/// </summary>
public enum WeaponType
{
    Dagger,
    Bow,
    // 后续可加更多类型
}

namespace Fight.Weapon
{
    /// <summary>
    /// 武器基类：所有具体武器都继承自它，挂在 SO 资源上
    /// </summary>
    public abstract class WeaponBase : ScriptableObject
    {
        [Header("武器基础配置（必填）")]
        [Tooltip("对应 ItemDefinition 中的 itemId，用于查找")]
        public int itemId;

        [Tooltip("武器显示名称（仅作编辑时参考）")]
        public string displayName;

        [Tooltip("武器造成的基础伤害值")]
        public int damage = 1;

        [Tooltip("武器攻击速度（每秒可攻击次数），可根据需要自行解读")]
        public float attackSpeed = 1f;

        [Header("攻击命中时要触发的 Buff 列表（可选）")]
        [Tooltip("在 Inspector 里挂入 BuffEntry，用于命中后自动给目标添加 BuffInfo")]
        public BuffEntry[] buffEntries;

        [Tooltip("此武器是否击中时会给目标施加 Buff（也可单独通过 buffEntries 配置更细）")]
        public bool givesBuff = false;

        /// <summary>
        /// 子类必须实现，返回本武器的类型
        /// </summary>
        public abstract WeaponType Type { get; }

        /// <summary>
        /// 所有武器必须重写此攻击方法：输入“攻击者 Transform”与“目标世界坐标”，
        /// 在此内部完成播放动画、生成特效、判定命中、扣血、施加 Buff 等一切逻辑
        /// </summary>
        /// <param name="attacker">发起攻击的 Transform（一般是玩家或敌人的 transform）</param>
        /// <param name="targetWorldPos">鼠标点击或目标位置的世界坐标</param>
        public abstract void Attack(Transform attacker, Vector2 targetWorldPos);
    }
}

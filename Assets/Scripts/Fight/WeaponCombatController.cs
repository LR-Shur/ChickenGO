// WeaponCombatController.cs

using Fight.Weapon;
using UnityEngine;
using Inventory;                // 拿到 ItemInstance
using UnityEngine.EventSystems;

namespace Fight
{
    [RequireComponent(typeof(Animator))]
    public class WeaponCombatController : MonoBehaviour
    {
        [Header("当前已经装备的武器 SO（实时从 WeaponManager 拉取）")]
        private WeaponBase currentWeaponSO;

        [Header("调试：当前武器类型，可从 currentWeaponSO.Type 获得")]
        [SerializeField] private WeaponType currentWeaponType = WeaponType.Dagger;

        private Camera mainCamera;
        private CharacterStatsBase stats;
        private Animator anim;
        public bool isEquipWeapon;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            mainCamera = Camera.main;
            stats = GetComponent<CharacterStatsBase>();
            isEquipWeapon=false;
        }

        /// <summary>
        /// 由外部（例如 InventoryUI）调用：
        /// 传入要装备的 ItemInstance，如果为 null 或非武器，则卸下当前武器。
        /// </summary>
        public void EquipWeapon(ItemInstance weaponItem)
        {
            if (weaponItem == null && weaponItem.Def.isWeapon)
            {
                // 卸下
                currentWeaponSO = null;
                currentWeaponType = WeaponType.Dagger; // 可随意赋，但不再攻击
                anim.Play("Idle");
            }
            else
            {
                // 1. 先从 Manager 里根据 itemId 拿到对应的武器 SO
                WeaponBase so = WeaponManager.Instance.GetWeaponByItemId(weaponItem.Def.itemId);
                if (so == null)
                {
                    Debug.LogWarning($"WeaponCombatController: 找不到 itemId={weaponItem.Def.itemId} 对应的 WeaponBase SO");
                    return;
                }

                // 2. 装备
                currentWeaponSO = so;
                currentWeaponType = so.Type;
                anim.Play("Idle"); // 切到持武器待机，具体动画由 Animator Controller 决定
                isEquipWeapon=true;
            }
        }

        /// <summary>
        /// 由 PlayerController 在 Update 检测到 “点击非 UI 区域且当前有武器时” 调用，
        /// 传入鼠标点击的世界坐标，由当前武器 SO 去执行 Attack(…)
        /// </summary>
        public void HandleAttack(Vector2 targetWorldPos)
        {
            if (currentWeaponSO == null) return;

            currentWeaponSO.Attack(transform, targetWorldPos);
        }

        /// <summary>
        /// 辅助：在 PlayerController 中检查鼠标是否在 UI 上
        /// </summary>
        public bool IsPointerOverUI()
        {
            return EventSystem.current != null 
                   && EventSystem.current.IsPointerOverGameObject();
        }
    }
}

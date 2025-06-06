// PlayerController.cs
using UnityEngine;
using Fight;        // 引用 WeaponCombatController 所在命名空间

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PlayerStats))]
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D rb;
        private Animator anim;
        public PlayerStats stats;
        private Vector2 input;
        private bool isWalking;

        private Camera mainCamera;
        private WeaponCombatController combatCtrl; // 引用战斗控制器

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            stats = GetComponent<PlayerStats>();
            rb.gravityScale = 0f;

            mainCamera = Camera.main;
            combatCtrl = GetComponent<WeaponCombatController>();
        }

        private void Update()
        {
            // 1. 移动逻辑
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            input.Normalize();

            bool shouldWalk = input.sqrMagnitude > 0.01f;
            if (shouldWalk && !isWalking)
            {
                anim.Play("Walk");
                isWalking = true;
            }
            else if (!shouldWalk && isWalking)
            {
                anim.Play("Idle");
                isWalking = false;
            }
            //转向

            if (input.x > 0.01f) transform.localScale = Vector3.one * 8.5f;
            else if (input.x < -0.01f) transform.localScale = new Vector3(-8.5f, 8.5f, 8.5f);

            // 2. 攻击逻辑托管给 combatCtrl：当手上有武器时，鼠标点击交给它去决策
            //    需要先判断 combatCtrl.currentWeaponType != None，并且不在 UI 上点击
            if (combatCtrl.isEquipWeapon )
            {
                if (Input.GetMouseButtonDown(0) && !combatCtrl.IsPointerOverUI())
                {
                    // 把鼠标位置转成世界坐标，再交给武器战斗控制器
                    Vector2 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    combatCtrl.HandleAttack(worldPos);
                }
            }
        }

        private void FixedUpdate()
        {
            // 用 stats.Speed 来移动
            rb.velocity = input * stats.Speed;
        }
    }
}

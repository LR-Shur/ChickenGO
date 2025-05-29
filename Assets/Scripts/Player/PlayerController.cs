
namespace Player
{


    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PlayerStats))]
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D rb;
        private Animator anim;
        public PlayerStats stats;
        private Vector2 input;
        private bool isWalking;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            stats = GetComponent<PlayerStats>();
            rb.gravityScale = 0f;
        }

        private void Update()
        {
            // 读取输入并归一化
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            input.Normalize();

            bool shouldWalk = input.sqrMagnitude > 0.01f;
            if (shouldWalk && !isWalking) {
                anim.Play("Walk");
                isWalking = true;
            }
            else if (!shouldWalk && isWalking) {
                anim.Play("Idle");
                isWalking = false;
            }

            // 翻转朝向
            if (input.x > 0.01f) transform.localScale = Vector3.one * 8.5f;
            else if (input.x < -0.01f) transform.localScale = new Vector3(-8.5f, 8.5f, 8.5f);
        }

        private void FixedUpdate()
        {
            // 用 stats.Speed 而非 moveSpeed
            rb.velocity = input * stats.Speed;
        }
    }
}
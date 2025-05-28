using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 input;
    private bool isWalking;

    private void Awake()
    {
        rb   = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.gravityScale = 0f;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnRoundStart += EnableControl;
        GameManager.Instance.OnRoundEnd   += DisableControl;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnRoundStart -= EnableControl;
        GameManager.Instance.OnRoundEnd   -= DisableControl;
    }

    private void EnableControl()  { enabled = true;  }
    private void DisableControl() { enabled = false; }

    private void Update()
    {
        // 读取输入并归一化
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();

        // 根据 input 决定状态
        bool shouldWalk = input.sqrMagnitude > 0.01f;
        if (shouldWalk && !isWalking)
        {
            anim.Play("Walk");    // 切到 Walk 动画
            isWalking = true;
        }
        else if (!shouldWalk && isWalking)
        {
            anim.Play("Idle");    // 切回 Idle
            isWalking = false;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = input * moveSpeed;
    }
}

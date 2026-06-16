using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;

    private void Awake()
    {
        // 컴포넌트 캐싱 (성능 최적화)
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 1. 유저 입력 받기 (매 프레임 체크하는 Update가 적합)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. 점프 입력 처리
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        // 3. 물리적인 이동 처리 (물리 연산은 FixedUpdate에서 수행)
        Move();
    }

    private void Move()
    {
        // Unity 6 권장: velocity 대신 linearVelocity 사용
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false; // 점프하는 순간 공중에 뜸
    }

    // 바닥에 착지했는지 확인하는 충돌 콜백
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
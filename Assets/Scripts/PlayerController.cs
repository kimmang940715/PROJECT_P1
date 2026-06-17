using UnityEngine;

// 이 스크립트를 오브젝트에 붙이면 Rigidbody2D와 BoxCollider2D가 없을 때 자동으로 생성됩니다.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
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
        // RequireComponent 덕분에 이제 무조건 존재함이 보장되므로 안심하고 캐싱할 수 있습니다.
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 1. 유저 입력 받기
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. 점프 입력 처리
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        // 3. 물리적인 이동 처리
        Move();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
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

    [Header("Player Stats")]
    public int maxHp = 3;
    private int currentHp;

    private void Awake()
    {
        // RequireComponent 덕분에 이제 무조건 존재함이 보장되므로 안심하고 캐싱할 수 있습니다.
        rb = GetComponent<Rigidbody2D>();

        // [추가된 코드] 플레이어가 빠른 속도로 떨어질 때 바닥을 뚫는 현상(터널링)을 방지합니다.
        // 물리 엔진이 프레임 사이의 이동 경로까지 연속(Continuous)적으로 추적하여 충돌을 감지하게 합니다.
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    private void Start() // Awake 말고 Start에 작성
    {
        currentHp = maxHp; // 시작할 때 체력 꽉 채우기
    }

    // 유성이 호출할 데미지 처리 함수
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"으악! 유성에 맞았다! 남은 HP: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("💀 플레이어 사망... 게임 오버!");

        // 싱글톤 호출 전 안전장치 추가
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isGameOver = true;
        }

        // 플레이어 조작 불가능하게 비활성화 하거나 파괴
        gameObject.SetActive(false);
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
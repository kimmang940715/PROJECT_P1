using UnityEngine;

public class Meteor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 유성이 플레이어와 부딪혔을 때
        if (other.CompareTag("Player"))
        {
            // 플레이어의 PlayerController 컴포넌트를 가져와서 데미지 함수 실행
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1); // 1의 데미지를 줌
            }

            // 부딪힌 유성은 파괴됨
            Destroy(gameObject);
        }
        // 2. 유성이 땅에 떨어졌을 때 (계속 떨어지면 메모리 낭비이므로 파괴)
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
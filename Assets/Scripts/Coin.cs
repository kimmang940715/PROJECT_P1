using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 부딪힌 대상이 'Player'인지 확인
        if (other.CompareTag("Player"))
        {
            // GameManager.Instance를 부르는 순간, 씬에 없으면 '자동으로 생성 및 할당' 됩니다!
            if (GameManager.Instance != null)
            {
                // 이전의 AddScore 대신 새로 만든 AddPart(부품 추가)를 호출합니다.
                GameManager.Instance.AddPart();
            }

            // 부품을 먹었으니 화면에서 파괴(삭제)
            Destroy(gameObject);
        }
    }
}
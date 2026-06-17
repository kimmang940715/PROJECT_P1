using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 전역에서 GameManager.Instance로 바로 접근 가능
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public int score = 0;
    public bool isGameOver = false;

    private void Awake()
    {
        // 싱글톤 인스턴스 세팅
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        
        score += amount;
        Debug.Log($"현재 점수: {score}");
        // TODO: UI 매니저에게 점수 표시 갱신 요청
    }
}
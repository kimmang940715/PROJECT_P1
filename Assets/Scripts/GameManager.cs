using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    // 프로퍼티를 통해 외부에서 접근할 때 체크 및 자동 생성 수행
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 1. 혹시 씬에 이미 GameManager가 배치되어 있는지 먼저 확인
                _instance = FindFirstObjectByType<GameManager>();

                // 2. 없다면 새로 생성
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager_AutoCreated");
                    _instance = go.AddComponent<GameManager>();

                    // 씬이 바뀌어도 파괴되지 않도록 설정
                    DontDestroyOnLoad(go);
                    Debug.Log("씬에 GameManager가 없어 코드가 자동으로 생성했습니다.");
                }
            }
            return _instance;
        }
    }

    [Header("Game State")]
    public int score = 0;
    public bool isGameOver = false;

    private void Awake()
    {
        // 누군가 에디터에서 직접 GameManager를 배치했을 경우를 대비한 중복 방지 로직
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        Debug.Log($"현재 점수: {score}점");
    }
}
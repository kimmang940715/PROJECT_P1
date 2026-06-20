using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager_AutoCreated");
                    _instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    [Header("Game State")]
    public int currentParts = 0;          // 현재 모은 부품 수
    public int totalPartsNeeded = 5;      // 우주선 완성을 위해 필요한 총 부품 수
    public bool isGameOver = false;

    private void Awake()
    {
        if (_instance == null) { _instance = this; DontDestroyOnLoad(gameObject); }
        else if (_instance != this) { Destroy(gameObject); }
    }

    // 기존 AddScore 대신 부품 획득 메서드로 변경
    public void AddPart()
    {
        if (isGameOver) return;

        currentParts++;
        Debug.Log($"부품 획득! ({currentParts}/{totalPartsNeeded})");

        // 목표치를 다 채웠다면?
        if (currentParts >= totalPartsNeeded)
        {
            GameClear();
        }
    }

    private void GameClear()
    {
        isGameOver = true;
        Debug.Log("🎉 모든 부품을 모았습니다! 우주선 조립 완료 및 탈출 성공!");
        // TODO: 클리어 UI 띄우기, 우주선이 날아가는 연출 등
    }
}
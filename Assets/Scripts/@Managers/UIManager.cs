using UnityEngine;
using TMPro; // TextMeshPro 텍스트 UI를 다루기 위해 필수입니다.

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    // 싱글톤 패턴 (씬에 없으면 자동 생성)
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<UIManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("UIManager_AutoCreated");
                    _instance = go.AddComponent<UIManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    [Header("인게임 텍스트 UI")]
    public TextMeshProUGUI partsText; // "부품: 0 / 5" 표시
    public TextMeshProUGUI hpText;    // "HP: 3" 표시

    [Header("결과 화면 패널")]
    public GameObject gameOverPanel;  // 유성에 맞아 죽었을 때 띄울 창
    public GameObject gameClearPanel; // 부품을 다 모았을 때 띄울 창

    private void Awake()
    {
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

    private void Start()
    {
        // 게임 시작 시 결과 화면(패널)들은 보이지 않게 꺼둡니다.
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameClearPanel != null) gameClearPanel.SetActive(false);
    }

    // [GameManager에서 호출] 부품을 먹었을 때 텍스트 갱신
    public void UpdatePartsUI(int currentParts, int totalParts)
    {
        if (partsText != null)
        {
            partsText.text = $"부품: {currentParts} / {totalParts}";
        }
    }

    // [PlayerController에서 호출] 맞았을 때 체력 텍스트 갱신
    public void UpdateHpUI(int currentHp)
    {
        if (hpText != null)
        {
            hpText.text = $"HP: {currentHp}";
        }
    }

    // [GameManager 또는 PlayerController에서 호출] 사망 시
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // [GameManager에서 호출] 부품을 모두 모았을 때
    public void ShowGameClear()
    {
        if (gameClearPanel != null)
        {
            gameClearPanel.SetActive(true);
        }
    }
}
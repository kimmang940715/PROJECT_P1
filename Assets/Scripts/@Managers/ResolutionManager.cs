using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    private static ResolutionManager _instance;

    public static ResolutionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<ResolutionManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("ResolutionManager_AutoCreated");
                    _instance = go.AddComponent<ResolutionManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    [Header("목표 화면 비율 (가로 / 세로)")]
    [Tooltip("기본값은 16:9 (16 / 9 = 1.777...) 입니다.")]
    public float targetWidth = 16f;
    public float targetHeight = 9f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // 게임 시작 시 즉시 해상도 비율 맞추기 실행
            SetResolution();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetResolution()
    {
        // 1. 목표 비율과 현재 기기 화면의 비율 계산
        float targetRatio = targetWidth / targetHeight;
        float currentRatio = (float)Screen.width / (float)Screen.height;

        // 2. 메인 카메라 찾기
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("메인 카메라를 찾을 수 없어 해상도 조절을 건너뜁니다.");
            return;
        }

        // 3. 비율에 맞춰 카메라의 Viewport Rect(그려지는 영역) 조절
        // 기기의 가로 비율이 목표치보다 더 길 때 (예: 최신 스마트폰) -> 좌우에 검은 띠(Pillarbox)
        if (currentRatio > targetRatio)
        {
            float widthRatio = targetRatio / currentRatio;
            mainCamera.rect = new Rect((1f - widthRatio) / 2f, 0f, widthRatio, 1f);
        }
        // 기기의 세로 비율이 목표치보다 더 길 때 (예: 아이패드) -> 상하에 검은 띠(Letterbox)
        else if (currentRatio < targetRatio)
        {
            float heightRatio = currentRatio / targetRatio;
            mainCamera.rect = new Rect(0f, (1f - heightRatio) / 2f, 1f, heightRatio);
        }
        // 비율이 정확히 일치할 때
        else
        {
            mainCamera.rect = new Rect(0f, 0f, 1f, 1f);
        }

        Debug.Log($"[ResolutionManager] 해상도 비율 조정 완료. (목표: {targetWidth}:{targetHeight}, 기기: {Screen.width}x{Screen.height})");
    }

    // PC 버전용: 특정 창 크기로 강제 고정하고 싶을 때 사용할 수 있는 함수
    public void SetPCResolution(int width, int height, bool isFullScreen)
    {
#if UNITY_STANDALONE // PC 빌드 환경에서만 실행됨
        Screen.SetResolution(width, height, isFullScreen);
        SetResolution(); // 창 크기를 바꾼 후 카메라 비율 다시 계산
#endif
    }
}
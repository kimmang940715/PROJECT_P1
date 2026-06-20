using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    // 싱글톤 패턴 (자동 생성 로직 포함)
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<AudioManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("AudioManager_AutoCreated");
                    _instance = go.AddComponent<AudioManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    [Header("오디오 소스 (자동 생성됨)")]
    private AudioSource bgmSource; // 배경음악 재생기
    private AudioSource sfxSource; // 효과음 재생기

    [Header("배경음악 클립")]
    public AudioClip bgmClip;

    [Header("효과음 클립 모음")]
    public AudioClip getPartClip;    // 부품 획득 시
    public AudioClip hitMeteorClip;  // 유성에 맞았을 때
    public AudioClip gameClearClip;  // 우주선 완성 시
    public AudioClip gameOverClip;   // 사망 시

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources(); // 오디오 소스 세팅
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 게임이 시작되면 BGM을 자동으로 틉니다.
        PlayBGM();
    }

    // 오디오 컴포넌트가 없으면 코드가 알아서 2개를 추가해 줍니다.
    private void InitializeAudioSources()
    {
        // 1. BGM 전용 소스 세팅
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true; // 배경음악은 무한 반복!
        bgmSource.playOnAwake = false;
        bgmSource.volume = 0.5f; // BGM 볼륨 (0.0 ~ 1.0)

        // 2. 효과음 전용 소스 세팅
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = 1.0f; // 효과음은 명확하게 크게
    }

    // BGM 재생 함수
    public void PlayBGM()
    {
        if (bgmClip != null && bgmSource != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }
    }

    // BGM 정지 함수 (게임 오버 등에서 사용)
    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }

    // 효과음 재생 함수
    // 소리가 겹쳐서 날 수 있도록 PlayOneShot을 사용합니다.
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
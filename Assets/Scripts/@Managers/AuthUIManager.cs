using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위한 네임스페이스

public class AuthUIManager : MonoBehaviour
{
    [Header("UI 입력 필드")]
    public TMP_InputField idInputField;       // 아이디 입력창
    public TMP_InputField passwordInputField; // 비밀번호 입력창

    [Header("피드백 텍스트")]
    public TextMeshProUGUI feedbackText;      // 성공/실패 메시지를 띄워줄 텍스트

    [Header("화면 전환 (선택 사항)")]
    public GameObject loginPanel; // 로그인 창 UI 그룹
    public GameObject gamePanel;  // 로그인 성공 시 보여줄 게임/로비 화면 그룹

    private void Start()
    {
        // 시작할 때 피드백 텍스트 초기화
        if (feedbackText != null)
        {
            feedbackText.text = "아이디와 비밀번호를 입력하세요.";
            feedbackText.color = Color.white;
        }
    }

    // [로그인 버튼]을 누르면 실행될 함수
    public void OnLoginButtonClicked()
    {
        string id = idInputField.text;
        string pw = passwordInputField.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            ShowFeedback("아이디와 비밀번호를 모두 입력해주세요.", Color.red);
            return;
        }

        /* * TODO: NetworkManager를 통해 실제 DB에 로그인 요청을 보냅니다.
         * 예: bool isSuccess = NetworkManager.Instance.LoginUser(id, pw); 
         * (아래는 임시로 성공했다고 가정하는 로직입니다)
         */
        bool isSuccess = true;

        if (isSuccess)
        {
            ShowFeedback($"환영합니다, {id}님! 게임을 시작합니다.", Color.green);

            // 로그인 창을 끄고, 게임 화면을 켭니다.
            if (loginPanel != null) loginPanel.SetActive(false);
            if (gamePanel != null) gamePanel.SetActive(true);
        }
        else
        {
            ShowFeedback("로그인 실패: 아이디나 비밀번호를 확인하세요.", Color.red);
        }
    }

    // [회원가입 버튼]을 누르면 실행될 함수
    public void OnSignupButtonClicked()
    {
        string id = idInputField.text;
        string pw = passwordInputField.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            ShowFeedback("생성할 아이디와 비밀번호를 입력해주세요.", Color.red);
            return;
        }

        /* * TODO: NetworkManager를 통해 실제 DB에 회원가입 요청을 보냅니다.
         * 예: bool isSuccess = NetworkManager.Instance.RegisterUser(id, pw); 
         * (아래는 임시로 성공했다고 가정하는 로직입니다)
         */
        bool isSuccess = true;

        if (isSuccess)
        {
            ShowFeedback("회원가입 완료! 이제 로그인해주세요.", Color.cyan);
            // 가입 후 비밀번호 창을 비워주는 디테일
            passwordInputField.text = "";
        }
        else
        {
            ShowFeedback("회원가입 실패: 이미 존재하는 아이디입니다.", Color.red);
        }
    }

    // 알림 메시지를 화면에 띄우는 편의용 함수
    private void ShowFeedback(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
        }
        Debug.Log($"[AuthUI] {message}");
    }
}
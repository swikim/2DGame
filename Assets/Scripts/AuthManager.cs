using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{

    [SerializeField]
    TMP_InputField emailField;
    public string Email;
    [SerializeField]
    TMP_InputField passwordField;

    [SerializeField]
    private GameObject FailedSignIn;
    [SerializeField]
    private GameObject FailedLogin;

    Firebase.Auth.FirebaseAuth auth;

    void Awake(){
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }
    public void SignIn()
{
    Email = emailField.text;
    auth.CreateUserWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWith(task =>
    {
        if (task.IsCanceled)
        {
            Debug.LogError("회원가입 취소");
            ShowFailedSigninPanel();
            return;
        }

        if (task.IsFaulted)
        {
            Debug.LogError("회원가입 실패");
            ShowFailedSigninPanel();
            return;
        }

        if (task.IsCompletedSuccessfully)
        {
            var authResult = task.Result;
            Firebase.Auth.FirebaseUser newUser = authResult.User;
            Debug.Log("회원가입 성공: " + newUser.Email);
        }
    });
}

    public void Login()
{
    if (string.IsNullOrEmpty(emailField.text) || string.IsNullOrEmpty(passwordField.text))
    {
        ShowFailedLoginPanel();
        Debug.LogError("이메일 또는 비밀번호가 비어 있습니다.");
        //return;
    }

    StartCoroutine(LoginCoroutine());
}

private IEnumerator LoginCoroutine()
{
    var loginTask = auth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text);

    // Firebase의 Task가 완료될 때까지 대기
    yield return new WaitUntil(() => loginTask.IsCompleted);

    if (loginTask.IsFaulted)
    {
        bool isBadFormatError = false;
        foreach (var e in loginTask.Exception.Flatten().InnerExceptions)
        {
            Firebase.FirebaseException firebaseEx = e as Firebase.FirebaseException;
            if (firebaseEx != null)
            {
                Debug.LogError("Error Code: " + firebaseEx.ErrorCode);
                Debug.LogError("Error Message: " + e.Message);

                if (firebaseEx.ErrorCode == (int)Firebase.Auth.AuthError.InvalidEmail)
                {
                    isBadFormatError = true;
                }
            }
        }
        if (isBadFormatError)
        {
            Debug.LogError("올바른 이메일 형식으로 다시 입력해 주세요.");
            emailField.text = ""; // 이메일 필드 초기화
        }
        ResetInputFields();
        ShowFailedLoginPanel();
        Debug.LogError("로그인 실패");
    }
    else if (loginTask.IsCanceled)
    {
        ResetInputFields();
        ShowFailedLoginPanel();
        Debug.LogError("로그인 취소됨");
    }
    else
    {
        Debug.Log(emailField.text + " 로 로그인 하셨습니다.");
        SceneManager.LoadScene("SampleScene");
    }
}
    private void ResetInputFields()
    {
        emailField.text = "";
        passwordField.text = "";
    }
    public void ShowFailedLoginPanel(){
        FailedLogin.SetActive(true);
    }
    public void ShowFailedSigninPanel(){
        FailedSignIn.SetActive(true);
    }

    public void HideShowFailedLoginPanel(){
        FailedLogin.SetActive(false);
    }
    public void HideFailedSigninPanel(){
        FailedSignIn.SetActive(false);
    }
    public void QuitGame()
    {
        // 에디터 환경에서는 플레이 모드 중지
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // 빌드된 게임에서는 애플리케이션 종료
            Application.Quit();
        #endif
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{

    [SerializeField]
    TMP_InputField emailField;
    [SerializeField]
    TMP_InputField passwordField;

    Firebase.Auth.FirebaseAuth auth;

    void Awake(){
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }
    public void SignIn(){
        auth.CreateUserWithEmailAndPasswordAsync(emailField.text,passwordField.text).ContinueWith(task=>{
            if(task.IsCanceled){
                Debug.LogError("회원가입 취소");
                return;
            }
            if(task.IsFaulted){
                Debug.LogError("회원가입 실패");
                return;
            }
            var authResult = task.Result; // AuthResult 객체
            Firebase.Auth.FirebaseUser newUser = authResult.User;
        });
    }
    public void Login()
{
    if (string.IsNullOrEmpty(emailField.text) || string.IsNullOrEmpty(passwordField.text))
    {
        Debug.LogError("이메일 또는 비밀번호가 비어 있습니다.");
        return;
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
        foreach (var e in loginTask.Exception.Flatten().InnerExceptions)
        {
            Firebase.FirebaseException firebaseEx = e as Firebase.FirebaseException;
            if (firebaseEx != null)
            {
                Debug.LogError("Error Code: " + firebaseEx.ErrorCode);
                Debug.LogError("Error Message: " + e.Message);
            }
        }
        Debug.LogError("로그인 실패");
    }
    else if (loginTask.IsCanceled)
    {
        Debug.LogError("로그인 취소됨");
    }
    else
    {
        Debug.Log(emailField.text + " 로 로그인 하셨습니다.");
        
        // 메인 스레드에서 씬 전환
        SceneManager.LoadScene("SampleScene");
    }
}
}

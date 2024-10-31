using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public Text TalkText;
    string talkData;
    public bool isAction = false;
    public GameObject TalkPannel;
    public GameObject scanObject;
    public ObjectData objectData;
    public TalkManager talkManager;

    public DialogueSystem dialogueSystem;
    
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject gameClearPanel;
    [SerializeField]
    private GameObject notEnoughGoldPanel;

    private AuthManager authManager;
    
    public int talkIndex = 0;
    public int gold = 0;
    private string userEmail;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    
    }
    // private void Start(){
    //     authManager = FindAnyObjectByType<AuthManager>();
    //     if(authManager != null){
    //         userEmail = authManager.Email;
    //         Debug.Log("현재 로그인된 사용자 이메일: " + userEmail);
    //     }else{
    //         Debug.LogError("AuthManager를 찾을 수 없습니다.");
    //     }
    // }
    private void Start(){
        LoadGoldData();
    }
    public void SetGameOver()
    {
        Invoke("ShowGameOverPanel",1f);
    }
    public void SetClear(){
        Invoke("ShowClearPanel",1f);
    }
    public void ShowClearPanel(){
        gameClearPanel = GameObject.Find("Gaceclear");
        gameClearPanel.SetActive(true);
    }
    public void ShowGameOverPanel(){
        gameOverPanel = GameObject.Find("GameOverPanel");
        
        gameOverPanel.SetActive(true);
    }
   public void PlayAgain(){
        SceneManager.LoadScene("SampleScene");
        Debug.Log("onclicked");
    }
    public void ShowNotEough(){
        notEnoughGoldPanel.SetActive(true);
    }

    public void Action(GameObject scanobj){
        scanObject = scanobj;
        ObjectData objectData = scanobj.GetComponent<ObjectData>();
        talk(objectData.id,objectData.isnpc);

        TalkPannel.SetActive(isAction);

    }
    void talk(int id, bool isnpc){
        talkData = talkManager.GetTalk(id,talkIndex);
        if(talkData == null){
            isAction = false;
            talkIndex = 0;
            return;
        }
        if(isnpc){
            TalkText.text = talkData;
        }else{     
            TalkText.text = talkData;
        }
        isAction = true;
        talkIndex++;
    }
    public void IncreaseGold(){
        gold ++;
        UpdataGoldInDatabase();
        UpdateGoldUI();
    }
    private void UpdataGoldInDatabase(){
        if(FirebaseManager.Instance.databaseReference !=null){
            FirebaseManager.Instance.databaseReference.Child("test").Child("gold").SetValueAsync(gold);
            Debug.Log("Succese Connected to DB");
        }
    }
    public void UpdateGoldUI(){
        Debug.Log("현재 골드: " + gold);
    }

    public IEnumerator LoadGoldDatabase(){
        var task = FirebaseManager.Instance.databaseReference.Child("test").GetValueAsync();
        if(task == null){
            Debug.Log("task is null");
        }
        yield return new WaitUntil(()=>task.IsCompleted);
        if(task.IsCanceled){
            Debug.Log("로드 취소");
        }else if(task.IsFaulted){
            Debug.Log("로드 실패");
        }else{
            var dataSnapshot = task.Result;
            string dataString = "";
            foreach(var data in dataSnapshot.Children){
                dataString += data.Key + " " + data.Value +"\n";
            }
            Debug.Log(dataString);
        }
    }
    public async Task LoadGoldData(){
        while (FirebaseManager.Instance.databaseReference == null)
            {
                await Task.Yield();
            }
        if (FirebaseManager.Instance == null || FirebaseManager.Instance.databaseReference == null)
{
            Debug.LogError("FirebaseManager 또는 databaseReference가 초기화되지 않았습니다.");
            return;
}
        await FirebaseManager.Instance.databaseReference.Child("test").Child("gold").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    gold = int.Parse(snapshot.Value.ToString());
                    Debug.Log("Gold 값 가져오기 성공: " + gold);
                }
                else
                {
                    Debug.Log("Gold 값이 존재하지 않습니다.");
                }
            }
            else
            {
                Debug.LogError("데이터베이스에서 값을 가져오는 데 실패했습니다: " + task.Exception);
            }
        });
    }

    public static implicit operator GameManager(TestDDOB v)
    {
        throw new NotImplementedException();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Unity.VisualScripting;
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
    public Button gameOverButton;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    
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
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            gameOverPanel = canvas.transform.Find("GameOverPanel")?.gameObject;
            gameClearPanel = canvas.transform.Find("GameClear")?.gameObject;
            if(gameOverButton ==null){
                gameOverButton = gameOverPanel.transform.Find("GameOverButton").GetComponent<Button>();
                if(gameOverButton != null){
                gameOverButton.onClick.AddListener(PlayAgain);
                }
            }
            
        }

        if (gameOverPanel != null&&gameClearPanel!=null)
        {
            gameOverPanel.SetActive(false);
            gameClearPanel.SetActive(false);
            Debug.Log("GameOverPanel을 찾았습니다.");
        }
        else
        {
            Debug.LogWarning("GameOverPanel을 찾을 수 없습니다.");
        }
        Debug.Log("새로운 씬이 로드되었습니다: " + scene.name);
    }
    public void SetGameOver()
    {
        Invoke("ShowGameOverPanel",1f);
    }
    public void SetClear(){
        Invoke("ShowClearPanel",1f);
    }
    public void ShowClearPanel(){
        
        gameClearPanel.SetActive(true);
    }
    public void ShowGameOverPanel(){        
        gameOverPanel.SetActive(true);
    }
   public void PlayAgain(){
        SceneManager.LoadScene("SampleScene");
    }
    public void ShowNotEough(){
        notEnoughGoldPanel.SetActive(true);
    }
    public void CloseNotEnough(){
        notEnoughGoldPanel.SetActive(false);
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

    public static implicit operator GameManager(TestDDOB v)
    {
        throw new NotImplementedException();
    }
}

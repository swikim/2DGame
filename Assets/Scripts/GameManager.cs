using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    
    public int talkIndex = 0;

    void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject gameClearPanel;

    void Awake(){
        if(Instance == null){
            Instance = this;
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

}

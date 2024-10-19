using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    [SerializeField]
    private GameObject gameOverPanel;

    void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }

    public void SetGameOver()
    {
        Invoke("ShowGameOverPanel",1f);
    }
    public void ShowGameOverPanel(){
        gameOverPanel.SetActive(true);
    }
   public void PlayAgain(){
        SceneManager.LoadScene("SampleScene");
    }

}

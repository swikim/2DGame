using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestDDOB : MonoBehaviour
{
    private static GameManager instance;

    private void Start(){

    }
    private void Awake(){
        GameObject[] objs = GameObject.FindGameObjectsWithTag("NPC");

        if (objs.Length > 1) // 이미 GameManager가 있는 경우
        {
            Destroy(this.gameObject); // 새로운 오브젝트는 삭제
        }
        else // 첫 씬에서 생성된 경우
        {
            DontDestroyOnLoad(this.gameObject); // 첫 씬의 GameManager만 유지
        }
    }
    
}
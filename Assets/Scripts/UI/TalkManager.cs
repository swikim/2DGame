using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    public GameManager gameManager;
    private int gold;
    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    // Update is called once per frame
    void GenerateData()
    {
        talkData.Add(1000,new string[]{"안녕","골드 10개 모아야해!"});
        //talkData.Add(100,new string[]{"평범한 문이다."});
        talkData.Add(100,new string[]{"골드가 아직 모잘라."});
    }
    string GetGoldText(){
        int gold = GameManager.Instance.gold;
        return $"골드가 아직 {gold}개 있어";
    }
    public string GetTalk(int id,int talkIndex){
        if(talkIndex ==talkData[id].Length){
            return null;
        } 
        if (id == 100 && talkIndex == 0){
            talkData[id][talkIndex] = GetGoldText();
        }

        return talkData[id][talkIndex];
    }
    
}

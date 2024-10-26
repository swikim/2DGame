using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    // Update is called once per frame
    void GenerateData()
    {
        talkData.Add(1000,new string[]{"안녕","집가고 싶어?"});
        //talkData.Add(100,new string[]{"평범한 문이다."});
        talkData.Add(100,new string[]{"골드가 부족합니다."});
    }
    public string GetTalk(int id,int talkIndex){
        if(talkIndex ==talkData[id].Length){
            return null;
        }else return talkData[id][talkIndex];
    }
}

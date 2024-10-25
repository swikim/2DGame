using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkInteract : MonoBehaviour
{
    [SerializeField]
    DialogueContainer dialogue;
   
   public void Interact()
    {
        Debug.Log("대화 시작");
        GameManager.Instance.dialogueSystem.Initialize(dialogue); // 대화 시작
    }
}

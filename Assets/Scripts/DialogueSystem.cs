using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Text dialogueText;
    public GameObject dialoguePanel;

    private Queue<string> dialogueQueue = new Queue<string>();

    
    
    public void Initialize(DialogueContainer dialogue){
        dialogueQueue.Clear();

        foreach (string line in dialogue.dialogueLines)
        {
            dialogueQueue.Enqueue(line); // 대사 내용을 순서대로 큐에 저장
        }

        DisplayNextLine(); // 첫 번째 대사 보여주기
        dialoguePanel.SetActive(true); // 대화창 활성화
    }

    public void DisplayNextLine(){
        if(dialogueQueue.Count == 0){
            EndDialogue();
            return;
        }
        string line = dialogueQueue.Dequeue();
        dialogueText.text = line;
    }

    public void EndDialogue(){
        dialoguePanel.SetActive(false);
    }
}

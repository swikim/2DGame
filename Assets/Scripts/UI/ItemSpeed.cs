using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/SpeedUp")]
public class ItemSpeed : ItemEffect
{
    private Dog doggy;
    private Status doggyStaus;

    public float increaseSpeed = 10f;
    public override bool ExecuteRole()
    {
        doggy = FindObjectOfType<Dog>();
        if(doggy != null){
            doggyStaus = doggy.GetStatus();
            if(doggyStaus != null){
                float moveSpeed = doggy.GetMoveSpeed();
                doggy.SetMoveSpeed(moveSpeed*5f);
                Debug.Log(doggyStaus.moveSpeed);
                return true; 
            }else{
                Debug.Log("doggyStatus를 가져오지 못했습니다");
            }
        }
        Debug.Log("객체를 찾지 못했습니다.");
        return true;
    }
}

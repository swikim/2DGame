using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/ATKSpeed")]
public class ItemATKspeed : ItemEffect
{
    private Dog doggy;
    private Status doggyStatus;
    public float ATKSpeed;

    public override bool ExecuteRole()
    {
        if(doggy !=null){
            ATKSpeed = doggy.GetAttackSpeed();
            doggy.SetAtkSpeed(ATKSpeed*1.5f);
            return true;
        }else{
            Debug.Log("Doggy Statuse를 가져오지 못했습니다.");
        }
        Debug.Log("객체를 찾을 수 없습니다.");
        return true;
    }
}

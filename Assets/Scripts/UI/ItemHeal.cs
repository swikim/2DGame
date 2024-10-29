using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Health")]
public class ItemHeal : ItemEffect
{
    private Dog doggy;
    private Status doggyStatus;
    public int healingPoint = 10;
    public override bool ExecuteRole()
    {
        doggy = FindObjectOfType<Dog>();
        if (doggy != null)  // doggy가 null이 아닌 경우에만 진행
        {
            doggyStatus = doggy.GetStatus();  // doggyStatus 가져옴

            if (doggyStatus != null)
            {
                doggyStatus.nowHp += healingPoint;  // 체력 회복
                Debug.Log(doggyStatus.nowHp);
                return true;
            }
            else
            {
                Debug.LogWarning("doggyStatus를 가져오지 못했습니다.");
            }
        }
        Debug.Log("객체를 찾을 수 없다");
        return true;
    }
}

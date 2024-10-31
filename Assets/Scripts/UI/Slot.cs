 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Slot : MonoBehaviour,IPointerUpHandler
{
    public int slotnum;
    public Item item;
    public Image itemIcon;  
    private bool isPointerUpExecuted = false;
    

    public void UpdateSlotUI(){
        itemIcon.sprite =item.itemImage;
        itemIcon.gameObject.SetActive(true);
    }

    public void RemoveSlot(){
        item =null;
        itemIcon.gameObject.SetActive(false);
    }
    

    public void OnPointerUp(PointerEventData eventData)
    {
        if (item == null || Inventory.Instance == null){
            Debug.LogWarning("Item 또는 Inventory가 null입니다.");
            return;
        }
        bool isUse = item.Use();
        Debug.Log("used");
        if(isUse){
            Inventory.Instance.RemoveItem(slotnum);
        }
    }
}

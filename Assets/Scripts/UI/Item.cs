using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{
    Equipment,
    Consumable,
    Etc
}
[System.Serializable]

public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public List<ItemEffect>efts;
    public bool Use() {
        bool isUsed = false;
        foreach(ItemEffect eft in efts) {
            if (eft.ExecuteRole()) {
                Debug.Log("item,bool use");
                isUsed = true;
            }
        }
        return isUsed;
}

}

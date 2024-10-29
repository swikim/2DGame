using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static  Inventory Instance ;
    private void Awake(){
        if(Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }    
    #endregion

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;
    public List<Item>items = new List<Item>();
    private int slotCnt;
    public int SlotCnt{
        get=>slotCnt;
        set{
            slotCnt = value;
            onSlotCountChange.Invoke(slotCnt);
        }
    }
    void Start()
    {
        SlotCnt = 4;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public bool AddItem(Item _item){
        if(items.Count < SlotCnt){
            items.Add(_item);
            if(onChangeItem != null){
                onChangeItem.Invoke();
            }
            
            return true;
        }
        return false;
    }
    public void RemoveItem(int _index){
        items.RemoveAt(_index);
        Debug.Log("remove");
        onChangeItem.Invoke();
    }
    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag=="FieldItem"){
            Debug.Log("fileditme");
            FieldItem fieldItems = col.gameObject.GetComponent<FieldItem>();
            if(AddItem(fieldItems.GetItem())) fieldItems.DestroyItem();
        }
    }
}

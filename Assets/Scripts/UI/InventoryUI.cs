using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private static InventoryUI Instance;
    Inventory inventory;
    public GameObject inventoryPanel;
    bool actInventory = false;
    public Slot[] slots;
    public Transform slotHolder;
    private void Awake(){
        if(Instance!=null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start(){
        inventory = Inventory.Instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inventory.onSlotCountChange += SlotChange;
        inventory.onChangeItem += RedrawSlotUI;
        inventoryPanel.SetActive(actInventory);
    }
    private void SlotChange(int val){
        for(int i = 0; i< slots.Length;i++){
            slots[i].slotnum = i;

            if(i < inventory.SlotCnt){
                slots[i].GetComponent<Button>().interactable = true;
                Debug.Log("add");
            }else{
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.I)){
            actInventory = !actInventory;
            inventoryPanel.SetActive(actInventory);
        }
    }
    public void AddSlot(){
        inventory.SlotCnt++;
    }
    
    void RedrawSlotUI(){
        for(int i=0; i<slots.Length;i++){
            slots[i].RemoveSlot();
        }
        for(int i = 0; i < inventory.items.Count;i++){
            slots[i].item = inventory.items[i];
            slots[i].UpdateSlotUI();

        }
    }
}

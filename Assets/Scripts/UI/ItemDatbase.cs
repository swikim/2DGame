using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDatbase : MonoBehaviour
{
    
    public static ItemDatbase instance;
    private void Awake(){
            if(instance == null){
                instance = this;
                DontDestroyOnLoad(gameObject);
            }else{
                Destroy(gameObject);
            }
        }
    void Start(){
        instance = this;
    }
    public List<Item>itemDB = new List<Item>();
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class ItemDatbase : MonoBehaviour
{
    // Start is called before the first frame update
    public static ItemDatbase instance;

    void Start(){
        instance = this;
    }
    public List<Item>itemDB = new List<Item>();
}

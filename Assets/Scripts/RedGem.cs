using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Player"){
            Dog doggy = col.gameObject.GetComponent<Dog>();
            Debug.Log("heal");

            int nowHp = doggy.GetNowHp();
            
            nowHp = HealHp(nowHp);
            doggy.SetnowHp(nowHp);
            
            Destroy(gameObject);
        }
    }
    private int HealHp(int hp){
        hp += 10;
        return hp;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGem : MonoBehaviour
{

    
    private void OnCollisionEnter2D(Collision2D col){
    if(col.gameObject.tag == "Player"){
        Dog doggy = col.gameObject.GetComponent<Dog>();
        StartCoroutine(IncreaseMoveSpeed(doggy));
        
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }
}

IEnumerator IncreaseMoveSpeed(Dog doggy){
    float moveSpeed = doggy.GetMoveSpeed();
    doggy.SetMoveSpeed(moveSpeed * 5f);

    yield return new WaitForSeconds(10);

    doggy.SetMoveSpeed(moveSpeed);
    Destroy(gameObject);
}
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Player"){
            Dog doggy = col.gameObject.GetComponent<Dog>();
        
        StartCoroutine(IncreaseAtkSpeed(doggy));

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator IncreaseAtkSpeed(Dog doggy){
        float atkSpeed = doggy.GetAttackSpeed();
        doggy.SetAtkSpeed(doggy.GetAttackSpeed()*1.5f);
        yield return new WaitForSeconds(10);

        doggy.SetAtkSpeed(atkSpeed);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private float moveSpeed = 10f;

    private float minY = -7;
    public Dog doggy;
    private Status doggyStatus;
    public void SetMoveSpeed(float moveSpeed){
        this.moveSpeed = moveSpeed;
    }
    private void Start(){
        if(doggy == null){
            doggy = GameObject.Find("Doggy").GetComponent<Dog>();
        }
        doggyStatus = doggy.GetStatus();
    }

    void Update(){
        transform.position += Vector3.down*moveSpeed*Time.deltaTime;
        if(transform.position.y<minY){
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Player"){
            doggyStatus.nowHp -= 10;
            Debug.Log(doggyStatus.nowHp);
        }
    }
}

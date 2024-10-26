using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Jump();
    }
    void Jump(){
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        float randomJumpForce = Random.Range(3f,7f);
        Vector2 jumpVelocity = Vector2.up * randomJumpForce;
        jumpVelocity.x = Random.Range(-2f,2f);
        rigidbody.AddForce(jumpVelocity, ForceMode2D.Impulse);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

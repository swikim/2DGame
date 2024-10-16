using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using UnityEngine.Video;

public class Dog : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;
    
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed = 1f;
    public bool attacked = false;

    public float jumpPower = 40f;
    bool inputJump = false;

    public UnityEngine.UI.Image nowHpbar;
    void AttackTrue(){
        attacked = true;
    }
    void AttackFalse(){
        attacked = false;
    }
    public void SetAttackSpeed(float speed){
        animator.SetFloat("attackSpeed", speed);
        atkSpeed = speed;
    }
    public GameObject doggy;

    public GameObject skillPrefab;   // 스킬 프리팹을 연결할 변수
    private GameObject currentSkill;
    [SerializeField]
    private Transform AttackTransform;
   

    Animator animator;

    Rigidbody2D rigid2D;

    Collider2D col2D;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = 50;
        nowHp = 50;
        atkDmg = 10;

        //transform.position = new Vector3(0, 0,0);
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
        SetAttackSpeed(1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
        // float horiznotalInput = Input.GetAxisRaw("Horizontal");
        // float verticalInput = Input.GetAxisRaw("Vertical");
        // Vector3 moveTo = new(horiznotalInput, 0f,0f);
        // transform.position += moveSpeed * Time.deltaTime * moveTo;

        float h = Input.GetAxis("Horizontal");
        if(h>0){
            transform.localScale = new Vector3(1,1,1);
            animator.SetBool("moving",true);
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);

        }
        else if (h<0){
            transform.localScale = new Vector3(-1,1,1);
            animator.SetBool("moving",true);
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }
        else animator.SetBool("moving",false);

        if(Input.GetKey(KeyCode.Z)&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
            animator.SetTrigger("attack");
            SpawnSkill();

            Debug.Log("Attack");
        }
        if(Input.GetKeyDown(KeyCode.C)&&!animator.GetBool("jumping")){
            inputJump = true;
        }

        RaycastHit2D raycastHit = Physics2D.BoxCast(col2D.bounds.center,col2D.bounds.size,0f,Vector2.down,LayerMask.GetMask("ground"));
        if(raycastHit.collider != null){
            animator.SetBool("jumping",false);
        }
        else animator.SetBool("jumping",true);
    }
    void FixedUpdate(){
        if(inputJump){
            inputJump = false;
            rigid2D.AddForce(Vector2.up*jumpPower);
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            Enemy enemy = other.GetComponent<Enemy>();
            nowHp -= enemy.atkDmg;
            if(nowHp <= 0 ){
                Destroy(gameObject);
            }
        }
    }
    void SpawnSkill(){
            if(currentSkill == null){

                currentSkill = Instantiate(skillPrefab,AttackTransform.position,Quaternion.identity);
                currentSkill.tag = "Attack";  // 태그를 수동으로 설정

            }
            StartCoroutine(RemoveSkillAfterDelay(0.5f));

        }

    IEnumerator RemoveSkillAfterDelay(float v)
    {
        yield return new WaitForSeconds(v);
        if (currentSkill != null)
        {
            Destroy(currentSkill);
            currentSkill = null;
        }
    }
}

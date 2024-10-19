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
    public float moveSpeed = 10f;
    
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed = 1f;
    public bool attacked = false;
    public bool death = false;

    public float jumpPower = 40f;
    bool inputJump = false;

    public UnityEngine.UI.Image nowHpbar;
    void AttackTrue(){
        attacked = true;
    }
    void AttackFalse(){
        attacked = false;
    }

    private bool isJumping = false;
    
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
    bool inputRight = false;
    bool inputLeft = false;

    void Update()
    {
        if(death) return;
        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
        //이동 입력 처리
        if(Input.GetKey(KeyCode.RightArrow)){
            inputRight = true;
            transform.localScale = new Vector3(1,1,1);
            animator.SetBool("moving",true);
        }else if(Input.GetKey(KeyCode.LeftArrow)){
            inputLeft = true;
            transform.localScale=new Vector3(-1,1,1);
            animator.SetBool("moving",true);
        }else animator.SetBool("moving",false);
        // 공격 처리
        if(Input.GetKey(KeyCode.Z)&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
            animator.SetTrigger("attack");
            SpawnSkill();

            Debug.Log("Attack");
        }
        // 점프 입력 처리
        if (Input.GetKeyDown(KeyCode.C) && !isJumping) { // 점프 중이 아닐 때만 점프 입력을 받음
            inputJump = true;
        }

        // 바닥 체크
        RaycastHit2D raycastHit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 1f, Vector2.down, LayerMask.GetMask("Ground"));
        
        if (raycastHit.collider != null) {
            animator.SetBool("jumping", false);
            isJumping = false; // 착지 시 점프 상태를 false로 설정
        } else {
            animator.SetBool("jumping", true);
        }
        
    }
    void FixedUpdate(){
        Vector2 currentVelocity = rigid2D.velocity;
        if (inputRight){
            inputRight = false;
            rigid2D.AddForce(Vector2.right * moveSpeed);
        }
        if (inputLeft)
        {
            inputLeft = false;
            rigid2D.AddForce(Vector2.left * moveSpeed);
        }
        //점프 처리
        if (inputJump && !isJumping) {
        // 바닥에 있을 때만 점프 실행
        if (Mathf.Abs(currentVelocity.y) < 0.001f) { // 바닥에 있을 때
            inputJump = false; // 점프 입력을 초기화
            isJumping = true; // 점프 중임을 표시
            rigid2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); // 점프 힘을 적용
        }
    }

    // 중력 적용
        currentVelocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;  // 중력 효과 추가


        if (rigid2D.velocity.x >= 2.5f) rigid2D.velocity = new Vector2(2.5f, rigid2D.velocity.y);
        else if (rigid2D.velocity.x <= -2.5f) rigid2D.velocity = new Vector2(-2.5f, rigid2D.velocity.y);    }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            Enemy enemy = other.GetComponent<Enemy>();
            nowHp -= enemy.atkDmg;
            if(nowHp <= 0 ){
                Destroy(gameObject);
            }
        }
        if(other.gameObject.tag == "EnemyAttack"){
            Attack attack = other.GetComponent<Attack>();
            nowHp -= attack.EnemyAtkDmg;
            if(nowHp <=0){
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

    public void SetMoveSpeed(float speed){
        moveSpeed = speed;
    }
    public float GetMoveSpeed(){
        return moveSpeed;
    }
    public int GetNowHp(){
        return nowHp;
    }
    public void SetnowHp(int hp){
        nowHp = hp;
    }
    public float GetAttackSpeed(){
        return atkSpeed;
    }
    public void SetAtkSpeed(float aspeed){
        atkSpeed = aspeed;
    }
}


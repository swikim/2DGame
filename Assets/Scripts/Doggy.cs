using System;
using System.Collections;
using JetBrains.Annotations;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class Dog : MonoBehaviour
{
    private Enemy enemy;
    private Status enemyStatus;
    private Status doggyStatus;
    public UnitCode unitCode;
    [SerializeField]
    public float moveSpeed = 10f;
    private float minY = -15f;
    public bool attacked = false;
    public bool death = false;

    private bool isTouchingPortal = false;    
     public float jumpPower = 40f;
    bool inputJump = false;
    int jumpCount = 0;

    public UnityEngine.UI.Image nowHpbar;
    public float interactRange = 2f;
    void AttackTrue(){
        attacked = true;
    }
    void AttackFalse(){
        attacked = false;
    }
    private bool isJumping = false;

    Vector3 dirVec;    
    public void SetAttackSpeed(float speed){
        animator.SetFloat("attackSpeed", speed);
        doggyStatus.atkSpeed = speed;
    }
    public GameManager gameManager;
    public GameObject doggy;
    public GameObject skillPrefab;   // 스킬 프리팹을 연결할 변수
    private GameObject currentSkill;
    GameObject scanObject;

    [SerializeField]
    private Transform AttackTransform;
    

    Animator animator;

    Rigidbody2D rigid2D;

    Collider2D col2D;
    
    
    void Start()
    {
        if(gameManager==null){
            gameManager = FindObjectOfType<GameManager>();
        }
        doggyStatus = new Status();
        doggyStatus = doggyStatus.SetUnitStatus(unitCode);
        

        //transform.position = new Vector3(0, 0,0);
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
        if(gameManager ==null){
            gameManager = FindAnyObjectByType<GameManager>();
        }
        SetAttackSpeed(1.5f);
    }
    bool inputRight = false;
    bool inputLeft = false;

    void Update()
    {
        if(death) return;
        nowHpbar.fillAmount = (float)doggyStatus.nowHp / (float)doggyStatus.maxHp;
        //이동 입력 처리
        if(Input.GetKey(KeyCode.RightArrow)&&!gameManager.isAction){
            inputRight = true;
            animator.SetBool("moving",true);
            transform.localScale = new Vector3(1,1,1);
            dirVec = Vector3.right;
        }else if(Input.GetKey(KeyCode.LeftArrow)&&!gameManager.isAction){
            inputLeft = true;
            animator.SetBool("moving",true);
            transform.localScale=new Vector3(-1,1,1);
            dirVec = Vector3.left;
        }else animator.SetBool("moving",false);
        // 공격 처리
        if(Input.GetKey(KeyCode.Z)&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
            animator.SetTrigger("attack");
            SpawnSkill();

            //Debug.Log("Attack");
        }
        // 점프 입력 처리
         if (Input.GetKeyDown(KeyCode.C) && !isJumping) { // 점프 중이 아닐 때만 점프 입력을 받음
            inputJump = true;
            jumpCount++;
        }
        if(Input.GetKeyDown(KeyCode.X)&&isJumping){
            if(jumpCount<1){
                jumpCount++; 
                DoubleJump();
            }
        }
        // 대화 입력 처리
        if(Input.GetKeyDown(KeyCode.E)&&scanObject != null){
            gameManager.Action(scanObject);
            Debug.Log("npc"+scanObject.name);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)&&isTouchingPortal){
            if(gameManager.gold >=1){
                SceneManager.LoadScene("SecondScene");
                transform.position = new Vector3(-14, -2, 0);
            }else{
                gameManager.ShowNotEough();
            }
        }
           
        // 바닥 체크
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(col2D.bounds.center, col2D.bounds.size, 0f, LayerMask.GetMask("Ground","Object"));
        if (hitColliders.Length > 0) {
            animator.SetBool("jumping", false);
            isJumping = false; // 착지 시 점프 상태를 false로 설정
            jumpCount = 0;
        } else {
            animator.SetBool("jumping", true);
        }

        if(transform.position.y<minY){
            Destroy(gameObject);
            GameManager.Instance.ShowGameOverPanel();
        }
        
        
        Debug.DrawRay(transform.position, dirVec * 1f, Color.green);
        
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
        else if (rigid2D.velocity.x <= -2.5f) rigid2D.velocity = new Vector2(-2.5f, rigid2D.velocity.y); 

        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position,dirVec,1.0f,LayerMask.GetMask("Object"));
        if(raycastHit.collider != null){
            scanObject = raycastHit.collider.gameObject;
        }
        else{
            scanObject = null;
        }
     }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            Enemy enemy = other.GetComponent<Enemy>();
            enemyStatus = enemy.getStatus();

            doggyStatus.nowHp -= enemyStatus.atkDmg;
            if(doggyStatus.nowHp <= 0 ){
                Destroy(gameObject);
            }
        }
        if(other.gameObject.tag == "EnemyAttack"){
            Attack attack = other.GetComponent<Attack>();
            doggyStatus.nowHp -= attack.EnemyAtkDmg;
            if(doggyStatus.nowHp <=0){
                Destroy(gameObject);
            }
        }
        if(other.gameObject.tag == "Potal"){
            Debug.Log("Touched");
            isTouchingPortal = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Gold"){
            gameManager.IncreaseGold();
            Destroy(col.gameObject);
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
        return doggyStatus.nowHp;
    }
    public void SetnowHp(int hp){
        doggyStatus.nowHp = hp;
    }
    public float GetAttackSpeed(){
        return doggyStatus.atkSpeed;
    }
    public void SetAtkSpeed(float atkspeed){
        doggyStatus.atkSpeed = atkspeed;
    }
    public Status GetStatus(){
        return doggyStatus;
    }

    void DoubleJump(){
        Vector2 jumpVelocity = Vector2.up*jumpPower;
        jumpVelocity.x = transform.localScale.x;
        rigid2D.AddForce(jumpVelocity,ForceMode2D.Impulse);
    }
}


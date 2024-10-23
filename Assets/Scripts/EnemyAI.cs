using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    float attackDelay;
    Dog dog;
    private Enemy enemy;
    private Dog doggy;
    private Status enemyStatus;
    private Status doggyStatus;
    Animator enemyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        doggy = GetComponent<Dog>();
        enemyAnimator = enemy.enemyAnimator;
        enemyStatus = enemy.getStatus();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || target.GetComponent<Dog>()==null){
            GameManager.Instance.ShowGameOverPanel();
            return;
        }
        doggyStatus = target.GetComponent<Dog>().GetStatus();
        enemyStatus = enemy.getStatus();
        if(doggyStatus.nowHp<=0){
            GameManager.Instance.ShowGameOverPanel();
            return;
        }
        attackDelay -= Time.deltaTime;
        if (attackDelay < 0) attackDelay = 0;

        // 적과 플레이어 사이의 거리 계산
        float distance = Vector3.Distance(transform.position, target.position);

        // 시야 범위 안에 있는 경우 처리
        if (distance <= enemyStatus.fieldOfVision)
        {
            FaceTarget();  // 적이 플레이어를 바라봄

            if (distance <= enemyStatus.atkRange && attackDelay == 0) // 공격 범위 내이고 딜레이가 끝났을 때
            {
                AttackTarget();
            }
            else if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) // 공격 중이 아닐 때만 이동
            {
                MoveToTarget(); // 플레이어를 향해 이동
            }
        }
        else
        {
            enemyAnimator.SetBool("moving", false); // 시야 밖에 있으면 이동 멈춤
        }

    }
    void MoveToTarget(){
        float dir = target.position.x - transform.position.x;
        dir = (dir<0)?-1:1;
        transform.Translate(new Vector2(dir,0)*enemyStatus.moveSpeed*Time.deltaTime);
        enemyAnimator.SetBool("moving",true);
        Debug.Log("moveTotarget");
    }
    void FaceTarget(){
        if(target.position.x - transform.position.x < 0){
            transform.localScale = new Vector3(-1,1,1);
        }else{ //오른쪽에 있을 때
            transform.localScale = new Vector3(1,1,1);
        }
                Debug.Log("faceTotarget");

    }
    void AttackTarget(){
        doggyStatus = target.GetComponent<Dog>().GetStatus();
        doggyStatus.nowHp -= enemyStatus.atkDmg;
        enemyAnimator.SetTrigger("attack"); 
        attackDelay = enemyStatus.atkSpeed;
        Debug.Log("Attakc Player");
        enemy.SpawnSkill();
    }
}

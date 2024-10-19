using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject skillPrefab;
    private GameObject currentSkill;

    [SerializeField]
    private Transform AttackTransform;
    public GameObject prfHpBar;
    public GameObject canvas;

    RectTransform hpBar;

    public string enemyName;
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed;
    public float moveSpeed;
    public float atkRange;
    public float fieldOfVision;


    private void SetEnemyStatus(string _enemyName,int _maxHp,int _atkDmg,float _atkSpeed,float _moveSpeed,float _atkRange,float _fieldOfVision)
    {
        enemyName = _enemyName;
        maxHp = _maxHp;
        nowHp = _maxHp;
        atkDmg = _atkDmg;
        atkSpeed = _atkSpeed;
        moveSpeed = _moveSpeed;
        atkRange = _atkRange;
        fieldOfVision = _fieldOfVision;
    }

    public Dog doggy;
    Image nowHpbar;

    public float height =1.7f;
    public Animator enemyAnimator;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        hpBar = Instantiate(prfHpBar,canvas.transform).GetComponent<RectTransform>();
        if(name.Equals("Enemy1")){
            SetEnemyStatus("Enemy1",100,10,1.5f,2,1.5f,7f);
        }
        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image> ();

        SetAttackSpeed(atkSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
            (new Vector3(transform.position.x, transform.position.y + height, 0));        
            hpBar.position = _hpBarPos;
        nowHpbar.fillAmount = (float)nowHp/(float)maxHp;
    }

    private void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Attack"){
            Attack attack = col.gameObject.GetComponent<Attack>();
            nowHp -= attack.damage;
            if(nowHp <= 0){
                Die();
            }
        }
    }
    void Die(){
        enemyAnimator.SetTrigger("die");
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(gameObject, 2);
        Destroy(hpBar.gameObject, 2);
    }
    void SetAttackSpeed(float speed){
        enemyAnimator.SetFloat("attackSpeed",speed);
        atkSpeed = speed;
    }
    
    public void SpawnSkill(){
        if(currentSkill == null){
            currentSkill = Instantiate(skillPrefab,AttackTransform.position,Quaternion.identity);
            currentSkill.tag = "EnemyAttack";
            Debug.Log("spawnskillllll");
        }
        StartCoroutine(RemoveSkillAfterDelay(0.5f));
    }

    IEnumerator RemoveSkillAfterDelay(float v){
        yield return new WaitForSeconds(v);
        if(currentSkill != null){
            Destroy(currentSkill);
        }
    }

}

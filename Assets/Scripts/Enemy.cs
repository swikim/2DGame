using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Status status;
    public bool isDead = false;
    public Vector3 spawnVector;
    public UnitCode unitCode;
    public GameObject skillPrefab;
    private GameObject currentSkill;
    [SerializeField]
    private GameObject gold;

    [SerializeField]
    private Transform AttackTransform;
    public GameObject prfHpBar;
    public Canvas canvas;

    RectTransform hpBar;
    public GameObject doggyG;

    public Dog doggy;
    Image nowHpbar;
    private bool hasExecuted = false;

    public float height =1.7f;
    private float minY = -15f;
    public Animator enemyAnimator;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        if(canvas ==null){
            canvas = GameObject.Find("canvas").GetComponent<Canvas>();
        }
        if(doggy ==null){
            doggy = GameObject.Find("Doggy").GetComponent<Dog>();
        }
        hpBar = Instantiate(prfHpBar,canvas.transform).GetComponent<RectTransform>();
        
        status = new Status();
        status = status.SetUnitStatus(unitCode);

        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image> ();

        spawnVector = transform.position;

        SetAttackSpeed(status.atkSpeed);

        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
            (new Vector3(transform.position.x, transform.position.y + height, 0));        
            hpBar.position = _hpBarPos;
            nowHpbar.fillAmount = (float)status.nowHp/(float)status.maxHp;

        if(!hasExecuted){
            if(transform.position.y < minY){
                Die();
                hasExecuted = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Attack"){
            Attack attack = col.gameObject.GetComponent<Attack>();
            status.nowHp -= attack.damage;
            if(status.nowHp <= 0){
                //GameManager.Instance.SetClear();
                Die();
                
            }
        }
    }
    void Die(){
        enemyAnimator.SetTrigger("die");
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(gameObject, 1);
        Destroy(hpBar.gameObject, 1);
        isDead = true;
        EnemySpawnser.Instance.OnEnemyDeath(this);
        Instantiate(gold,transform.position,Quaternion.identity);
    }
    
    void SetAttackSpeed(float speed){
        enemyAnimator.SetFloat("attackSpeed",speed);
        status.atkSpeed = speed;
    }
    
    public void SpawnSkill(){
        if(currentSkill == null){
            currentSkill = Instantiate(skillPrefab,AttackTransform.position,Quaternion.identity);
            currentSkill.tag = "EnemyAttack";
        }
        StartCoroutine(RemoveSkillAfterDelay(0.5f));
    }

    IEnumerator RemoveSkillAfterDelay(float v){
        yield return new WaitForSeconds(v);
        if(currentSkill != null){
            Destroy(currentSkill);
        }
    }

    public Status getStatus(){
        return status;
    }

}

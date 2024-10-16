using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject prfHpBar;
    public GameObject canvas;

    RectTransform hpBar;

    public string enemyName;
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public int atkSpeed;

    private void SetEnemyStatus(string _enemyName,int _maxHp,int _atkDmg,int _atkSpeed){
        enemyName = _enemyName;
        maxHp = _maxHp;
        nowHp = _maxHp;
        atkDmg = _atkDmg;
        atkSpeed = _atkSpeed;
    }

    public Dog doggy;
    Image nowHpbar;

    public float height =1.7f;
    void Start()
    {
        hpBar = Instantiate(prfHpBar,canvas.transform).GetComponent<RectTransform>();
        if(name.Equals("Enemy1")){
            SetEnemyStatus("Enemy1",100,10,1);
        }
        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image> ();
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
                Destroy(gameObject);
                Destroy(hpBar.gameObject);
            }
        }
    }
    

}

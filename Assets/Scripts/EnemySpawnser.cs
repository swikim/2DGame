using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnser : MonoBehaviour
{
    public Transform target;
    Enemy enemy;
    Status enemyStatus;

    public Canvas canvas;
    private float[] arrPosx = {1f,3f};
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private float spawnInterval = 7f;

    private Dog doggy;


    void Start()
    {   
        transform.position = new Vector3(0, 1, 0);
        StartEnemyRoutine();
    }
    void StartEnemyRoutine(){
        StartCoroutine("EnemyRoutine");
    }
    public void StopEnemyRoutine(){
        StopCoroutine("EnemyRoutine");
    }
    IEnumerator EnemyRoutine(){
        yield return new WaitForSeconds(2f);   
        int enemyIndex = 0;
        while(true){
            foreach(float posX in arrPosx){
                SpawnEnemy(posX, enemyIndex);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    void SpawnEnemy(float PosX,int index){
        Vector3 spawnPos = new Vector3(PosX, transform.position.y,transform.position.z);
        

        GameObject enemyObject = Instantiate(enemies[index],spawnPos,Quaternion.identity);
        enemyObject.GetComponent<Enemy>().canvas = canvas; // 프리팹 생성 후 Canvas 할당
        Dog doggy = FindObjectOfType<Dog>();
    if (doggy != null)
    {
        // Dog 오브젝트를 GameObject로 변환
        GameObject doggyGameObject = doggy.gameObject;
        enemyObject.GetComponent<Enemy>().doggyG = doggyGameObject; // 찾은 Dog 오브젝트 할당

        // EnemyAI에 target 할당
        EnemyAI enemyAI = enemyObject.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.target = doggy.transform; // doggy의 Transform을 target으로 할당
            Debug.Log("Target assigned: " + doggy.name); // 타겟 이름 출력
        }
    }
    else
    {
        Debug.LogError("Dog object not found in the scene!");
    }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

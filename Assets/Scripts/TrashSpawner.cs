using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] trashes;
    private float[] arrPosx;
    [SerializeField]
    private float spawnInterval = 1.5f;

    private void Start(){
        StartCoroutine("StartTrashRoutine");
        List<float> positions = new List<float>();
        for (float i = 17f; i <= 35f; i += 2f) {
            positions.Add(i);
        }
        arrPosx = positions.ToArray(); 
    }
    void StartTrashRoutine(){
        StartCoroutine("TrashRoutine");
    }
    public void StopTrashCoroutine(){
        StopCoroutine("TrashRoutine");
    }

    IEnumerator TrashRoutine(){
        yield return new WaitForSeconds(5f);
        float moveSpeed = Random.Range(4f,6f);
        int trashIndex = SetIndex();
        bool spawnT;
        while(true){
            foreach(float posX in arrPosx){
                trashIndex = SetIndex();
                spawnT= Random.Range(0,2)==0;
                if(spawnT){
                    SpawnTrash(posX, trashIndex,moveSpeed);
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    public int SetIndex(){
        int index = Random.Range(0,trashes.Length);
        return index;
    }
    void SpawnTrash(float PosX, int trashIndex,float moveSpeed){
        Vector3 spawnPos = new Vector3(PosX,transform.position.y,transform.position.z);

        GameObject trashObject = Instantiate(trashes[trashIndex],spawnPos,Quaternion.identity);
        Trash trash = trashObject.GetComponent<Trash>();
        moveSpeed = Random.Range(2f,10f);
        trash.SetMoveSpeed(moveSpeed);
    }
}

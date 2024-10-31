using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform player; // 따라다닐 Player의 Transform
    public Vector3 offset; // 카메라와 Player 사이의 거리
    public float smoothSpeed = 0.125f; // 부드러운 이동 속도

    //카메라가 이동할 수 있는 범위 설정
    public Vector2 minLimit;
    public Vector2 maxLimit;

    void Start(){
        offset = transform.position - player.position;
        player = FindObjectOfType<Dog>().transform;
    }
    void LateUpdate()
    {
        if(player == null){
            return;
        }
        // 목표 위치를 설정 (Player의 위치 + 오프셋)
        Vector3 desiredPosition = player.position + offset;
        float clampedX = Mathf.Clamp(desiredPosition.x,minLimit.x,maxLimit.x);
        float clampedY = Mathf.Clamp(desiredPosition.y,minLimit.y,maxLimit.y);

        //제한된 좌표로 카메라 이동
        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);
        // 카메라의 현재 위치에서 목표 위치로 부드럽게 이동
        Vector3 smoothedPosition = Vector3.Lerp(player.position, clampedPosition, smoothSpeed);
        
        // 카메라 위치를 업데이트
        transform.position = smoothedPosition;
        

        
    }
}

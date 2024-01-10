using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 카메라 이동 속도

    void Update()
    {
        // 키보드 입력을 통해 카메라 이동
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;

        // 현재 위치에 이동 벡터를 더합니다.
        transform.Translate(movement);
    }
}
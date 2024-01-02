using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    CinemachineBrain mainCam;
    public CinemachineVirtualCameraBase startCam; // **** 주의 : 시작할 카메라가 없으면 기존에 보여주고 있는 카메라를 보여줌, 있다면 vertualCamera여야 함
    public CinemachineBlendListCamera blendListCamera;

    private void Awake()
    {
        mainCam = Camera.main.GetComponent<CinemachineBrain>();
        if(startCam != null)
            startCam.MoveToTopOfPrioritySubqueue();
    }

    public void ChangeCam(GameObject changingCam)
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    CinemachineBrain mainCam;
    public CinemachineVirtualCameraBase startCam; // **** ���� : ������ ī�޶� ������ ������ �����ְ� �ִ� ī�޶� ������, �ִٸ� vertualCamera���� ��
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

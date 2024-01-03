using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera startCam; // **** ���� : ������ ī�޶� ������ ������ �����ְ� �ִ� ī�޶� ������, �ִٸ� vertualCamera���� ��
    public CinemachineBlendListCamera blendListCamera;
    CinemachineVirtualCamera currentCam;
    


    private void Awake()
    {
        if(startCam != null){
            startCam.MoveToTopOfPrioritySubqueue();
            currentCam = startCam;
        }
    }

    public void ChangeBlendListCamSetting(bool useCurrentCam, List<ChangingCameraInfo> camList)
    {
        for(int i=0; i<camList.Count; i++){
            if(camList[i] != null){
                
            }
        }
    }
}

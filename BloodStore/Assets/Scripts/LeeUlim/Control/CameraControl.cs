using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera startCam; // **** 주의 : 시작할 카메라가 없으면 기존에 보여주고 있는 카메라를 보여줌, 있다면 vertualCamera여야 함
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using Unity.VisualScripting;

public class CameraControl : MonoBehaviour
{
    public CinemachineBlendListCamera blendListCam;
    public CinemachineVirtualCamera subCam;

    CinemachineBrain mainCam;
    


    private void Awake()
    {
        mainCam = Camera.main.GetComponent<CinemachineBrain>();
    }

    public void ChangeBlendListCamSetting(InteractObjInfo interObj)
    {
        int index = 1;
        List<CinemachineBlendListCamera.Instruction> newList = new();
        blendListCam.m_Instructions = newList.ToArray<CinemachineBlendListCamera.Instruction>();
        
        // set other subCams
        for(int i=0; i<interObj._changingCamList.Count; i++){
            if(interObj._changingCamList[i] != null){
                CinemachineVirtualCamera temp = Instantiate(subCam);
                temp.Follow = interObj._changingCamList[i].target.transform;
                temp.transform.SetParent(blendListCam.transform);

                blendListCam.m_Instructions[index].m_VirtualCamera = temp;

                blendListCam.m_Instructions[index-1].m_Hold  = interObj._changingCamList[i].backCamHold;
                blendListCam.m_Instructions[index].m_Blend.m_Style = interObj._changingCamList[i].blendIn;
                blendListCam.m_Instructions[index].m_Blend.m_Time = interObj._changingCamList[i].blendTime;
                index++;
            }
        }

        
    }
}

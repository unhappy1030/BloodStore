using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class CameraControl : MonoBehaviour
{
    CinemachineBrain mainCam;

    int camCount = 0;

    private void Awake()
    {
        mainCam = Camera.main.GetComponent<CinemachineBrain>();
    }

    public void ChangeCam(InteractObjInfo interObj)
    {
        GameObject temp = new GameObject("CreatedCam"+camCount);
        temp.transform.SetParent(transform);
        temp.transform.SetAsFirstSibling();

        switch(interObj._cameraType){
            case CameraType.VirtualCamera:
                temp.AddComponent<CinemachineVirtualCamera>();
                CinemachineVirtualCamera newVirtualCam = temp.GetComponent<CinemachineVirtualCamera>();

                VirtualCameraInfo virtualCam = interObj._vertualCam;
                newVirtualCam.m_Follow = virtualCam.target.transform;
                // newVirtualCam.m_Lens.OrthographicSize = 5.4f;

                newVirtualCam.AddCinemachineComponent<CinemachineFramingTransposer>();

                mainCam.m_DefaultBlend.m_Time = virtualCam.blendInfo.blendTime;
                mainCam.m_DefaultBlend.m_Style = virtualCam.blendInfo.blendIn;


                StartCoroutine(MoveTopOfCam(temp.GetComponent<CinemachineVirtualCameraBase>(), virtualCam.blendInfo.hold));
                break;

            case CameraType.TargetGroupCamera:
                temp.AddComponent<CinemachineVirtualCamera>();
                CinemachineVirtualCamera newTargetCam = temp.GetComponent<CinemachineVirtualCamera>();
                
                GameObject group = new GameObject();
                group.AddComponent<CinemachineTargetGroup>();
                CinemachineTargetGroup targetGroup = group.GetComponent<CinemachineTargetGroup>();

                TargetGroupCameraInfo targetCam = interObj._targetGroupCam;
                // set targetgroup and apply to m_follow 
                for(int i=0; i<targetCam.targets.Count(); i++) 
                    targetGroup.AddMember(targetCam.targets[i].transform, 1, 1);

                newTargetCam.m_Follow = group.transform;
                
                newTargetCam.AddCinemachineComponent<CinemachineFramingTransposer>();
                
                mainCam.m_DefaultBlend.m_Time = targetCam.blendInfo.blendTime;
                mainCam.m_DefaultBlend.m_Style = targetCam.blendInfo.blendIn;

                StartCoroutine(MoveTopOfCam(temp.GetComponent<CinemachineVirtualCameraBase>(), targetCam.blendInfo.hold));
                break;

            case CameraType.BlendListCamera:
                temp.AddComponent<CinemachineBlendListCamera>();
                CinemachineBlendListCamera newBlendCam = temp.GetComponent<CinemachineBlendListCamera>();

                BlendListCameraInfo blendCam = interObj._blendListCam;
                
                List<CinemachineBlendListCamera.Instruction> tempList = new();

                for(int i=0; i<blendCam.subCams.Count(); i++){
                    GameObject newSubCam = new("CrearedSubCam"+i);
                    newSubCam.AddComponent<CinemachineVirtualCamera>();
                    newSubCam.transform.SetParent(newBlendCam.transform);
                    newSubCam.transform.SetAsLastSibling();
                    CinemachineVirtualCamera newSubVirtualCam = newSubCam.GetComponent<CinemachineVirtualCamera>();

                    CinemachineBlendListCamera.Instruction instruction = new();
                    
                    switch(blendCam.subCams[i].cameraType){
                        case CameraType.VirtualCamera:
                            newSubVirtualCam.m_Follow = blendCam.subCams[i].virtualCam.target.transform;
                            instruction.m_VirtualCamera = newSubVirtualCam;

                            if(i != 0){
                                instruction.m_Hold = blendCam.subCams[i].virtualCam.blendInfo.hold;
                                instruction.m_Blend.m_Style = blendCam.subCams[i].virtualCam.blendInfo.blendIn;
                                instruction.m_Blend.m_Time = blendCam.subCams[i].virtualCam.blendInfo.blendTime;
                            }
                            
                            newSubVirtualCam.AddCinemachineComponent<CinemachineFramingTransposer>();

                            break;
                        case CameraType.TargetGroupCamera:

                            break;
                    }
                    tempList.Add(instruction);
                }

                newBlendCam.m_Instructions = tempList.ToArray();

                mainCam.m_DefaultBlend.m_Time = blendCam.blendInfo.blendTime;
                mainCam.m_DefaultBlend.m_Style = blendCam.blendInfo.blendIn;

                StartCoroutine(MoveTopOfCam(temp.GetComponent<CinemachineVirtualCameraBase>(), blendCam.blendInfo.hold));
                break;
        }
        camCount++;
    }

    IEnumerator MoveTopOfCam(CinemachineVirtualCameraBase cam, float hold){
        yield return new WaitForSecondsRealtime(hold);
        Debug.Log("Waiting...");
        cam.MoveToTopOfPrioritySubqueue();
    }
}

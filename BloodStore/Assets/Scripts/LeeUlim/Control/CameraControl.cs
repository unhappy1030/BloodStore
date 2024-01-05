using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using Unity.VisualScripting;

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
        // create new object for main Camera and set this as Parent
        GameObject temp = new("CreatedCam"+camCount);
        temp.SetActive(false);
        temp.transform.SetParent(transform);
        temp.transform.SetAsFirstSibling();

        switch(interObj._cameraType){
            case CameraType.VirtualCamera:
                // add Cinemachine component to new object
                CinemachineVirtualCamera newVirtualCam = temp.AddComponent<CinemachineVirtualCamera>(); // new Camera Info
                VirtualCameraInfo virtualCam = interObj._vertualCam; // Camera Info in Inspector
                
                newVirtualCam.m_Follow = virtualCam.target.transform;
                newVirtualCam.m_Lens.OrthographicSize = virtualCam.lensOthoSize;
                newVirtualCam.AddCinemachineComponent<CinemachineFramingTransposer>();

                mainCam.m_DefaultBlend.m_Time = virtualCam.blendInfo.blendTime;
                mainCam.m_DefaultBlend.m_Style = virtualCam.blendInfo.blendIn;

                // hold in Main Camera == coroutine delay time
                StartCoroutine(MoveTopOfCam(temp.GetComponent<CinemachineVirtualCameraBase>(), virtualCam.blendInfo.hold));
                break;

            case CameraType.TargetGroupCamera:
                // add Cinemachine component to new object
                CinemachineVirtualCamera newTargetCam = temp.AddComponent<CinemachineVirtualCamera>(); // new Camera Info
                TargetGroupCameraInfo targetCam = interObj._targetGroupCam; // Camera Info in Inspector
                
                // create Target Group for Camera
                GameObject group = new("TargetGroup");
                CinemachineTargetGroup targetGroup = group.AddComponent<CinemachineTargetGroup>();
                
                // add targetgroup member and apply features
                for(int i=0; i<targetCam.targets.Count(); i++) 
                    targetGroup.AddMember(targetCam.targets[i].transform, 1, 1);

                newTargetCam.m_Follow = group.transform;
                newTargetCam.AddCinemachineComponent<CinemachineFramingTransposer>();
                
                mainCam.m_DefaultBlend.m_Time = targetCam.blendInfo.blendTime;
                mainCam.m_DefaultBlend.m_Style = targetCam.blendInfo.blendIn;

                // hold in Main Camera == coroutine delay time
                StartCoroutine(MoveTopOfCam(temp.GetComponent<CinemachineVirtualCameraBase>(), targetCam.blendInfo.hold));
                break;

            case CameraType.BlendListCamera:
                // add Cinemachine component to new object
                CinemachineBlendListCamera newBlendCam = temp.AddComponent<CinemachineBlendListCamera>(); // new Top Camera Info
                BlendListCameraInfo blendCam = interObj._blendListCam; // Top Camera Info in Inspector
                
                // create List for 'newBlendCam.m_Instructions' -> apply by ToArray()
                List<CinemachineBlendListCamera.Instruction> tempList = new();

                // add member of List(SubCams)
                for(int i=0; i<blendCam.subCams.Count(); i++){
                    // create new sub Camera
                    GameObject newSubCam = new("CrearedSubCam"+i);
                    CinemachineVirtualCamera newSubVirtualCam = newSubCam.AddComponent<CinemachineVirtualCamera>(); // new Sub Camera Info
                    newSubCam.transform.SetParent(newBlendCam.transform);
                    newSubCam.transform.SetAsLastSibling();
                    
                    // create new List member
                    CinemachineBlendListCamera.Instruction instruction = new();
                    
                    switch(blendCam.subCams[i].cameraType){
                        case CameraType.VirtualCamera:
                            newSubVirtualCam.m_Follow = blendCam.subCams[i].virtualCam.target.transform;
                            newSubVirtualCam.m_Lens.OrthographicSize = blendCam.subCams[i].virtualCam.lensOthoSize;
                            newSubVirtualCam.AddCinemachineComponent<CinemachineFramingTransposer>();

                            // set new sub Camera as camera of new List member feature
                            instruction.m_VirtualCamera = newSubVirtualCam;
                            // set feature of new List member
                            if(i != 0){
                                instruction.m_Hold = blendCam.subCams[i].virtualCam.blendInfo.hold;
                                instruction.m_Blend.m_Style = blendCam.subCams[i].virtualCam.blendInfo.blendIn;
                                instruction.m_Blend.m_Time = blendCam.subCams[i].virtualCam.blendInfo.blendTime;
                            }

                            break;
                        case CameraType.TargetGroupCamera:
                            GameObject newtargetGroupObj = new("TargetGroup");
                            CinemachineTargetGroup newTargetGroup = newtargetGroupObj.AddComponent<CinemachineTargetGroup>();

                            for(int k=0; k<blendCam.subCams[i].targetGroupCam.targets.Count(); k++) 
                                newTargetGroup.AddMember(blendCam.subCams[i].targetGroupCam.targets[k].transform, 1, 1);

                            newSubVirtualCam.m_Follow = newtargetGroupObj.transform;
                            newSubVirtualCam.AddCinemachineComponent<CinemachineFramingTransposer>();

                            instruction.m_VirtualCamera = newSubVirtualCam;
                            if(i != 0){
                                instruction.m_Hold = blendCam.subCams[i].virtualCam.blendInfo.hold;
                                instruction.m_Blend.m_Style = blendCam.subCams[i].virtualCam.blendInfo.blendIn;
                                instruction.m_Blend.m_Time = blendCam.subCams[i].virtualCam.blendInfo.blendTime;
                            }
                            
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
        cam.gameObject.SetActive(true);
        cam.MoveToTopOfPrioritySubqueue();
    }
}

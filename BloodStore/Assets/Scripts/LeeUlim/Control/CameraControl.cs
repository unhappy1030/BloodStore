using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class CameraControl : MonoBehaviour
{

    public CinemachineVirtualCamera virtualCamera;
    public CinemachineBlendListCamera blendListCamera;
    public CinemachineTargetGroup targetGroup;
    CinemachineBrain mainCam;

    private void Awake()
    {
        mainCam = Camera.main.GetComponent<CinemachineBrain>();
    }

    public void ChangeCam(InteractObjInfo interObj)
    {
        GameObject temp = new GameObject("CreatedCam");
        temp.transform.SetParent(transform);
        temp.transform.SetAsFirstSibling();

        switch(interObj._cameraType){
            case CameraType.VirtualCamera:
                temp.AddComponent<CinemachineVirtualCamera>();
                CinemachineVirtualCamera newVirtualCam = temp.GetComponent<CinemachineVirtualCamera>();

                VirtualCameraInfo virtualCam = interObj._vertualCam;
                newVirtualCam.m_Priority++; // to avoid showing cam immediately
                newVirtualCam.m_Follow = virtualCam.target.transform;

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

                newTargetCam.m_Priority++;

                TargetGroupCameraInfo targetCam = interObj._targetGroupCam;
                // set targetgroup and apply to m_follow 
                for(int i=0; i<targetCam.targets.Count(); i++) 
                    targetGroup.AddMember(targetCam.targets[i].transform, 1, 1);

                newTargetCam.m_Follow = group.transform;
                
                mainCam.m_DefaultBlend.m_Time = targetCam.blendInfo.blendTime;
                mainCam.m_DefaultBlend.m_Style = targetCam.blendInfo.blendIn;

                StartCoroutine(MoveTopOfCam(temp.GetComponent<CinemachineVirtualCameraBase>(), targetCam.blendInfo.hold));
                break;

            case CameraType.BlendListCamera:
                temp.AddComponent<CinemachineBlendListCamera>();
                CinemachineBlendListCamera newBlendCam = temp.GetComponent<CinemachineBlendListCamera>();

                BlendListCameraInfo blendCam = interObj._blendListCam;

                for(int i=0; i<blendCam.subCams.Count(); i++){
                    switch(blendCam.subCams[i].cameraType){
                        case CameraType.VirtualCamera:
                            GameObject newsubCam = new("CrearedSubCam");
                            newsubCam.AddComponent<CinemachineVirtualCamera>();
                            newsubCam.transform.SetParent(newBlendCam.transform);
                            newsubCam.transform.SetAsLastSibling();
                            CinemachineVirtualCamera subCamScript = newsubCam.GetComponent<CinemachineVirtualCamera>();

                            subCamScript.m_Follow = blendCam.subCams[i].virtualCam.target.transform;

                            newBlendCam.m_Instructions[i].m_VirtualCamera = subCamScript;

                            if(i != 0){
                                newBlendCam.m_Instructions[i-1].m_Hold = blendCam.blendInfo.hold;
                                newBlendCam.m_Instructions[i].m_Blend.m_Style = blendCam.blendInfo.blendIn;
                                newBlendCam.m_Instructions[i].m_Blend.m_Time = blendCam.blendInfo.blendTime;
                            }

                            break;
                        case CameraType.TargetGroupCamera:

                            break;
                    }
                }

                mainCam.m_DefaultBlend.m_Time = blendCam.blendInfo.blendTime;
                mainCam.m_DefaultBlend.m_Style = blendCam.blendInfo.blendIn;

                StartCoroutine(MoveTopOfCam(temp.GetComponent<CinemachineVirtualCameraBase>(), blendCam.blendInfo.hold));
                break;
        }

    }

    IEnumerator MoveTopOfCam(CinemachineVirtualCameraBase cam, float hold){
        yield return new WaitForSecondsRealtime(hold);
        cam.MoveToTopOfPrioritySubqueue();
    }
}

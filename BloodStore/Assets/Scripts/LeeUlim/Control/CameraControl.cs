using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using Unity.VisualScripting;

public class CameraControl : MonoBehaviour
{
    // *** warning : must be in Scene and set "CameraControl" tag
    
    public CinemachineBrain mainCam;

    public List<GameObject> cameraList;

    int camCount = 0;

    private void Awake()
    {
        mainCam = Camera.main.GetComponent<CinemachineBrain>();
        cameraList = new();
    }

    public void ChangeCam(InteractObjInfo interObj)
    {
        // create new object for main Camera and set this as Parent
        GameObject newCamera = new("CreatedCam"+camCount);
        newCamera.SetActive(false);
        newCamera.transform.SetParent(transform);
        newCamera.transform.SetAsFirstSibling();

        switch(interObj._cameraType){
            case CameraType.VirtualCamera:
                VirtualCameraCase(newCamera, interObj);
                break;

            case CameraType.TargetGroupCamera:
                TargetGroupCameraCase(newCamera, interObj);
                break;

            case CameraType.BlendListCamera:
                BlendListCameraCase(newCamera, interObj);
                break;
        }

        cameraList.Add(newCamera);
        if(cameraList.Count >= 3){
            Destroy(cameraList[0]);
            cameraList.RemoveAt(0);
        }
        camCount++;
    }

    // create and active new camera
    void VirtualCameraCase(GameObject newCamera, InteractObjInfo interObj){
        // add Cinemachine component to new Camera
        CinemachineVirtualCamera newVirtualCam = newCamera.AddComponent<CinemachineVirtualCamera>(); // new Camera Info
        // get Information in Inspector for new Camera
        VirtualCameraInfo virtualCamInfo = interObj._virtualCam;
        
        newVirtualCam.m_Follow = virtualCamInfo.targets[0].transform;
        newVirtualCam.m_Lens.OrthographicSize = virtualCamInfo.lensOthoSize;
        newVirtualCam.AddCinemachineComponent<CinemachineFramingTransposer>();
        
        if(virtualCamInfo.doseUseBound && virtualCamInfo.bound != null){
            CinemachineConfiner2D bound = newCamera.AddComponent<CinemachineConfiner2D>();
            bound.m_BoundingShape2D = virtualCamInfo.bound;
        }
        
        mainCam.m_DefaultBlend.m_Time = virtualCamInfo.blendInfo.blendTime;
        mainCam.m_DefaultBlend.m_Style = virtualCamInfo.blendInfo.blendIn;

        // hold in Main Camera == coroutine delay time
        StartCoroutine(MoveTopOfCam(newCamera.GetComponent<CinemachineVirtualCameraBase>(), virtualCamInfo.blendInfo.hold));
    }

    void TargetGroupCameraCase(GameObject newCamera, InteractObjInfo interObj){
        // add Cinemachine component to new Camera
        CinemachineVirtualCamera newTargetCam = newCamera.AddComponent<CinemachineVirtualCamera>(); // new Camera Info
        // get Information in Inspector for new Camera
        VirtualCameraInfo targetCamInfo = interObj._virtualCam;
        
        // create Target Group for Camera
        GameObject group = new("TargetGroup");
        group.transform.SetParent(newCamera.transform);
        CinemachineTargetGroup targetGroup = group.AddComponent<CinemachineTargetGroup>();
        
        int nullCount = 0;
        // add targetgroup member and apply features
        for(int i=0; i<targetCamInfo.targets.Count; i++){
            if(targetCamInfo.targets[i] != null)
                targetGroup.AddMember(targetCamInfo.targets[i].transform, 1, 1.5f);
            else
                nullCount++;
        }

        if(nullCount == targetCamInfo.targets.Count){
            Debug.Log("It is not avaiable target...");
            return;
        }

        // set group as Follow
        newTargetCam.m_Follow = group.transform;
        newTargetCam.AddCinemachineComponent<CinemachineFramingTransposer>();
        
        if(targetCamInfo.doseUseBound && targetCamInfo.bound != null){
            CinemachineConfiner2D bound = newCamera.AddComponent<CinemachineConfiner2D>();
            bound.m_BoundingShape2D = targetCamInfo.bound;
        }
        
        mainCam.m_DefaultBlend.m_Time = targetCamInfo.blendInfo.blendTime;
        mainCam.m_DefaultBlend.m_Style = targetCamInfo.blendInfo.blendIn;

        // hold in Main Camera == coroutine delay time
        StartCoroutine(MoveTopOfCam(newCamera.GetComponent<CinemachineVirtualCameraBase>(), targetCamInfo.blendInfo.hold));
    }

    void BlendListCameraCase(GameObject newCamera, InteractObjInfo interObj){
        // add Cinemachine component to new Camera
        CinemachineBlendListCamera newBlendCam = newCamera.AddComponent<CinemachineBlendListCamera>(); // new Top Camera Info
        // get Information in Inspector for new Camera
        BlendListCameraInfo blendCamInfo = interObj._blendListCam;
        
        // create List for 'newBlendCam.m_Instructions' -> apply by ToArray()
        List<CinemachineBlendListCamera.Instruction> tempList = new();

        // add member of List(SubCams)
        for(int i=0; i<blendCamInfo.subCams.Count; i++){
            Debug.Log("Sub Cam Cahnging...");
            // get  Information for sub Camera and instruction
            BlendListSubCameraInfo subCamInfo = blendCamInfo.subCams[i];

            // create new instruction
            CinemachineBlendListCamera.Instruction instruction = new();
            
            // create new sub Camera for instructions.m_VirtualCamera
            GameObject newSubCam = new("CreatedSubCam"+i);
            newSubCam.transform.SetParent(newBlendCam.transform);
            newSubCam.transform.SetAsLastSibling();
            // add Cinemachine component to new Sub Camera
            CinemachineVirtualCamera newSubVirtualCam = newSubCam.AddComponent<CinemachineVirtualCamera>();
            
            // set sub Camera Information
            switch(subCamInfo.cameraType){
                case CameraType.VirtualCamera:
                    newSubVirtualCam.m_Follow = subCamInfo.virtualCam.targets[0].transform;
                    newSubVirtualCam.m_Lens.OrthographicSize = subCamInfo.virtualCam.lensOthoSize;
                    break;

                case CameraType.TargetGroupCamera:
                    GameObject newtargetGroupObj = new("TargetGroup");
                    newtargetGroupObj.transform.SetParent(newSubCam.transform);
                    CinemachineTargetGroup newTargetGroup = newtargetGroupObj.AddComponent<CinemachineTargetGroup>();

                    for(int k=0; k<subCamInfo.virtualCam.targets.Count(); k++) 
                        newTargetGroup.AddMember(subCamInfo.virtualCam.targets[k].transform, 1, 1);

                    newSubVirtualCam.m_Follow = newtargetGroupObj.transform;
                    break;
            }

            newSubVirtualCam.AddCinemachineComponent<CinemachineFramingTransposer>();

            // set instruction features
            instruction.m_VirtualCamera = newSubVirtualCam;
            instruction.m_Hold = subCamInfo.virtualCam.blendInfo.hold;
            
            if(i != 0){
                instruction.m_Blend.m_Style = subCamInfo.virtualCam.blendInfo.blendIn;
                instruction.m_Blend.m_Time = subCamInfo.virtualCam.blendInfo.blendTime;
            }
        
            tempList.Add(instruction);
        }

        newBlendCam.m_Instructions = tempList.ToArray();
        
        if(blendCamInfo.doseUseBound && blendCamInfo.bound != null){
            CinemachineConfiner2D bound = newCamera.AddComponent<CinemachineConfiner2D>();
            bound.m_BoundingShape2D = blendCamInfo.bound;
        }

        mainCam.m_DefaultBlend.m_Time = blendCamInfo.blendInfo.blendTime;
        mainCam.m_DefaultBlend.m_Style = blendCamInfo.blendInfo.blendIn;

        StartCoroutine(MoveTopOfCam(newCamera.GetComponent<CinemachineVirtualCameraBase>(), blendCamInfo.blendInfo.hold));
    }

    IEnumerator MoveTopOfCam(CinemachineVirtualCameraBase cam, float hold){
        yield return new WaitForSecondsRealtime(hold);
        Debug.Log("Waiting...");
        cam.gameObject.SetActive(true);
        cam.MoveToTopOfPrioritySubqueue();
    }
}

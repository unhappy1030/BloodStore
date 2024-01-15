using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseRayCast : MonoBehaviour
{
    public CameraControl cameraControl; // *** warning : must be in Scene and set "CameraControl" tag
    public NPCInteract npcInteract;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

            if (ray.collider != null)
                MouseInteract(ray.collider.gameObject);
        }
    }

    void MouseInteract(GameObject interactObj)
    {
        Debug.Log(interactObj + (interactObj == null).ToString());
        InteractObjInfo interactObjInfo = interactObj.GetComponent<InteractObjInfo>();
        SetCameraTarget setCameraTarget = interactObj.GetComponent<SetCameraTarget>();
        bool isAvailableCameraMove = true;

        if (interactObjInfo == null)
            return;

        if (interactObjInfo._interactType == InteractType.CameraControl)
        {
            // only Node case -> isAvailableCameraMove changes false at here
            if(setCameraTarget != null)
                isAvailableCameraMove = setCameraTarget.AssignCameraTarget(interactObjInfo);

            if(isAvailableCameraMove){
                Debug.Log("isAvailableCameraMove : " + isAvailableCameraMove);
                
                Debug.Log(cameraControl + (cameraControl == null).ToString());
                Debug.Log(interactObjInfo + (interactObjInfo == null).ToString());
                cameraControl.ChangeCam(interactObjInfo);
            }
            else
                Debug.Log("It is not available form of targets...");
        }

        if(interactObjInfo._interactType == InteractType.NpcInteraction)
        {
            npcInteract.StartDialogue(interactObjInfo);
        }

        if (interactObjInfo._interactType == InteractType.SceneLoad)
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(interactObjInfo._sceneName, 0.05f));
        }
    }

}

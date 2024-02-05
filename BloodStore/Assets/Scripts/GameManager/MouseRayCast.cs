using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class MouseRayCast : MonoBehaviour
{
    // warning : object must be set Layer as "Interact"
    
    public CameraControl cameraControl; // *** warning : must be in Scene and set "CameraControl" tag
    public NPCInteract npcInteract;
    public NodeInteraction nodeInteraction;
    public DialogueRunner dialogueRunner;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) 
            && !EventSystem.current.IsPointerOverGameObject() 
            && !dialogueRunner.IsDialogueRunning
            && !GameManager.Instance.isFading
            && !cameraControl.mainCam.IsBlending
            && !UIControl.isPause)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.zero, 0f, LayerMask.GetMask("Interact"));

            if (ray.collider != null){
                MouseInteract(ray.collider.gameObject);
            }
        }
    }

    void MouseInteract(GameObject interactObj)
    {
        InteractObjInfo interactObjInfo = interactObj.GetComponent<InteractObjInfo>();

        if (interactObjInfo == null)
            return;

        if (interactObjInfo._interactType == InteractType.CameraControl)
        {
            cameraControl.ChangeCam(interactObjInfo);
        }

        if(interactObjInfo._interactType == InteractType.StartDialogue)
        {
            npcInteract.StartDialogue(interactObjInfo._nodeName);
        }

        if(interactObjInfo._interactType == InteractType.FamilyTree)
        {
            nodeInteraction.MouseInteract(interactObjInfo);
        }


        if(interactObjInfo._interactType == InteractType.UIOnOff)
        {
            Debug.Log("UI ON");
            GameObject ui = interactObjInfo._ui;
            ui.SetActive(true);
        }


        if (interactObjInfo._interactType == InteractType.SceneLoad)
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(interactObjInfo._sceneName, 0.05f));
        }
    }

}

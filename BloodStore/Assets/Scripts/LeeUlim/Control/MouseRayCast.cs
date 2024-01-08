using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseRayCast : MonoBehaviour
{
    public GameObject moveTarget;
    CameraControl cameraControl; // *** warning : must be in Scene and set "CameraControl" tag
    YarnControl yarnControl;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // cameraControl allign
        cameraControl = GameManager.Instance.cameraControl;
        // yarnControl allign
        yarnControl = GameManager.Instance.yarnControl;
        
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


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

    void MouseInteract(GameObject gameObject)
    {
        InteractObjInfo interactObjInfo = gameObject.GetComponent<InteractObjInfo>();

        if (interactObjInfo == null)
            return;

        if (interactObjInfo._interactType == InteractType.CameraControl){
            cameraControl.ChangeCam(interactObjInfo);
        }

        if(interactObjInfo._interactType == InteractType.NpcInteraction){
            yarnControl.StartDialogue(interactObjInfo);
        }

        if (interactObjInfo._interactType == InteractType.SceneLoad){
            GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(interactObjInfo._sceneName, 0.05f));
        }
    }

}

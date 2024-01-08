using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseRayCast : MonoBehaviour
{
    public GameObject moveTarget;
    CameraControl cameraControl; // *** warning : must be in Scene and set "CameraControl" tag

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // find CameraContorl object
        GameObject cameraControlObj = GameObject.FindWithTag("CameraControl");
        if(cameraControlObj != null)
            cameraControl = cameraControlObj.GetComponent<CameraControl>();
        
        else
        {
            cameraControlObj = new("CameraContorl");
            cameraControl = cameraControlObj.AddComponent<CameraControl>();
        }
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

        if (interactObjInfo._interactType == InteractType.CameraControl)
        {
            cameraControl.ChangeCam(interactObjInfo);
        }

        if (interactObjInfo._interactType == InteractType.SceneLoad)
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(interactObjInfo._sceneName, 0.05f));
        }
    }

}

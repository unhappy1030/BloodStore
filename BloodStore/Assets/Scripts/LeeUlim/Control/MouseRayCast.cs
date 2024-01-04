using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseRayCast : MonoBehaviour
{
    // 주의 : 버튼을 제외한 모든 상호작용 물체는 Collider를 가지고 있어야 한다
    public GameObject moveTarget;
    CameraControl cameraControl;

    private void Start()
    {
        // *** 카메라는 MainCamera 태그를 가지고 있어야 함
        cameraControl = GameObject.FindWithTag("CameraControl").GetComponent<CameraControl>();
        
    }

    private void Update()
    {
        // 마우스 입력 처리
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

            if (ray.collider != null)
                MouseInteract(ray.collider.gameObject);
        }
    }

    // [마우스 상호작용 처리]
    void MouseInteract(GameObject gameObject)
    {
        InteractObjInfo interactObjInfo = gameObject.GetComponent<InteractObjInfo>();

        if (interactObjInfo == null)
            return;

        //  - 카메라 이동
        if (interactObjInfo._interactType == InteractType.CameraControl)
        {
            cameraControl.ChangeBlendListCamSetting(interactObjInfo);
        }

        // - 씬 이동
        if (interactObjInfo._interactType == InteractType.SceneLoad)
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(interactObjInfo._sceneName, 0.05f));
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseRayCast : MonoBehaviour
{
    // ���� : ��ư�� ������ ��� ��ȣ�ۿ� ��ü�� Collider�� ������ �־�� �Ѵ�
    public GameObject moveTarget;
    CameraControl cameraControl;

    private void Start()
    {
        // *** ī�޶�� MainCamera �±׸� ������ �־�� ��
        cameraControl = GameObject.FindWithTag("CameraControl").GetComponent<CameraControl>();
        
    }

    private void Update()
    {
        // ���콺 �Է� ó��
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

            if (ray.collider != null)
                MouseInteract(ray.collider.gameObject);
        }
    }

    // [���콺 ��ȣ�ۿ� ó��]
    void MouseInteract(GameObject gameObject)
    {
        InteractObjInfo interactObjInfo = gameObject.GetComponent<InteractObjInfo>();

        if (interactObjInfo == null)
            return;

        //  - ī�޶� �̵�
        if (interactObjInfo._interactType == InteractType.CameraControl)
        {
            cameraControl.ChangeCam(interactObjInfo);
        }

        // - �� �̵�
        if (interactObjInfo._interactType == InteractType.SceneLoad)
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(interactObjInfo._sceneName, 0.05f));
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    public GameObject treeManager;
    TreeManager tree;
    void Start()
    {
        transform.position = new Vector2(0, 0);
        tree = treeManager.GetComponent<TreeManager>();
        PolygonCollider2D polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        Group rootGroup = tree.mainGroup.transform.GetChild(0).gameObject.GetComponent<Group>();

        // 박스의 꼭지점 좌표 배열
        Vector2[] boxVertices = rootGroup.GetCameraColliderPos();

        // PolygonCollider2D에 박스의 꼭지점 좌표 설정
        polygonCollider.SetPath(0, boxVertices);
    }
}

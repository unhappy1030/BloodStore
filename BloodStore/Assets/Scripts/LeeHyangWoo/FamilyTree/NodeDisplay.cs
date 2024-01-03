using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeDisplay : MonoBehaviour
{
    public GameObject moveCamera;
    public PairSO pairData;
    public GameObject nodePrefab;
    public GameObject EmptyPrefab;
    private Pair pair;
    private PosTree posTree;
    public TextMeshPro nameLabel;
    public TextMeshPro sexLabel;
    public TextMeshPro bloodTypeLabel;
    public TextMeshPro hpLabel;
    public TextMeshPro ageLabel;
    void Update()
    {
        // 마우스 왼쪽 버튼이 클릭되었을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치를 2D 좌표로 변환
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 마우스 위치에 collider가 있는지 확인
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            // collider가 있다면 클릭된 것으로 간주
            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                if(pair.parent != null){
                    pair.parent.DestroyDP(pair);
                    pair.SetDataView(nodePrefab,EmptyPrefab);
                    pair.SetChildrenView();
                }
            }
        }
    }
    public void SetNodeData(Pair pair, Node node)
    {
        this.pair = pair;
        nameLabel.text = "Name: " + node.name;
        sexLabel.text = "Sex: " + node.sex;
        bloodTypeLabel.text = "Blood Type: " + node.bloodType[0];
        hpLabel.text = "HP: " + node.hp.ToString();
        ageLabel.text = "Age: " + node.age.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmptyDisplay : MonoBehaviour
{
    public NodeSO nodeSO;
    Group group;
    void Start(){
        group = gameObject.transform.parent.GetComponent<Group>();
    }
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
                if(group.pair.BlankNodeCheck() == nodeSO.node.sex){
                    MakePair();
                    group.pair.AddChild();
                    ChangeDisplay(nodeSO.node.sex);
                }
                else{
                    DeleteNode();
                }

            }
        }
    }
    public void MakeBoxCollider(){
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        box.size = new Vector2(1, 1);
    }
    public void ActiveCollider(){
        BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
        if(box != null){
            box.enabled = true;
        }
    }
    public void DeActiveCollider(){
        BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
        if(box != null){
            box.enabled = false;
        }
    }
    void ChangeDisplay(string sex){
        if(sex == "Male"){
            group.leftDisplay = group.CreateNode(group.pair.male);
            group.leftDisplay.transform.parent = group.transform;
            group.leftDisplay.transform.position = this.transform.position;
        }
        else{
            group.rightDisplay = group.CreateNode(group.pair.female);
            group.rightDisplay.transform.parent = group.transform;
            group.rightDisplay.transform.position = this.transform.position;
        }
        Destroy(gameObject);
    }
    void MakePair(){
        Node node = new Node();
        node = nodeSO.node;
        group.pair.MakePair(node);
        group.pair.isPair = true;
    }
    void DeleteNode(){
        group.pair.isPair = false;
    }
}

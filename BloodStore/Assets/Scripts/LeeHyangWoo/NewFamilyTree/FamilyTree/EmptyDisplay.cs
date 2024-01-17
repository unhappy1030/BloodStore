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

    public void SetNode(){
        if(group.pair.BlankNodeCheck() == nodeSO.node.sex){
            MakePair();
            group.pair.AddChild();
            ChangeDisplay(nodeSO.node.sex);
        }
        else{
            DeleteNode();
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

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
        if(!group.pairTree.pair.male.empty && !group.pairTree.pair.male.isDead){

        }
        else if(!group.pairTree.pair.female.empty && !group.pairTree.pair.female.isDead){
            if(group.pairTree.BlankNodeCheck() == nodeSO.node.sex){
                MakePair();
                group.button.SetActive(true);
                ChangeDisplay(nodeSO.node.sex);
            }
        }
        else{
            DeleteNode();
        }
    }

    public void MakeBoxCollider(){
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        box.size = spriteSize;
    }
    void ChangeDisplay(string sex){
        if(sex == "Male"){
            group.leftDisplay = group.CreateNode(group.pairTree.pair.male);
            group.leftDisplay.transform.parent = group.transform;
            group.leftDisplay.transform.position = this.transform.position;
        }
        else{
            group.rightDisplay = group.CreateNode(group.pairTree.pair.female);
            group.rightDisplay.transform.parent = group.transform;
            group.rightDisplay.transform.position = this.transform.position;
        }
        Destroy(gameObject);
    }
    void MakePair(){
        Node node = new Node();
        node = nodeSO.node;
        group.pairTree.MakePair(node);
        group.pairTree.pair.isPair = true;
    }
    void DeleteNode(){
        group.pairTree.pair.isPair = false;
    }
}

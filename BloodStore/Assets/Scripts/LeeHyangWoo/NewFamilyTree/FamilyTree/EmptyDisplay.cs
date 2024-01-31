using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyDisplay : MonoBehaviour
{
    public NodeSO nodeSO;
    Group group;
    void Start(){
        group = gameObject.transform.parent.GetComponent<Group>();
    }

    public void SetNode(){
        if(!group.pairTree.pair.male.empty && !group.pairTree.pair.male.isDead && group.pairTree.pair.male.age >= 20){
            if(group.pairTree.BlankNodeCheck() == nodeSO.node.sex && !nodeSO.node.empty){
                MakePair();
                if(group.pairTree.pair.male.age < 60){
                    group.button.SetActive(true);
                }
                group.selectedCard.SetActive(false);
                ChangeDisplay(nodeSO.node.sex);
            }
        }
        else if(!group.pairTree.pair.female.empty && !group.pairTree.pair.female.isDead && group.pairTree.pair.female.age >= 20){
            if(group.pairTree.BlankNodeCheck() == nodeSO.node.sex && !nodeSO.node.empty){
                MakePair();
                if(group.pairTree.pair.female.age < 60){
                    group.button.SetActive(true);
                }
                group.selectedCard.SetActive(false);
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
        Node node = new Node{
            name = nodeSO.node.name,
            sex = nodeSO.node.sex,
            bloodType = nodeSO.node.bloodType,
            hp = nodeSO.node.hp,
            age = nodeSO.node.age,
            isDead = nodeSO.node.isDead,
            empty = false
        };
        group.pairTree.MakePair(node);
        group.pairTree.pair.isPair = true;
        nodeSO.node.empty = true;
    }
    void DeleteNode(){
        group.pairTree.pair.isPair = false;
    }
}

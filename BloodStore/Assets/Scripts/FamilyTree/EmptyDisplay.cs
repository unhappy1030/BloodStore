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

    public void SetNode(GameObject checkPairUI, GameObject noticeUI){
        if(!group.pairTree.pair.male.empty && !group.pairTree.pair.male.isDead && group.pairTree.pair.male.age >= 20){
            if(group.pairTree.BlankNodeCheck() == nodeSO.node.sex && !nodeSO.node.empty){
                CheckPair checkPair = checkPairUI.GetComponent<CheckPair>();
                checkPair.ConfirmCheck(this);
                return ;
            }
        }
        else if(!group.pairTree.pair.female.empty && !group.pairTree.pair.female.isDead && group.pairTree.pair.female.age >= 20){
            if(group.pairTree.BlankNodeCheck() == nodeSO.node.sex && !nodeSO.node.empty){
                CheckPair checkPair = checkPairUI.GetComponent<CheckPair>();
                checkPair.ConfirmCheck(this);
                return ;
            }
        }
        noticeUI.SetActive(true);
        DeleteNode();
    }
    public void ChangeConfirmed(){
        MakePair();
        if(group.pairTree.pair.male.age < 60){
            group.childButtonOn.SetActive(true);
        }
        group.selectedCard.SetActive(false);
        ChangeDisplay(nodeSO.node.sex);
    }
    public void MakeBoxCollider(){
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        box.size = spriteSize;
    }
    void ChangeDisplay(string sex){
        if(sex == "Male"){
            group.leftNode = group.CreateNode(group.pairTree.pair.male);
            group.leftNode.transform.parent = group.transform;
            group.leftNode.transform.position = this.transform.position;
            group.leftNode.GetComponent<BoxCollider2D>().enabled = true;
            group.leftNode.transform.SetAsFirstSibling();
        }
        else{
            group.rightNode = group.CreateNode(group.pairTree.pair.female);
            group.rightNode.transform.parent = group.transform;
            group.rightNode.transform.position = this.transform.position;
            group.rightNode.GetComponent<BoxCollider2D>().enabled = true;
            group.rightNode.transform.SetAsFirstSibling();
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
            empty = false,
            imageIdx = nodeSO.node.imageIdx,
        };
        group.pairTree.MakePair(node);
        group.pairTree.pair.isPair = true;
        nodeSO.node.empty = true;
    }
    void DeleteNode(){
        group.pairTree.pair.isPair = false;
    }
}

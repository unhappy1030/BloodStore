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
        if(!group.treePair.pair.male.empty && !group.treePair.pair.male.isDead && group.treePair.pair.male.age >= 20){
            if(group.treePair.BlankNodeCheck() == nodeSO.node.sex && !nodeSO.node.empty){
                CheckPair checkPair = checkPairUI.GetComponent<CheckPair>();
                checkPair.ConfirmCheck(this);
                return ;
            }
        }
        else if(!group.treePair.pair.female.empty && !group.treePair.pair.female.isDead && group.treePair.pair.female.age >= 20){
            if(group.treePair.BlankNodeCheck() == nodeSO.node.sex && !nodeSO.node.empty){
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
        if(group.treePair.pair.male.age < 60){
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
            group.leftNode = group.CreateNode(group.treePair.pair.male);
            group.leftNode.transform.parent = group.transform;
            group.leftNode.transform.position = this.transform.position;
            group.leftNode.GetComponent<BoxCollider2D>().enabled = true;
            group.leftNode.transform.SetAsFirstSibling();
        }
        else{
            group.rightNode = group.CreateNode(group.treePair.pair.female);
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
            mentalScore = nodeSO.node.mentalScore,
            isDead = nodeSO.node.isDead,
            empty = false,
            imageIdx = nodeSO.node.imageIdx,
        };
        group.treePair.MakePair(node);
        group.treePair.pair.isPair = true;
        nodeSO.node.empty = true;
    }
    void DeleteNode(){
        group.treePair.pair.isPair = false;
    }
}

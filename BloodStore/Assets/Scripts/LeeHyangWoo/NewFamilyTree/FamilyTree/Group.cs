using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Yarn;
using System.Linq;
using Unity.VisualScripting;
using System;
public class Group : MonoBehaviour
{
    public PairTree pairTree;
    public GameObject nodePrefab;//프리펩 전달 안받게 변경
    public GameObject emptyPrefab;
    public GameObject deadPrefab;
    public Vector2 groupPos;
    public Vector2 leftPos;
    public Vector2 rightPos;
    public GameObject leftDisplay;
    public GameObject rightDisplay;
    public GameObject button;
    public GameObject buttonOff;
    private float pairOffSet;
    private float halfX, halfY;
    private float pairSize, unit;
    public float offSetX, offSetY;
    public Group parentGroup;
    public List<Group> childrenGroup;

    public float lineWidth = 0.05f;
    public void SetPrefab(GameObject nodePrefab, GameObject emptyPrefab, GameObject deadPrefab){
        this.nodePrefab = nodePrefab;
        this.emptyPrefab = emptyPrefab;
        this.deadPrefab = deadPrefab;
    }
    public void SetUI(GameObject selectedCard){
        this.selectedCard = selectedCard;
    }
    public void SetSizeData(float halfX, float halfY, float pairSize, float unit, float pairOffSet, float offSetX, float offSetY){
        this.halfX = halfX;
        this.halfY = halfY;
        this.pairSize = pairSize;
        this.unit = unit;
        this.pairOffSet = pairOffSet;
        this.offSetX = offSetX;
        this.offSetY = offSetY;
    }
    public Group DisplayNodes(){
        leftDisplay = CreateNode(pairTree.pair.male);
        leftDisplay.transform.position = leftPos;
        rightDisplay = CreateNode(pairTree.pair.female);
        rightDisplay.transform.position = rightPos;
        return this;
    }

    public GameObject CreateNode(Node node){
        GameObject display;
        if(!node.empty){
            if(node.isDead){
                display = Instantiate(deadPrefab, new Vector2(0, 0), Quaternion.identity);
                InteractObjInfo inter = display.AddComponent<InteractObjInfo>();
                inter._interactType = InteractType.FamilyTree;
                inter._familyTreeType = FamilyTreeType.Node;
                NodeDisplay nodeDisplay = display.GetComponent<NodeDisplay>();
                nodeDisplay.SetDeadData(node);
                nodeDisplay.MakeBoxCollider();
                BoxCollider2D box = nodeDisplay.gameObject.GetComponent<BoxCollider2D>();
                box.enabled = false;
            }
            else{
                display = Instantiate(nodePrefab, new Vector2(0, 0), Quaternion.identity);
                InteractObjInfo inter = display.AddComponent<InteractObjInfo>();
                inter._interactType = InteractType.FamilyTree;
                inter._familyTreeType = FamilyTreeType.Node;
                NodeDisplay nodeDisplay = display.GetComponent<NodeDisplay>();
                nodeDisplay.SetNodeData(node);
                nodeDisplay.MakeBoxCollider();
                BoxCollider2D box = nodeDisplay.gameObject.GetComponent<BoxCollider2D>();
                box.enabled = false;
            }
            
        }
        else{
            display = Instantiate(emptyPrefab, new Vector2(0, 0), Quaternion.identity);
            InteractObjInfo inter = display.AddComponent<InteractObjInfo>();
            inter._interactType = InteractType.FamilyTree;
            inter._familyTreeType = FamilyTreeType.EmptyNode;
            EmptyDisplay emptyDisplay = display.GetComponent<EmptyDisplay>();
            emptyDisplay.MakeBoxCollider();
            BoxCollider2D box = emptyDisplay.gameObject.GetComponent<BoxCollider2D>();
            box.enabled = false;
        }
        return display;
    }
    public void MakeBoxCollider(){
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        box.size = new Vector2(pairSize, halfY * 2);
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
    public void CameraSetting(){
        InteractObjInfo inter = gameObject.AddComponent<InteractObjInfo>();
        inter.SetTargetCameraInfo(SendFamilyList(), 0.25f, CinemachineBlendDefinition.Style.EaseInOut, 0.5f);
    }

    public List<GameObject> SendFamilyList(){
        List<GameObject> familyList = new List<GameObject>();
        familyList.Add(GetGameObject());
        if(childrenGroup != null){
            foreach(Group group in childrenGroup){
                familyList.Add(group.GetGameObject());
            }
        }

        return familyList;
    }
    public GameObject GetGameObject(){
        return gameObject;
    }

    public void PairLine(){
        GameObject pairLine = new("PairLine");
        LineRenderer line = pairLine.AddComponent<LineRenderer>();
        Vector2 globalPos = transform.TransformPoint(gameObject.transform.parent.transform.position);
        pairLine.transform.position = new Vector3(globalPos.x, globalPos.y, 0);
        line.widthMultiplier = lineWidth;
        line.material.color = Color.black;
        Vector3[] points = new Vector3[2];
        points[0] = new Vector3( globalPos.x - pairOffSet / 2, globalPos.y, 0);
        points[1] = new Vector3( globalPos.x + pairOffSet / 2, globalPos.y, 0);
        line.positionCount = points.Count();
        line.SetPositions(points);
        // Debug.Log(" group pos : " + groupPos.ToString());
        // Debug.Log(" line pos : " + globalPos.ToString());
        pairLine.transform.parent = gameObject.transform;
    }
    public void FamilyLine(){
        if(childrenGroup != null){
            foreach(Group group in childrenGroup){
                GameObject pairLine = new("Line");
                LineRenderer line = pairLine.AddComponent<LineRenderer>();
                Vector2 globalPos = transform.TransformPoint(gameObject.transform.parent.transform.position);
                Vector2 globalChildPos = group.transform.position;
                pairLine.transform.position = new Vector3(globalChildPos.x, globalChildPos.y, 0);
                line.widthMultiplier = lineWidth;
                line.material.color = Color.black;
                Vector3[] points = new Vector3[4];
                points[0] = new Vector3( globalPos.x, globalPos.y, 0);
                points[1] = new Vector3( globalPos.x, globalPos.y - halfY - offSetY / 2, 0);
                points[2] = new Vector3( globalChildPos.x, globalChildPos.y + halfY + offSetY / 2, 0);
                points[3] = new Vector3( globalChildPos.x , globalChildPos.y, 0);
                line.positionCount = points.Count();
                line.SetPositions(points);
                // Debug.Log("child pos : " + group.transform.position.ToString());
                // Debug.Log("globalchild pos : " + globalChildPos.ToString());
                // Debug.Log(" line pos : " + globalPos.ToString());
                pairLine.transform.parent = gameObject.transform;
            }
        }
    }

    public void MakeChildButton(GameObject childButtonPrefab, GameObject childButtonOffPrefab){
        button =  Instantiate(childButtonPrefab, groupPos, Quaternion.identity);
        InteractObjInfo inter = button.AddComponent<InteractObjInfo>();
        inter._interactType = InteractType.FamilyTree;
        inter._familyTreeType = FamilyTreeType.ChildButton;
        buttonOff = Instantiate(childButtonOffPrefab, groupPos, Quaternion.identity);
        button.SetActive(false);
        buttonOff.SetActive(false);
        ChildButton childButton = button.AddComponent<ChildButton>();
        childButton.group = this;
        BoxCollider2D box = button.AddComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = button.GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        box.size = spriteSize;
        button.transform.parent = transform;
        buttonOff.transform.parent = transform;
        if(pairTree.pair.isPair && pairTree.pair.childNum == 0){
            button.SetActive(true);
        }
    }
    public void ChangeButton(){
        button.SetActive(false);
        buttonOff.SetActive(true);
    }
}
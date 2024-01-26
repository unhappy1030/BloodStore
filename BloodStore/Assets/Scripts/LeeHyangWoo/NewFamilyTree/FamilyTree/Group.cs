using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Yarn;
using System.Linq;
using Unity.VisualScripting;
public class Group : MonoBehaviour
{
    public PairTree pairTree;
    public GameObject addChildUI;
    public GameObject nodePrefab;
    public GameObject emptyPrefab;
    public GameObject childButtonPrefab;
    public GameObject childButtonOffPrefab;
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
    public void SetPrefab(GameObject nodePrefab, GameObject emptyPrefab, GameObject childButtonPrefab, GameObject childButtonOffPrefab, GameObject addChildUI){
        this.nodePrefab = nodePrefab;
        this.emptyPrefab = emptyPrefab;
        this.childButtonPrefab = childButtonPrefab;
        this.childButtonOffPrefab = childButtonOffPrefab;
        this.addChildUI = addChildUI;
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
            display = Instantiate(nodePrefab, new Vector2(0, 0), Quaternion.identity);
            NodeDisplay nodeDisplay = display.GetComponent<NodeDisplay>();
            nodeDisplay.SetNodeData(node);
            nodeDisplay.MakeBoxCollider();
            nodeDisplay.DeActiveCollider();
        }
        else{
            display = Instantiate(emptyPrefab, new Vector2(0, 0), Quaternion.identity);
            EmptyDisplay emptyDisplay = display.GetComponent<EmptyDisplay>();
            emptyDisplay.MakeBoxCollider();
            emptyDisplay.DeActiveCollider();
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

    public void MakeChildButton(){
        if(pairTree.pair.isPair && pairTree.pair.childNum == 0){
            button =  Instantiate(childButtonPrefab, groupPos, Quaternion.identity);
            buttonOff = Instantiate(childButtonOffPrefab, groupPos, Quaternion.identity);
            buttonOff.SetActive(false);
            ChildButton childButton = button.AddComponent<ChildButton>();
            childButton.addChildUI = addChildUI;
            childButton.group = this;
            BoxCollider2D box = button.AddComponent<BoxCollider2D>();
            SpriteRenderer spriteRenderer = button.GetComponent<SpriteRenderer>();
            Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
            box.size = spriteSize;
            button.transform.parent = transform;
            buttonOff.transform.parent = transform;
        }
    }
    public void ChangeButton(){
        button.SetActive(false);
        buttonOff.SetActive(true);
    }
}
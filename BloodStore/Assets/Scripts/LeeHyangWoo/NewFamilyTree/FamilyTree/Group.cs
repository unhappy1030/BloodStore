using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Group : MonoBehaviour
{
    public Pair pair;
    public GameObject nodePrefab;
    public GameObject emptyPrefab;
    public Vector2 groupPos;
    public Vector2 leftPos;
    public Vector2 rightPos;

    public GameObject leftDisplay;
    public GameObject rightDisplay;
    private float halfX, halfY;
    private float pairSize, unit;
    public Group parentGroup;
    public List<Group> childrenGroup;

    public void SetPrefab(GameObject nodePrefab, GameObject emptyPrefab){
        this.nodePrefab = nodePrefab;
        this.emptyPrefab = emptyPrefab;
    }
    public void SetSizeData(float halfX, float halfY, float pairSize, float unit){
        this.halfX = halfX;
        this.halfY = halfY;
        this.pairSize = pairSize;
        this.unit = unit;
    }
    public Group DisplayNodes(){
        leftDisplay = CreateNode(pair.male);
        leftDisplay.transform.position = leftPos;
        rightDisplay = CreateNode(pair.female);
        rightDisplay.transform.position = rightPos;
        return this;
    }

    public GameObject CreateNode(Node node){
        GameObject display;
        if(!node.empty){
            display = Instantiate(nodePrefab, new Vector2(0, 0), Quaternion.identity);
            NodeDisplay nodeDisplay = display.GetComponent<NodeDisplay>();
            nodeDisplay.SetNodeData(node);
        }
        else{
            display = Instantiate(emptyPrefab, new Vector2(0, 0), Quaternion.identity);
        }
        return display;
    }

    public void MakeBoxCollider(){
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        box.size = new Vector2(pairSize, halfY * 2);
    }
    public void CameraSetting(){
        SetCameraTarget camera = gameObject.AddComponent<SetCameraTarget>();
        InteractObjInfo inter = gameObject.AddComponent<InteractObjInfo>();
        inter.SetBlendData();
        camera.SetTarget(SendFamilyList());
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
}

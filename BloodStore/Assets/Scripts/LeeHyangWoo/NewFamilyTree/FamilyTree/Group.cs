using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Group parentGroup;
    public List<Group> childrenGroup;

    public void SetPrefab(GameObject nodePrefab, GameObject emptyPrefab){
        this.nodePrefab = nodePrefab;
        this.emptyPrefab = emptyPrefab;
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
}

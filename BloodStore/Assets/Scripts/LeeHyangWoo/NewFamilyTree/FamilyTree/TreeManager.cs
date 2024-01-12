using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

public class TreeManagerTest : MonoBehaviour
{
    public PairSO pairSO;
    public GameObject nodePrefab;
    public GameObject emptyPrefab;
    public float pairOffSet = 0.2f;
    public float offSetX, offSetY;
    private float halfX, halfY;
    private float pairSize, unit;
    private float lastX, lastY;
    void Start()
    {
        lastX = 0f;
        lastY = 0f;
        if (pairSO.root.Count == 0)
        {
            Node node = new Node();
            node.SetAllRandom();
            AddFirstNode(node);
        }
        SetPrefab();
        MakeFamilyTree();
    }

    void SetPrefab(){
        halfX = nodePrefab.GetComponent<SpriteRenderer>().bounds.extents.x;
        halfY = nodePrefab.GetComponent<SpriteRenderer>().bounds.extents.y;
        pairSize = pairOffSet + 4 * halfX;
        unit = pairSize + offSetX;
    }

    void MakeFamilyTree(){
        Group rootGroup = RootDisplay();
        MakeChildren(rootGroup);
    }

    void MakeChildren(Group rootGroup){
        if(rootGroup.pair.childNum != 0){
            rootGroup.childrenGroup = new();
            List<Vector2> posList = MakeChildPosList(rootGroup.groupPos, rootGroup.pair.childNum);
            for(int i = 0; i < rootGroup.pair.childNum; i++){
                Group group = MakeGroupObject();
                group.pair = rootGroup.pair.children[i];
                if(posList[i].y >= lastY && posList[i].x <= lastX){
                    Debug.Log("Move");
                    lastX += unit;
                    posList[i] = new Vector2(lastX, posList[i].y);
                }
                Debug.Log("LastX : " + lastX.ToString() + "  name : " + group.pair.male.name);
                lastX = posList[i].x;
                lastY = posList[i].y;
                Debug.Log("posX : " + posList[i].x.ToString() + " posY : " + posList[i].y.ToString());
                group.groupPos = posList[i];
                group.transform.position = group.groupPos;
                group.leftPos =  group.groupPos + new Vector2(-1 * (halfX + (pairOffSet / 2)), 0);
                group.rightPos = group.groupPos + new Vector2(halfX + (pairOffSet / 2), 0);
                group.DisplayNodes();
                group.leftDisplay.transform.parent = group.transform;
                group.rightDisplay.transform.parent = group.transform;
                rootGroup.childrenGroup.Add(group);
                group.parentGroup = rootGroup;
                MakeChildren(group);
            }
        }
    }

    Group RootDisplay(){
        Group group = MakeGroupObject();
        group.pair = pairSO.root[0];
        group.groupPos = new Vector2(0, 0);
        group.transform.position = group.groupPos;
        group.leftPos =  group.groupPos + new Vector2(-1 * (halfX + (pairOffSet / 2)), 0);
        group.rightPos = group.groupPos + new Vector2(halfX + (pairOffSet / 2), 0);
        group.DisplayNodes();
        group.leftDisplay.transform.parent = group.transform;
        group.rightDisplay.transform.parent = group.transform;
        return group;
    }
    Group MakeGroupObject(){
        GameObject groupObject = new GameObject("Group");
        Group group = groupObject.AddComponent<Group>();
        group.SetPrefab(nodePrefab, emptyPrefab);
        return group;
    }
    // Group DisplayNodes(Group group){
    //     group.leftDisplay = CreateNode(group.pair, group.pair.male);
    //     group.leftDisplay.transform.position = group.leftPos;
    //     group.rightDisplay = CreateNode(group.pair, group.pair.female);
    //     group.rightDisplay.transform.position = group.rightPos;
    //     return group;
    // }
    // GameObject CreateNode(Node node){
    //     GameObject display;
    //     if(!node.empty){
    //         display = Instantiate(nodePrefab, new Vector2(0, 0), Quaternion.identity);
    //         NodeDisplay nodeDisplay = display.GetComponent<NodeDisplay>();
    //         nodeDisplay.SetNodeData(node);
    //     }
    //     else{
    //         display = Instantiate(emptyPrefab, new Vector2(0, 0), Quaternion.identity);
    //     }
    //     return display;
    // }

    List<Vector2> MakeChildPosList(Vector2 rootPos, int childNum){
        List<Vector2> posList = new();
        float startPoint = rootPos.x;
        for(int i = 0; i < childNum; i++){
            Vector2 pos = new Vector2(startPoint + unit * i, rootPos.y -1 * (halfY * 2 + offSetY));
            posList.Add(pos);
        }
        return posList;
    }
    public void AddFirstNode(Node node)
    {
        if (node.sex == "Male")
        {
            Pair first = new Pair
            {
                parentPair = null,
                male = node,
                female = new Node(),
                isPair = false,
            };
            pairSO.root.Add(first);
        }
        if (node.sex == "Female")
        {
            Pair first = new Pair
            {
                parentPair = null,
                male = new Node(),
                female = node,
                isPair = false,
            };
            pairSO.root.Add(first);
        }
    }
}
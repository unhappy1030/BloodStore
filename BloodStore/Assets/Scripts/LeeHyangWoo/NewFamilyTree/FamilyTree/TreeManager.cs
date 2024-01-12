using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class TreeManagerTest : MonoBehaviour
{
    public PairSO pairSO;
    public GameObject nodePrefab;
    public GameObject emptyPrefab;
    public float pairOffSet = 0.2f;
    public float offSetX, offSetY;
    private float halfX, halfY;


    void Start()
    {
        if (pairSO.root.Count == 0)
        {
            Node node = new Node();
            node.SetAllRandom();
            AddFirstNode(node);
        }
        SetHalfPrefab();
        MakeFamilyTree();
    }

    void SetHalfPrefab(){
        halfX = nodePrefab.GetComponent<SpriteRenderer>().bounds.extents.x;
        halfY = nodePrefab.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    void MakeFamilyTree(){
        Group rootGroup = RootDisplay();
        MakeChildren(rootGroup);
    }

    void MakeChildren(Group rootGroup){
        if(rootGroup.pair.childNum != 0){
            rootGroup.childrenGroup = new();
            List<Vector2> posList = MakeChildPosList(rootGroup.groupPos, rootGroup.pair.childNum, offSetX, offSetY);
            for(int i = 0; i < rootGroup.pair.childNum; i++){
                Group group = MakeGroupObject();
                group.pair = rootGroup.pair.children[i];
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

    List<Vector2> MakeChildPosList(Vector2 rootPos, int childNum, float offSetX, float offSetY){
        List<Vector2> posList = new();
        float pairSize = pairOffSet + 4 * halfX;
        float unit = pairSize + offSetX;
        float startPoint = rootPos.x - (childNum - 1) * unit / 2;
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

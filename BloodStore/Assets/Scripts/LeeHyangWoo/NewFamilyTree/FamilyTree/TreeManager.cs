using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
using Cinemachine;

public class TreeManager : MonoBehaviour
{
    public Pairs pairList;
    public PairTree root;
    public BloodPacks bloodPackList;
    public GameObject nodePrefab;
    public GameObject emptyPrefab;
    public GameObject childButtonPrefab;
    public GameObject childButtonOffPrefab;
    public GameObject mainGroup;
    public float pairOffSet = 0.2f;
    public float offSetX, offSetY;
    private float halfX, halfY;
    private float pairSize, unit;
    private float lastX = 0f, lastY = 0f;
    void Start()
    {
        this.pairList = GameManager.Instance.pairList;
        this.bloodPackList = GameManager.Instance.bloodPackList;
        this.pairList = pairList.Load();
        root = pairList.Deserialize();
        if(pairList.pairs.Count == 0)
        {
            Node node = new Node();
            node.SetAllRandom();
            AddFirstNode(node);
        }
        SetPrefabData();
        MakeFamilyTree();
    }
    void OnDestroy() {
        pairList.Serialize(root);
        pairList.Save(pairList.pairs);
        bloodPackList.Packing(pairList);
        // pairList.Load();
    }
    void SetPrefabData(){
        halfX = nodePrefab.GetComponent<SpriteRenderer>().bounds.extents.x;
        halfY = nodePrefab.GetComponent<SpriteRenderer>().bounds.extents.y;
        pairSize = pairOffSet + 4 * halfX;
        unit = pairSize + offSetX;
    }

    void MakeFamilyTree(){
        MakeMainGroupObject();
        Group rootGroup = RootDisplay();
        MakeChildren(rootGroup);
        MakeCenter(rootGroup);
        // Debug.Log("X : " + rootGroup.groupPos.x.ToString() + "  Y : " + rootGroup.groupPos.y.ToString());
        mainGroup.transform.position = rootGroup.groupPos;
        rootGroup.transform.parent = mainGroup.transform;
        // rootGroup.CameraSetting();
        MakeParentMainGroup(rootGroup);
        mainGroup.transform.position =new Vector2(0, 0);
        rootGroup.PairLine();
        rootGroup.FamilyLine();
        MakeLine(rootGroup);
    }

    void MakeChildren(Group rootGroup){
        if(rootGroup.pairTree.pair.childNum != 0){
            rootGroup.childrenGroup = new();
            List<Vector2> posList = MakeChildPosList(rootGroup.groupPos, rootGroup.pairTree.pair.childNum);
            for(int i = 0; i < rootGroup.pairTree.pair.childNum; i++){
                Group group = MakeGroupObject();
                group.pairTree = rootGroup.pairTree.children[i];
                if(posList[i].y >= lastY && posList[i].x <= lastX){
                    // Debug.Log("Move");
                    lastX += unit;
                    posList[i] = new Vector2(lastX, posList[i].y);
                }
                // Debug.Log("LastX : " + lastX.ToString() + "  name : " + group.pair.male.name);
                lastX = posList[i].x;
                lastY = posList[i].y;
                // Debug.Log("posX : " + posList[i].x.ToString() + " posY : " + posList[i].y.ToString());
                group.groupPos = posList[i];
                group.transform.position = group.groupPos;
                group.leftPos =  group.groupPos + new Vector2(-1 * (halfX + (pairOffSet / 2)), 0);
                group.rightPos = group.groupPos + new Vector2(halfX + (pairOffSet / 2), 0);
                group.DisplayNodes();
                group.leftDisplay.transform.parent = group.transform;
                group.rightDisplay.transform.parent = group.transform;
                rootGroup.childrenGroup.Add(group);
                group.parentGroup = rootGroup;
                group.MakeChildButton();
                MakeChildren(group);
                MakeCenter(group);
            }
        }
    }
    void MakeCenter(Group group){
        if(group.pairTree.pair.childNum != 0){
            group.groupPos = new Vector2((group.childrenGroup[0].groupPos.x + group.childrenGroup[group.childrenGroup.Count - 1].groupPos.x) / 2, group.groupPos.y);
            group.transform.position = group.groupPos;
        }
    }
    Group RootDisplay(){
        Group group = MakeGroupObject();
        group.pairTree = root;
        group.groupPos = new Vector2(0, 0);
        group.transform.position = group.groupPos;
        group.leftPos =  group.groupPos + new Vector2(-1 * (halfX + (pairOffSet / 2)), 0);
        group.rightPos = group.groupPos + new Vector2(halfX + (pairOffSet / 2), 0);
        group.DisplayNodes();
        group.leftDisplay.transform.parent = group.transform;
        group.rightDisplay.transform.parent = group.transform;
        group.MakeChildButton();
        return group;
    }
    void MakeMainGroupObject(){
        mainGroup = new GameObject("MainGroup");
    }
    Group MakeGroupObject(){
        GameObject groupObject = new GameObject("Group");
        InteractObjInfo inter = groupObject.AddComponent<InteractObjInfo>();
        inter._interactType = InteractType.FamilyTree;
        inter._familyTreeType = FamilyTreeType.Group;
        groupObject.layer = LayerMask.NameToLayer("Interact");
        Group group = groupObject.AddComponent<Group>();
        group.SetPrefab(nodePrefab, emptyPrefab, childButtonPrefab, childButtonOffPrefab);
        group.SetSizeData(halfX, halfY, pairSize, unit, pairOffSet, offSetX, offSetY);
        group.MakeBoxCollider();
        return group;
    }

    void MakeParentMainGroup(Group rootGroup){
        if(rootGroup.pairTree.pair.childNum != 0){
            foreach(Group group in rootGroup.childrenGroup){
                group.transform.parent = mainGroup.transform;
                // group.CameraSetting();
                MakeParentMainGroup(group);
            }
        }
    }
    void MakeLine(Group rootGroup){
        if(rootGroup.pairTree.pair.childNum != 0){
            foreach(Group group in rootGroup.childrenGroup){
                group.PairLine();
                group.FamilyLine();
                MakeLine(group);
            }
        }
    }
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
                male = node,
                female = new Node(),
                isPair = false,
            };
            root = new PairTree(first);
        }
        if (node.sex == "Female")
        {
            Pair first = new Pair
            {
                male = new Node(),
                female = node,
                isPair = false,
            };
            root = new PairTree(first);
        }
    }
}
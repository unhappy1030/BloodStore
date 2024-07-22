using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
using Cinemachine;

public class TreeManager : MonoBehaviour
{
    public PairManager pairManager;
    public TreePair root;
    public BloodPacks bloodPackList;
    public GameObject nodePrefab;
    public GameObject emptyPrefab;
    public GameObject deadPrefab;
    public GameObject childButtonPrefab;
    public GameObject childButtonOffPrefab;
    public GameObject highLightPrefab;
    public GameObject mainGroup;
    public GameObject selectedCard;
    public NodeSO nodeSO;
    public AddChildSO addChildSO;
    public float pairOffSet = 0.2f;
    public float offSetX, offSetY;
    private float halfX, halfY;
    private float pairSize, unit;
    private float lastX = 0f, lastY = 0f;
    void Awake()
    {
        nodeSO.node.empty = true;
        this.pairManager = GameManager.Instance.pairManager;
        this.bloodPackList = GameManager.Instance.bloodPackList;
        root = pairManager.Deserialize();
        if(pairManager.serializePairList.Count == 0)
        {
            Node node = new Node();
            node.SetAllRandom();
            AddFirstNode(node);
        }
        SetPrefabData();
        MakeFamilyTree();
    }
    public void SavePairData() {
        pairManager.Serialize(root);
        pairManager.Save();
    }
    public void SaveAndChangeData(){
        pairManager.Serialize(root);
        pairManager.MakeOlder();
        pairManager.MakeDead();
        pairManager.Save();
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
                group.MakeChildButton(childButtonPrefab, childButtonOffPrefab);
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
        group.MakeChildButton(childButtonPrefab, childButtonOffPrefab);
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
        group.SetValues(addChildSO);
        group.SetPrefab(nodePrefab, emptyPrefab, deadPrefab);
        group.SetSizeData(halfX, halfY, pairSize, unit, pairOffSet, offSetX, offSetY);
        group.SetUI(selectedCard);
        group.MakeBoxCollider();
        group.highLight = Instantiate(highLightPrefab, new Vector2(0, 0), Quaternion.identity);
        group.highLight.transform.localScale = new Vector3(pairSize, halfY * 2);
        group.highLight.transform.SetParent(groupObject.transform);
        group.highLight.SetActive(false);
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
            SerializePair first = new SerializePair
            {
                male = node,
                female = new Node(),
                isPair = false,
            };
            root = new TreePair(first);
        }
        if (node.sex == "Female")
        {
            SerializePair first = new SerializePair
            {
                male = new Node(),
                female = node,
                isPair = false,
            };
            root = new TreePair(first);
        }
    }
}
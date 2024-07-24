using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 가계를 화면에 그림
/// </summary>
public class TreeManager : MonoBehaviour
{
    public PairManager pairManager;
    public TreePair root;
    public BloodPacks bloodPackList;
    public FamilyTreePrefabSO familyTreePrefabSO;
    public GameObject mainGroup;
    public GameObject selectedCard;
    public NodeSO nodeSO;
    public AddChildSO addChildSO;
    private float lastX = 0f, lastY = 0f;
    /// <summary>
    /// 초기에 필요한 세팅을 적용
    /// </summary>
    void Awake()
    {
        familyTreePrefabSO.treePrefab.SetLength();
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
        MakeFamilyTree();
    }
    /// <summary>
    /// 현재 TreePair의 root를 통해서 전체 트리를 직렬화 하여 저장
    /// </summary>
    public void SavePairData() {
        pairManager.Serialize(root);
        pairManager.Save();
    }
    /// <summary>
    /// 현재 TreePair의 root를 통해서 전체 트리를 직렬화 하고 나이 증가와 죽음 판별 후 저장
    /// </summary>
    public void SaveAndChangeData(){
        pairManager.Serialize(root);
        pairManager.MakeOlder();
        pairManager.MakeDead();
        pairManager.Save();
    }
    /// <summary>
    /// 가계도 그리기
    /// </summary>
    void MakeFamilyTree(){
        MakeMainGroupObject();
        Group rootGroup = RootDisplay();
        MakeChildren(rootGroup);
        MakeCenter(rootGroup);
        // Debug.Log("X : " + rootGroup.groupPos.x.ToString() + "  Y : " + rootGroup.groupPos.y.ToString());
        mainGroup.transform.position = rootGroup.groupPosition;
        rootGroup.transform.parent = mainGroup.transform;
        // rootGroup.CameraSetting();
        MakeParentMainGroup(rootGroup);
        mainGroup.transform.position =new Vector2(0, 0);
        rootGroup.PairLine();
        rootGroup.FamilyLine();
        MakeLine(rootGroup);
    }
    /// <summary>
    /// "MainGroup"을 이름으로 하는 가계도 전체를 자식 오브젝트로하는 GameObject 생성
    /// </summary>
    void MakeMainGroupObject(){
        mainGroup = new GameObject("MainGroup");
    }
    /// <summary>
    /// GroupObject를 생성
    /// </summary>
    /// <returns></returns>
    Group MakeGroupObject(){
        GameObject groupObject = new GameObject("Group");
        InteractObjInfo inter = groupObject.AddComponent<InteractObjInfo>();
        inter._interactType = InteractType.FamilyTree;
        inter._familyTreeType = FamilyTreeType.Group;
        groupObject.layer = LayerMask.NameToLayer("Interact");
        Group group = groupObject.AddComponent<Group>();
        group.familyTreePrefabSO = this.familyTreePrefabSO;
        group.SetValues(addChildSO);
        group.SetUI(selectedCard);
        group.MakeBoxCollider();
        group.highLight = Instantiate(familyTreePrefabSO.treePrefab.highLightPrefab, new Vector2(0, 0), Quaternion.identity);
        group.highLight.transform.localScale = new Vector3(familyTreePrefabSO.treePrefab.pairLength, familyTreePrefabSO.treePrefab.nodeHalfLength[1] * 2);
        group.highLight.transform.SetParent(groupObject.transform);
        group.highLight.SetActive(false);
        return group;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Group RootDisplay(){
        Group group = MakeGroupObject();
        group.pairTree = root;
        group.groupPosition = new Vector2(0, 0);
        group.transform.position = group.groupPosition;
        group.leftNodePosition =  group.groupPosition + new Vector2(-1 * (familyTreePrefabSO.treePrefab.nodeHalfLength[0] + (familyTreePrefabSO.treePrefab.nodeOffset / 2)), 0);
        group.rightNodePosition = group.groupPosition + new Vector2(familyTreePrefabSO.treePrefab.nodeHalfLength[0] + (familyTreePrefabSO.treePrefab.nodeOffset / 2), 0);
        group.DisplayNodes();
        group.leftNode.transform.parent = group.transform;
        group.rightNode.transform.parent = group.transform;
        group.MakeChildButton();
        return group;
    }

    void MakeChildren(Group rootGroup){
        if(rootGroup.pairTree.pair.childNum != 0){
            rootGroup.childGroupList = new();
            List<Vector2> posList = MakeChildPosList(rootGroup.groupPosition, rootGroup.pairTree.pair.childNum);
            for(int i = 0; i < rootGroup.pairTree.pair.childNum; i++){
                Group group = MakeGroupObject();
                group.pairTree = rootGroup.pairTree.children[i];
                if(posList[i].y >= lastY && posList[i].x <= lastX){
                    // Debug.Log("Move");
                    lastX += familyTreePrefabSO.treePrefab.unit;
                    posList[i] = new Vector2(lastX, posList[i].y);
                }
                // Debug.Log("LastX : " + lastX.ToString() + "  name : " + group.pair.male.name);
                lastX = posList[i].x;
                lastY = posList[i].y;
                // Debug.Log("posX : " + posList[i].x.ToString() + " posY : " + posList[i].y.ToString());
                group.groupPosition = posList[i];
                group.transform.position = group.groupPosition;
                group.leftNodePosition =  group.groupPosition + new Vector2(-1 * (familyTreePrefabSO.treePrefab.nodeHalfLength[0] + (familyTreePrefabSO.treePrefab.nodeOffset / 2)), 0);
                group.rightNodePosition = group.groupPosition + new Vector2(familyTreePrefabSO.treePrefab.nodeHalfLength[0] + (familyTreePrefabSO.treePrefab.nodeOffset / 2), 0);
                group.DisplayNodes();
                group.leftNode.transform.parent = group.transform;
                group.rightNode.transform.parent = group.transform;
                rootGroup.childGroupList.Add(group);
                group.parentGroup = rootGroup;
                group.MakeChildButton();
                MakeChildren(group);
                MakeCenter(group);
            }
        }
    }
    void MakeCenter(Group group){
        if(group.pairTree.pair.childNum != 0){
            group.groupPosition = new Vector2((group.childGroupList[0].groupPosition.x + group.childGroupList[group.childGroupList.Count - 1].groupPosition.x) / 2, group.groupPosition.y);
            group.transform.position = group.groupPosition;
        }
    }
    
    void MakeParentMainGroup(Group rootGroup){      //DFS 중복
        if(rootGroup.pairTree.pair.childNum != 0){
            foreach(Group group in rootGroup.childGroupList){
                group.transform.parent = mainGroup.transform;
                // group.CameraSetting();
                MakeParentMainGroup(group);
            }
        }
    }
    void MakeLine(Group rootGroup){                 //DFS 중복
        if(rootGroup.pairTree.pair.childNum != 0){
            foreach(Group group in rootGroup.childGroupList){
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
            Vector2 pos = new Vector2(startPoint + familyTreePrefabSO.treePrefab.unit * i, rootPos.y -1 * (familyTreePrefabSO.treePrefab.nodeHalfLength[1] * 2 + familyTreePrefabSO.treePrefab.pairOffSetLength[1]));
            posList.Add(pos);
        }
        return posList;
    }
    /// <summary>
    /// 가계도에 제일 처음 노드의 값을 지정
    /// </summary>
    /// <param name="node"> 가계도의 제일 처음 노드 </param>
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
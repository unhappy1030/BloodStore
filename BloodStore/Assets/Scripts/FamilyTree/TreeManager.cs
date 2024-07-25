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
        mainGroup = new GameObject("MainGroup");
        Group rootGroup = CreateAllGroupObject(root);
        SetAllGroupPosition(rootGroup);
        mainGroup.transform.position = rootGroup.groupPosition;
        MakeParentMainGroup(rootGroup);
        mainGroup.transform.position =new Vector2(0, 0);
        MakeLine(rootGroup);
    }
    /// <summary>
    /// 모든 Group오브젝트를 생성
    /// </summary>
    /// <param name="treePair">Group오브젝트에 저장될 treePair</param>
    /// <returns>마지막으로 RootGroup을 반환</returns>
    Group CreateAllGroupObject(TreePair treePair){
        Group now = CreateGroupObject(treePair);
        if(now.treePair.pair.childNum != 0){
            now.childGroupList = new();
            for(int i = 0; i < now.treePair.pair.childNum; i++){
                Group group = CreateAllGroupObject(now.treePair.children[i]);
                group.parentGroup = now;
                now.childGroupList.Add(group);
            }
        }
        return now;
    }
    /// <summary>
    /// 하나의 Group오브젝트를 생성 및 반환
    /// </summary>
    /// <param name="treePair">Group오브젝트에 저장될 treePair</param>
    /// <returns>초기 설정이 된 Group반환</returns>
    Group CreateGroupObject(TreePair treePair){
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
        group.treePair = treePair;
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
    /// <summary>
    /// 생성된 모든 Group을 순회하며 Group위치 지정
    /// </summary>
    /// <param name="parentGroup">순환을 위한 rootGroup</param>
    void SetAllGroupPosition(Group parentGroup){
        if(parentGroup.treePair.pair.childNum != 0){
            List<Vector2> posList = MakeChildPosList(parentGroup.groupPosition, parentGroup.treePair.pair.childNum);
            for(int i = 0; i < parentGroup.treePair.pair.childNum; i++){
                Group group = parentGroup.childGroupList[i];
                if(posList[i].y >= lastY && posList[i].x <= lastX){
                    lastX += familyTreePrefabSO.treePrefab.unit;
                    posList[i] = new Vector2(lastX, posList[i].y);
                }
                lastX = posList[i].x;
                lastY = posList[i].y;
                group.groupPosition = posList[i];
                group.transform.position = group.groupPosition;
                SetAllGroupPosition(group);
            }
        }
        SetPositionCenter(parentGroup);
    }
    /// <summary>
    /// 부모 좌표를 통해서 자식들의 좌표 리스트를 반환
    /// </summary>
    /// <param name="rootPos">부모 Group의 좌표</param>
    /// <param name="childNum">자식의 수</param>
    /// <returns>자식들의 좌표 리스트를 반환</returns>
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
    /// 부모 Group을 자식 Group들의 중앙으로 위치 변경
    /// </summary>
    /// <param name="group">부모 Group</param>
    void SetPositionCenter(Group group){
        if(group.treePair.pair.childNum != 0){
            group.groupPosition = new Vector2((group.childGroupList[0].groupPosition.x + group.childGroupList[group.childGroupList.Count - 1].groupPosition.x) / 2, group.groupPosition.y);
            group.transform.position = group.groupPosition;
        }
    }
    /// <summary>
    ///  모든 Group 오브젝트를 MainGroup의 하위로 지정
    /// </summary>
    /// <param name="parentGroup"> Root Group(트리 순회를 위한) </param>
    void MakeParentMainGroup(Group parentGroup){  
        parentGroup.transform.parent = mainGroup.transform;    //DFS 중복
        if(parentGroup.treePair.pair.childNum != 0){
            foreach(Group group in parentGroup.childGroupList){
                MakeParentMainGroup(group);
            }
        }
    }
    /// <summary>
    /// 모든 Group 오브젝트의 가계도에 맞는 선을 그림
    /// </summary>
    /// <param name="parentGroup"> Root Group(트리 순회를 위한) </param>
    void MakeLine(Group parentGroup){                 //DFS 중복
        parentGroup.PairLine();
        parentGroup.FamilyLine();
        if(parentGroup.treePair.pair.childNum != 0){
            foreach(Group group in parentGroup.childGroupList){
                MakeLine(group);
            }
        }
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
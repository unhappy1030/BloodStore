using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class NodeInteraction : MonoBehaviour
{
    // warning : object must be set Layer as "FamilyTree"

    public enum NodeShowingStatus{
        ShowTotal,
        ShowFamily,
        ShowGroup
    }

    public enum NodeInteractionStatus{
        None,
        ShowInfo,
        SelectPair
    }

    public CameraControl cameraControl;
    public NodeShowingStatus nodeShowingStatus;
    public NodeInteractionStatus nodeInteractionStatus;
    public Group currentGroup;
    public Group currentParent;
    bool wasZeroChild;
    bool wasNodeActived;

    void Start(){
        wasZeroChild = false;
        wasNodeActived =false;
        nodeShowingStatus = NodeShowingStatus.ShowTotal;
        nodeInteractionStatus = NodeInteractionStatus.None;
        ShowTotal();
    }

    void Update(){
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.zero, 0f, LayerMask.GetMask("FamilyTree"));

            if (ray.collider != null)
            {
                MouseInteract(ray.collider.gameObject);
                // Debug.Log("FamilyTree interaction...");
            }
            else
            {
                // 이거 어떻게 처리할지 정해야 함!
                GoBackToCurrentStatus();
            }
        }
    }

    void MouseInteract(GameObject interactObj){
        Group group = interactObj.GetComponent<Group>();
        NodeDisplay node = interactObj.GetComponent<NodeDisplay>();

        if(group != null)
        {
            GroupInteract(group);
        }
        else if(node != null)
        {
            // NodeInteract(node);
        }
    }

    void GroupInteract(Group _newgroup){
        if(nodeShowingStatus == NodeShowingStatus.ShowTotal) // From Start
        {
            ShowFamily(_newgroup);
        }
        else if(nodeShowingStatus == NodeShowingStatus.ShowFamily)
        {
            if(_newgroup == currentParent || _newgroup.parentGroup == currentParent && (_newgroup.childrenGroup == null || _newgroup.childrenGroup.Count == 0))
            {
                ShowGroup(_newgroup);
            }
            else
            {
                ShowFamily(_newgroup);
            }

            currentGroup = _newgroup;
        }
    }

    void ShowTotal(){
        Debug.Log("ShowTotal...");

        nodeShowingStatus = NodeShowingStatus.ShowTotal;
    }

    void ShowFamily(Group _group){
        Debug.Log("ShowFamily...");

        // set camera target
        List<GameObject> familyTarget = new();

        if(_group.childrenGroup == null || _group.childrenGroup.Count == 0) // no children(no family) -> show parent's family
        {
            Group parent = _group.parentGroup;
            List<Group> siblings = parent.childrenGroup;

            familyTarget.Add(parent.gameObject);
            foreach(Group sibling in siblings){
                familyTarget.Add(sibling.gameObject);
            }

            wasZeroChild = true;
            currentParent = _group.parentGroup;
        }
        else
        {
            List<Group> children = _group.childrenGroup;

            familyTarget.Add(_group.gameObject);
            foreach(Group child in children){
                familyTarget.Add(child.gameObject);
            }

            wasZeroChild = false;
            currentParent = _group;
        }

        CreateTargetCamera(familyTarget);

        if(currentGroup != null){
            EnableNodeCollider(currentGroup, false);
        }
        
        nodeShowingStatus = NodeShowingStatus.ShowFamily;
    }

    void ShowGroup(Group _group){
        Debug.Log("ShowGroup...");
        
        // set target
        List<GameObject> groupTarget = new();
        groupTarget.Add(_group.gameObject);

        CreateTargetCamera(groupTarget);

        // setActive Node collider
        EnableNodeCollider(_group, true);

        nodeShowingStatus = NodeShowingStatus.ShowGroup;
        nodeInteractionStatus = NodeInteractionStatus.None;
    }

    void NodeInteract(NodeDisplay node){
        Debug.Log("Interacting Node...");

        if(nodeInteractionStatus == NodeInteractionStatus.None)
        {
            ShowNodeInfo(node);
        }
        else if(nodeInteractionStatus == NodeInteractionStatus.SelectPair)
        {
            SelectPair(node);
        }
    }


    void ShowNodeInfo(NodeDisplay nodeDisplay){
        Group group = nodeDisplay.GetComponentInParent<Group>();
        if(group == null){
            Debug.Log("There is no group script...");
            return;
        }

        Pair pair = group.pair;
        Node node;
        if(nodeDisplay.sexLabel.text == "Male"){
            node = pair.male;
        }
        else{
            node = pair.female;
        }

        Debug.Log("<Node Info>");
        Debug.Log(node.name);
        Debug.Log(node.sex);
        Debug.Log(node.age);
        Debug.Log(node.bloodType[1] + node.bloodType[2]);
        Debug.Log(node.hp);
    }

    void SelectPair(NodeDisplay node){

    }


    void GoBackToCurrentStatus(){
        Debug.Log("Go back to current Status....");
        switch(nodeShowingStatus){
            case NodeShowingStatus.ShowFamily:
                ShowTotal();
            break;
            case NodeShowingStatus.ShowGroup:
                ShowFamily(currentParent);
            break;
        }
    }
    
    void EnableNodeCollider(Group _group, bool enable){
        BoxCollider2D boxCollider2D = _group.GetComponent<BoxCollider2D>();
        if(boxCollider2D != null){
            boxCollider2D.enabled = !enable;
        }


        BoxCollider2D[] nodeColliders = _group.GetComponentsInChildren<BoxCollider2D>();
        foreach(BoxCollider2D collider in nodeColliders){
            if(collider.gameObject != _group.gameObject)
                collider.enabled = enable;
        }

        wasNodeActived = enable;
    }

    void CreateTargetCamera(List<GameObject> targets){
        InteractObjInfo interactObjInfo = gameObject.GetComponent<InteractObjInfo>();
        if(interactObjInfo == null)
            interactObjInfo = gameObject.AddComponent<InteractObjInfo>();

        interactObjInfo.SetTargetCameraInfo(targets, 0.25f, CinemachineBlendDefinition.Style.EaseInOut, 0.5f);
        cameraControl.ChangeCam(interactObjInfo);
    }

}


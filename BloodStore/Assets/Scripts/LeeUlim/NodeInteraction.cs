using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NodeInteraction : MonoBehaviour
{
    // warning : object must be set Layer as "FamilyTree"

    public enum NodeShowingStatus{
        ShowTotal,
        ShowFamily,
        ShowGroup,
        ShowNode
    }

    public enum NodeInteractionStatus{
        None,
        ShowInfo,
        SelectPair
    }

    public NodeShowingStatus nodeShowingStatus;
    public NodeInteractionStatus cardInteractionStatus;
    public GameObject currentGroup;
    bool wasNodeActived;

    void Start(){
        wasNodeActived =false;
        cardInteractionStatus = NodeInteractionStatus.None;
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
            NodeInteract();
        }
    }

    void GroupInteract(Group _newgroup){
        if(currentGroup == null || _newgroup.gameObject != currentGroup) // the first interaction or same group interaction
        {
            ShowFamily(_newgroup);
        }
        else
        {
            ShowGroup(_newgroup);
        }
        currentGroup = _newgroup.gameObject;
    }

    void ShowTotal(){
        nodeShowingStatus = NodeShowingStatus.ShowTotal;
        Debug.Log("ShowTotal...");
    }

    void ShowFamily(Group _group){
        if(wasNodeActived == true)
        {
            EnableNodeCollider(_group, false);
        }

        nodeShowingStatus = NodeShowingStatus.ShowFamily;
        Debug.Log("ShowFamily...");
    }

    void ShowGroup(Group _group){
        // setActive Node collider
        Debug.Log("_group : " + (_group == null).ToString());
        EnableNodeCollider(_group, true);

        nodeShowingStatus = NodeShowingStatus.ShowGroup;
        Debug.Log("ShowGroup...");
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

    void NodeInteract(){
        Debug.Log("Interacting Node...");
        nodeShowingStatus = NodeShowingStatus.ShowNode;
    }

    void GoBackToCurrentStatus(){
        Debug.Log("Go back to current Status....");
        switch(nodeShowingStatus){
            case NodeShowingStatus.ShowFamily:
                ShowTotal();
            break;
            case NodeShowingStatus.ShowGroup:
                Group currntGroup1 = currentGroup.GetComponent<Group>();
                if(currntGroup1 != null)
                    ShowFamily(currntGroup1);
            break;
            case NodeShowingStatus.ShowNode:
                Group currntGroup2 = currentGroup.GetComponent<Group>();
                if(currntGroup2 != null)
                    ShowGroup(currntGroup2);
            break;

        }
    }

}

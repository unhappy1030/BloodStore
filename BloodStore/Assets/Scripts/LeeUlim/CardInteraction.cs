using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInteraction : MonoBehaviour
{
    // warning : object must be set Layer as "FamilyTree"

    public enum CardShowingStatus{
        ShowTotal,
        ShowFamily,
        ShowGroup
    }
    public enum CardInteractionStatus{
        None,
        ShowInfo,
        SelectPair
    }

    public CardShowingStatus cardShowingStatus;
    public CardInteractionStatus cardInteractionStatus;
    public GameObject currentObj;

    void Start(){
        
        cardInteractionStatus = CardInteractionStatus.None;
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
                Debug.Log("FamilyTree interaction...");
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

        }
    }

    void GroupInteract(Group _newgroup){
        if(currentObj == null || _newgroup.gameObject != currentObj) // the first interaction or same group interaction
        {
            ShowFamily(_newgroup);
        }
        else
        {
            ShowGroup(_newgroup);
        }
        currentObj = _newgroup.gameObject;
    }

    void ShowTotal(){
        // setActive Group collider
        cardShowingStatus = CardShowingStatus.ShowTotal;
        Debug.Log("ShowTotal...");
    }

    void ShowFamily(Group _group){
        // setActive Group collider
        cardShowingStatus = CardShowingStatus.ShowFamily;
        Debug.Log("ShowFamily...");
    }

    void ShowGroup(Group _group){
        // setActive Card collider
        cardShowingStatus = CardShowingStatus.ShowGroup;
        Debug.Log("ShowGroup...");
    }

    // void NodeInteraction(){
        
    // }

    void GoBackToCurrentStatus(){
        Debug.Log("Go back to current Status....");
        switch(cardShowingStatus){
            case CardShowingStatus.ShowFamily:
                ShowTotal();
            break;
            case CardShowingStatus.ShowGroup:
                Group currntGroup = currentObj.GetComponent<Group>();
                if(currntGroup != null)
                    ShowFamily(currntGroup);
            break;
        }
    }

}

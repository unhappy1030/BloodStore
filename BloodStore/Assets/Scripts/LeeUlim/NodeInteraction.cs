using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yarn.Unity;

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

    public GameObject nodeInfoCanvas; // assign at Inspector
    public GameObject nodeInfoTexts; // assign at Inspector
    public Image nodeImg; // assign at Inspector
    public TreeManagerTest treeManagerTest;
    public CameraControl cameraControl;
    public NodeShowingStatus nodeShowingStatus;
    public NodeInteractionStatus nodeInteractionStatus;
    public Group currentGroup;
    public Group currentParent;
    public DialogueRunner dialogueRunner;
    bool wasNodeActived;

    void Start(){
        treeManagerTest = FindObjectOfType<TreeManagerTest>();
        nodeInfoCanvas.SetActive(false);
        wasNodeActived =false;
        nodeShowingStatus = NodeShowingStatus.ShowTotal;
        nodeInteractionStatus = NodeInteractionStatus.None;
        ShowTotal();
    }

    void Update(){
        if (Input.GetMouseButtonDown(0) 
            && !EventSystem.current.IsPointerOverGameObject() 
            && !dialogueRunner.IsDialogueRunning 
            && !GameManager.Instance.isFading
            && !cameraControl.mainCam.IsBlending)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.zero, 0f, LayerMask.GetMask("FamilyTree"));

            Debug.Log("ray.collider : " + (ray.collider == null).ToString());

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
        EmptyDisplay emptyNode = interactObj.GetComponent<EmptyDisplay>();

        if(group != null)
        {
            GroupInteract(group);
        }
        else if(node != null)
        {
            ShowNodeInfo(node);
        }
        else if(emptyNode != null)
        {
            SelectPair(emptyNode);    
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
            
            if(treeManagerTest.pairSO.pairs.Count <= 1 || _group.pairTree.pair == treeManagerTest.pairSO.pairs[0]){
                ShowGroup(_group);
                return;
            }

            List<Group> siblings = parent.childrenGroup;

            familyTarget.Add(parent.gameObject);
            foreach(Group sibling in siblings){
                familyTarget.Add(sibling.gameObject);
            }

            currentParent = _group.parentGroup;
        }
        else
        {
            List<Group> children = _group.childrenGroup;

            familyTarget.Add(_group.gameObject);
            foreach(Group child in children){
                familyTarget.Add(child.gameObject);
            }

            currentParent = _group;
        }

        CreateTargetCamera(familyTarget);

        if(currentGroup != null){
            EnableNodeCollider(currentGroup, false);
        }
        
        nodeShowingStatus = NodeShowingStatus.ShowFamily;
        nodeInteractionStatus = NodeInteractionStatus.None;
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

    void ShowNodeInfo(NodeDisplay nodeDisplay){
        Group group = nodeDisplay.GetComponentInParent<Group>();
        if(group == null){
            Debug.Log("There is no group script...");
            return;
        }

        PairTree pairTree = group.pairTree;
        Node node;
        if(nodeDisplay.sexLabel.text == "Male"){
            node = pairTree.pair.male;
        }
        else{
            node = pairTree.pair.female;
        }

        nodeInfoCanvas.SetActive(true);
        SetNodeInfoUIImg(nodeDisplay);
        SetNodeInfoUIText(node);
        nodeInteractionStatus = NodeInteractionStatus.ShowInfo;
    }

    void SetNodeInfoUIImg(NodeDisplay nodeDisplay){
        GameObject spriteObj = nodeDisplay.transform.GetChild(0).gameObject;
        SpriteRenderer spriteScript = spriteObj.GetComponent<SpriteRenderer>();
        
        if(spriteScript == null){
            Debug.Log("there is no Sprite Renderer in Node child...");
            return;
        }
        
        nodeImg.sprite = spriteScript.sprite;
    }

    void SetNodeInfoUIText(Node node){
        List<TextMeshProUGUI> texts = new();
        int childCount = nodeInfoTexts.transform.childCount;

        for(int i=0; i<childCount; i++){
            GameObject textObj = nodeInfoTexts.transform.GetChild(i).gameObject;
            TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
            texts.Add(tmp);
        }

        texts[0].text = "Name : " + node.name;
        texts[1].text = "Sex : " + node.sex;
        texts[2].text = "Age : " + node.age;
        texts[3].text = "BloodType : " + node.bloodType[1] + node.bloodType[2];
        texts[4].text = "HP : " + node.hp;
        texts[5].text = "Type : " + "None";
    }

    void SelectPair(EmptyDisplay emptyNode){
        Debug.Log("Select Pair start...");
        Group group = emptyNode.transform.parent.GetComponent<Group>();

        if(group == null){
            Debug.Log("There is no group in emptyNode's parent...");
            return;
        }

        emptyNode.SetNode();
        nodeInteractionStatus = NodeInteractionStatus.SelectPair;
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


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

    public NodeShowingStatus nodeShowingStatus;
    public NodeInteractionStatus nodeInteractionStatus;

    public Group currentGroup;
    public Group currentSelectGroup;
    public Group currentParent;

    int currentHorInput;
    int currentVerInput;
    
    bool wasNodeActived;
    bool wasRoot;
    bool isFirstInput;

    // public TreeManagerTest treeManagerTest;

    public CameraControl cameraControl;
    public DialogueRunner dialogueRunner;

    
    void Start(){
        // treeManagerTest = FindObjectOfType<TreeManagerTest>();
        nodeInfoCanvas.SetActive(false);

        currentHorInput = 0;
        currentVerInput = 0;

        wasNodeActived = false;
        wasRoot = false;
        isFirstInput = true;
        
        nodeShowingStatus = NodeShowingStatus.ShowTotal;
        nodeInteractionStatus = NodeInteractionStatus.None;
        
        ShowTotal();
    }

    void Update(){
        if(!dialogueRunner.IsDialogueRunning 
            && !GameManager.Instance.isFading
            && !cameraControl.mainCam.IsBlending)
        {
            if(isFirstInput){
                KeyInteract();
            }

            if(Input.GetMouseButtonDown(0) 
                && !EventSystem.current.IsPointerOverGameObject())
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
    }

    void ShowSelectedGroup(Group selectedGroup){
        if(currentGroup != null){
            LineRenderer currentLine = currentGroup.GetComponentInChildren<LineRenderer>();
            currentLine.material.color = Color.black;
        }

        LineRenderer newLine = selectedGroup.GetComponentInChildren<LineRenderer>();
        newLine.material.color = Color.white;
    }

    void KeyInteract(){
        int hInput = (int)Input.GetAxisRaw("Horizontal");
        int vInput = (int)Input.GetAxisRaw("Vertical");

        if(hInput != 0 && vInput != 0){ // avoid duplication between hor and ver input
            return;
        }

        if(currentGroup == null){
            return;
        }

        if(nodeShowingStatus != NodeShowingStatus.ShowFamily){
            Debug.Log("Key input is only acceptable in ShowFamily status...");
            return;
        }

        if(currentHorInput != hInput) // Avoid duplication of input
        {
            if(hInput != 0){
                Debug.Log("Hor Move");
                HorKeyInput(hInput);
            }
            currentHorInput = hInput;
        }
        else if(currentVerInput != vInput) // Avoid duplication of input
        {
            if(vInput != 0){
                Debug.Log("Ver Move");
                VerKeyInput(vInput);
            }
            currentVerInput = vInput;
        }
    }

    void HorKeyInput(int input){
        if(currentParent == null){
            Debug.Log("There is no parent in current node...");
            return;
        }

        if(currentParent.childrenGroup.Count <= 1){
            Debug.Log("It is the only child...");
            return;
        }

        int currentIndex = -1;
        int i=0;

        foreach(Group sibling in currentParent.childrenGroup){ // get current selected group sibling index
            if(sibling == currentGroup){
                currentIndex = i;
                break;
            }
            i++;
        }

        if(currentIndex == -1){
            Debug.Log("There is no current select Group in this parent...");
            return;
        }

        if(input > 0) // Right input
        {
            if(currentIndex == currentParent.childrenGroup.Count - 1){
                Debug.Log("It is the youngest...");
                return;
            }

            Group sibling = currentParent.childrenGroup[currentIndex + 1];
            ShowFamily(sibling);
            
            ShowSelectedGroup(sibling);
            currentGroup = sibling;
            currentParent = sibling.parentGroup;

        }
        else // Left input
        {
            if(i == 0){
                Debug.Log("It is the Oldest...");
                return;
            }

            Group sibling = currentParent.childrenGroup[currentIndex - 1];
            ShowFamily(sibling);

            ShowSelectedGroup(sibling);
            currentGroup = sibling;
            currentParent = sibling.parentGroup;
        }
    }
    
    void VerKeyInput(int input){
        if(input > 0) // Up
        {
            if(currentParent == null){
                Debug.Log("There is no parent to move...");
                return;
            }
            ShowFamily(currentParent);

            ShowSelectedGroup(currentParent);
            currentGroup = currentParent;
            currentParent = currentParent.parentGroup;
        }
        else // Down
        {
            if(currentParent.childrenGroup.Count == 0){
                Debug.Log("There is no parent to move...");
                return;
            }
            
            ShowFamily(currentParent.childrenGroup[0]);

            ShowSelectedGroup(currentParent.childrenGroup[0]);
            currentGroup = currentParent.childrenGroup[0];
            currentParent = currentParent.childrenGroup[0].parentGroup;
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
        ShowGroup(_newgroup);

        ShowSelectedGroup(_newgroup);
        currentGroup = _newgroup;
    }

    void ShowTotal(){
        Debug.Log("ShowTotal...");

        if(currentSelectGroup != null){
            EnableNodeCollider(currentSelectGroup, false);
        }
        
        currentGroup = null;
        currentParent = null;
        
        nodeShowingStatus = NodeShowingStatus.ShowTotal;
    }

    void ShowFamily(Group _parent){
        Debug.Log("ShowFamily...");
        
        if(currentSelectGroup != null){
            EnableNodeCollider(currentSelectGroup, false);
        }     

        // set camera target
        List<GameObject> familyTarget = new();
        familyTarget.Add(_parent.gameObject);

        if(_parent.childrenGroup == null || _parent.childrenGroup.Count == 0){
            return;
        }
        
        foreach(Group child in _parent.childrenGroup){
            familyTarget.Add(child.gameObject);
        }

        /*
        if(_group.childrenGroup == null || _group.childrenGroup.Count == 0) // no children(no family) -> show parent's family
        {
            Group parent = _group.parentGroup;
            
            if(GameManager.Instance.pairList.pairs.Count <= 1 || _group.pairTree.pair == GameManager.Instance.pairList.pairs[0]){ // if root
                ShowGroup(_group);
                wasRoot = true;
                return;
            }

            List<Group> siblings = parent.childrenGroup;

            familyTarget.Add(parent.gameObject);
            foreach(Group sibling in siblings){
                familyTarget.Add(sibling.gameObject);
            }

            currentParent = _group.parentGroup;
        }
        else // show its own family
        {
            List<Group> children = _group.childrenGroup;

            familyTarget.Add(_group.gameObject);
            foreach(Group child in children){
                familyTarget.Add(child.gameObject);
            }

            currentParent = _group;
        }
        */

        CreateTargetCamera(familyTarget);
        
        currentParent = _parent;

        nodeShowingStatus = NodeShowingStatus.ShowFamily;
        nodeInteractionStatus = NodeInteractionStatus.None;
    }

    void ShowGroup(Group _group){
        Debug.Log("ShowGroup...");
        
        // set target
        List<GameObject> groupTarget = new();
        groupTarget.Add(_group.gameObject);

        CreateTargetCamera(groupTarget);

        EnableNodeCollider(_group, true); // setActive Node collider

        currentSelectGroup = _group;
        
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
                if(wasRoot)
                {
                    ShowTotal();
                    wasRoot = false;
                }
                else
                {
                    ShowFamily(currentParent);
                }
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


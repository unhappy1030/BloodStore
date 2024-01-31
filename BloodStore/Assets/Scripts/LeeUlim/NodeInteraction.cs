using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yarn.Unity;
using System.Xml.Serialization;

public class NodeInteraction : MonoBehaviour
{
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

    KeyCode[] AlphakeyCodes = {
        KeyCode.Alpha0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9
    };

    public GameObject nodeInfoCanvas; // assign at Inspector
    public GameObject nodeInfoTexts; // assign at Inspector
    public GameObject addChildUI;
    public GameObject treeManager;
    public Image nodeImg; // assign at Inspector

    public NodeShowingStatus nodeShowingStatus;
    public NodeInteractionStatus nodeInteractionStatus;

    public Group currentSelectGroup;
    public Group currentParent;
    public Group leftGroup;
    public Group rightGroup;
    public Group upGroup;
    public List<Group> downGroup;
    TreeManager tree;
    bool mouseMoveCheck;
    int currentH;
    int currentV;
    
    bool wasNodeActived;
    bool wasRoot;

    // public TreeManagerTest treeManagerTest;

    public CameraControl cameraControl;
    public DialogueRunner dialogueRunner;

    
    void Start(){
        // treeManagerTest = FindObjectOfType<TreeManagerTest>();
        tree = treeManager.GetComponent<TreeManager>();
        nodeInfoCanvas.SetActive(false);

        wasNodeActived = false;
        wasRoot = false;
        mouseMoveCheck = false;
        nodeShowingStatus = NodeShowingStatus.ShowTotal; // test
        nodeInteractionStatus = NodeInteractionStatus.None;
        
        // ShowTotal();
        currentSelectGroup = tree.mainGroup.transform.GetChild(0).gameObject.GetComponent<Group>();
        ShowFamily(currentSelectGroup);
        AbleKeyInput(currentSelectGroup);
    }

    void Update(){
        // Debug.Log("node Show Status : " + nodeShowingStatus);

        if(!dialogueRunner.IsDialogueRunning 
            && !GameManager.Instance.isFading
            && !cameraControl.mainCam.IsBlending)
        {
            KeyInteract();
            MoveCamera();
        }
    }

    void KeyInteract(){
        int h = (int)Input.GetAxisRaw("Horizontal");
        int v = (int)Input.GetAxisRaw("Vertical");

        if(nodeShowingStatus == NodeShowingStatus.ShowTotal){
            return;
        }
        
        // number
        int index = 0;
        foreach(KeyCode keyCode in AlphakeyCodes){
            if(Input.GetKeyDown(keyCode)){
                if(downGroup != null && downGroup.Count > index-1 && downGroup[index-1] != null)
                {
                    Debug.Log("Down : " + keyCode.ToString());
                    SelectShow(downGroup[index-1]);
                    currentSelectGroup = downGroup[index-1];

                    AbleKeyInput(downGroup[index-1]);
                    return;
                }
            }

            index++;
        }

        if(h != 0 && v != 0){
            return;
        }

        if(h != currentH)
        {
            if(h == -1 && leftGroup != null)
            {
                Debug.Log("Left");
                SelectShow(leftGroup);
                currentSelectGroup = leftGroup;

                AbleKeyInput(leftGroup);
            }
            else if(h == 1 && rightGroup != null)
            {
                Debug.Log("Right");
                SelectShow(rightGroup);
                currentSelectGroup = rightGroup;

                AbleKeyInput(rightGroup);
            }
            
            currentH = h;
        }
        else if(v != currentV)
        {
            if(v == -1 && downGroup != null && downGroup.Count != 0)
            {
                Debug.Log("Down");
                SelectShow(downGroup[0]);
                currentSelectGroup = downGroup[0];

                AbleKeyInput(downGroup[0]);
            }
            else if(v == 1 && upGroup != null)
            {
                Debug.Log("Up");
                SelectShow(upGroup);
                currentSelectGroup = upGroup;
                //전체 그룹 콜라이더 켜기 & 켜져있는 node콜라이더 끄기
                OnAllGroupColliderOffAllNodeCollider();
                AbleKeyInput(upGroup);
            }
            currentV = v;
        }
    }

    void AbleKeyInput(Group newGroup){
        mouseMoveCheck = false;
        leftGroup = null;
        rightGroup = null;
        upGroup = null;
        downGroup = null;

        if(newGroup.childrenGroup != null && newGroup.childrenGroup.Count != 0){
            downGroup = newGroup.childrenGroup;
        }

        if(newGroup.parentGroup != null){
            upGroup = newGroup.parentGroup;
        }

        if(newGroup.parentGroup != null){
            Group parent = newGroup.parentGroup;
            if(parent.childrenGroup != null && parent.childrenGroup.Count > 1){
                int siblingIndex = 0;
                foreach(Group sibling in parent.childrenGroup){
                    if(newGroup == sibling){
                        break;
                    }
                    siblingIndex++;
                }

                if(siblingIndex > 0)
                {
                    leftGroup = parent.childrenGroup[siblingIndex - 1];
                }
                
                if(siblingIndex < parent.childrenGroup.Count - 1)
                {
                    rightGroup = parent.childrenGroup[siblingIndex + 1];
                }
            }
        }
    }

    public void MouseInteract(InteractObjInfo info){
        FamilyTreeType treeType = info._familyTreeType;
        
        if(treeType == FamilyTreeType.Group)
        {
            Group group = info.GetComponent<Group>();

            if(group == null){
                Debug.Log("There is no group script...");
                return;
            }

            // if(nodeShowingStatus != NodeShowingStatus.ShowGroup){
            ShowGroup(group);
            AbleKeyInput(group);
            // }

            // GroupInteract(group);
        }
        else if(treeType == FamilyTreeType.Node)
        {
            NodeDisplay node = info.GetComponent<NodeDisplay>();

            if(node == null){
                Debug.Log("There is no node display...");
                return;
            }

            ShowNodeInfo(node);
        }
        else if(treeType == FamilyTreeType.EmptyNode)
        {
            EmptyDisplay emptyNode = info.GetComponent<EmptyDisplay>();
            if(emptyNode == null){
                Debug.Log("There is no Empty node display...");
                return;
            }
            SelectPair(emptyNode);
        }
        else if(treeType == FamilyTreeType.ChildButton)
        {
            ChildButton childButton = info.GetComponent<ChildButton>();
            ChildAddUI UI = addChildUI.GetComponent<ChildAddUI>();
            UI.Active(childButton.group);
        }
    }
    
    public void MoveCamera(){
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseCamPos = Camera.main.ScreenToViewportPoint(mousePos);

        float temp = 0.025f;
        
        GameObject currentCam = cameraControl.cameraList[cameraControl.cameraList.Count-1];
        CinemachineVirtualCamera camScript = currentCam.GetComponent<CinemachineVirtualCamera>();
        

        if(mouseCamPos.x <= 0)
        {
            camScript.m_Lens.OrthographicSize = Camera.main.orthographicSize;
            camScript.m_Follow = null;
            Vector3 camPos = new Vector3(currentCam.transform.position.x - temp, currentCam.transform.position.y, -10);
            currentCam.transform.position = camPos;
            if(!mouseMoveCheck){
                OnAllGroupColliderOffAllNodeCollider();
                mouseMoveCheck = true;
            }
        }
        else if(mouseCamPos.x >= 1)
        {
            camScript.m_Lens.OrthographicSize = Camera.main.orthographicSize;
            camScript.m_Follow = null;
            Vector3 camPos = new Vector3(currentCam.transform.position.x + temp, currentCam.transform.position.y, -10);
            currentCam.transform.position = camPos;
            if(!mouseMoveCheck){
                OnAllGroupColliderOffAllNodeCollider();
                mouseMoveCheck = true;
            }
        }
        else if(mouseCamPos.y <= 0)
        {
            camScript.m_Lens.OrthographicSize = Camera.main.orthographicSize;
            camScript.m_Follow = null;
            Vector3 camPos = new Vector3(currentCam.transform.position.x, currentCam.transform.position.y - temp, -10);
            currentCam.transform.position = camPos;
            if(!mouseMoveCheck){
                OnAllGroupColliderOffAllNodeCollider();
                mouseMoveCheck = true;
            }
        }
        else if(mouseCamPos.y >= 1)
        {
            camScript.m_Lens.OrthographicSize = Camera.main.orthographicSize;
            camScript.m_Follow = null;
            Vector3 camPos = new Vector3(currentCam.transform.position.x, currentCam.transform.position.y + temp, -10);
            currentCam.transform.position = camPos;
            if(!mouseMoveCheck){
                OnAllGroupColliderOffAllNodeCollider();
                mouseMoveCheck = true;
            }
        }
    }

    void SelectShow(Group group){
        if(group.childrenGroup == null || group.childrenGroup.Count == 0)
        {
            ShowGroup(group);
        }
        else
        {
            ShowFamily(group);
        }
    }

    void ShowTotal(){
        Debug.Log("ShowTotal...");

        if(currentSelectGroup != null){
            EnableNodeCollider(currentSelectGroup, false);
        }
        
        currentSelectGroup = null;
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
        Node node = nodeDisplay.data;


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
    void OnAllGroupColliderOffAllNodeCollider(){
        Group rootGroup = tree.mainGroup.transform.GetChild(0).gameObject.GetComponent<Group>();
        BoxCollider2D boxCollider = rootGroup.GetComponent<BoxCollider2D>();
        boxCollider.enabled = true;
        BoxCollider2D[] nodeColliders = rootGroup.GetComponentsInChildren<BoxCollider2D>();
        foreach(BoxCollider2D collider in nodeColliders){
            if(collider.gameObject != rootGroup.gameObject){
                collider.enabled = false;
            }
        }
        if(rootGroup.childrenGroup != null){
            foreach(Group group in rootGroup.childrenGroup){
                boxCollider = group.GetComponent<BoxCollider2D>();
                boxCollider.enabled = true;
                nodeColliders = group.GetComponentsInChildren<BoxCollider2D>();
                foreach(BoxCollider2D collider in nodeColliders){
                    if(collider.gameObject != group.gameObject){
                        collider.enabled = false;
                    }
                }
                OnAllGroupColliderOffAllNodeCollider(group);
            }
        }
    }
    void OnAllGroupColliderOffAllNodeCollider(Group rootGroup){
        if(rootGroup.childrenGroup != null){
            foreach(Group group in rootGroup.childrenGroup){
                BoxCollider2D boxCollider = group.GetComponent<BoxCollider2D>();
                boxCollider.enabled = true;
                BoxCollider2D[] nodeColliders = group.GetComponentsInChildren<BoxCollider2D>();
                foreach(BoxCollider2D collider in nodeColliders){
                    if(collider.gameObject != group.gameObject){
                        collider.enabled = false;
                    }
                }
                OnAllGroupColliderOffAllNodeCollider(group);
            }
        }
    }
}


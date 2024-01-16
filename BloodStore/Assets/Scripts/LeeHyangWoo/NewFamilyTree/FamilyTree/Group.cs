using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Yarn;
using System.Linq;
using Unity.VisualScripting;
public class Group : MonoBehaviour
{
    public Pair pair;
    public GameObject nodePrefab;
    public GameObject emptyPrefab;
    public Vector2 groupPos;
    public Vector2 leftPos;
    public Vector2 rightPos;

    public GameObject leftDisplay;
    public GameObject rightDisplay;
    private float pairOffSet;
    private float halfX, halfY;
    private float pairSize, unit;
    public float offSetX, offSetY;
    public Group parentGroup;
    public List<Group> childrenGroup;

    public float lineWidth = 0.05f;
    public void SetPrefab(GameObject nodePrefab, GameObject emptyPrefab){
        this.nodePrefab = nodePrefab;
        this.emptyPrefab = emptyPrefab;
    }
    public void SetSizeData(float halfX, float halfY, float pairSize, float unit, float pairOffSet, float offSetX, float offSetY){
        this.halfX = halfX;
        this.halfY = halfY;
        this.pairSize = pairSize;
        this.unit = unit;
        this.pairOffSet = pairOffSet;
        this.offSetX = offSetX;
        this.offSetY = offSetY;
    }
    public Group DisplayNodes(){
        leftDisplay = CreateNode(pair.male);
        leftDisplay.transform.position = leftPos;
        rightDisplay = CreateNode(pair.female);
        rightDisplay.transform.position = rightPos;
        return this;
    }

    public GameObject CreateNode(Node node){
        GameObject display;
        if(!node.empty){
            display = Instantiate(nodePrefab, new Vector2(0, 0), Quaternion.identity);
            NodeDisplay nodeDisplay = display.GetComponent<NodeDisplay>();
            nodeDisplay.SetNodeData(node);
            nodeDisplay.MakeBoxCollider();
            nodeDisplay.DeActiveCollider();
        }
        else{
            display = Instantiate(emptyPrefab, new Vector2(0, 0), Quaternion.identity);
            EmptyDisplay emptyDisplay = display.GetComponent<EmptyDisplay>();
            emptyDisplay.MakeBoxCollider();
            emptyDisplay.DeActiveCollider();
        }
        return display;
    }

    public void MakeBoxCollider(){
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        box.size = new Vector2(pairSize, halfY * 2);
    }
    public void ActiveCollider(){
        BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
        if(box != null){
            box.enabled = true;
        }
    }
    public void DeActiveCollider(){
        BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
        if(box != null){
            box.enabled = false;
        }
    }
    public void CameraSetting(){
        SetCameraTarget camera = gameObject.AddComponent<SetCameraTarget>();
        InteractObjInfo inter = gameObject.AddComponent<InteractObjInfo>();
        
        inter._interactType = InteractType.CameraControl;
        inter._cameraMovementType = CameraControlType.ChangeCamera;
        inter._cameraType = CameraType.TargetGroupCamera;

        if (inter._vertualCam == null) {
            inter._vertualCam = new VirtualCameraInfo();
        }

        if (inter._vertualCam.blendInfo == null) {
            inter._vertualCam.blendInfo = new BlendInfo();
        }
        
        inter._vertualCam.blendInfo.hold = 0.25f;
        inter._vertualCam.blendInfo.blendIn = CinemachineBlendDefinition.Style.EaseInOut;
        inter._vertualCam.blendInfo.blendTime = 0.25f;
    
        camera.SetTarget(SendFamilyList());
    }

    public List<GameObject> SendFamilyList(){
        List<GameObject> familyList = new List<GameObject>();
        familyList.Add(GetGameObject());
        if(childrenGroup != null){
            foreach(Group group in childrenGroup){
                familyList.Add(group.GetGameObject());
            }
        }

        return familyList;
    }
    public GameObject GetGameObject(){
        return gameObject;
    }

    public void PairLine(){
        GameObject pairLine = new("PairLine");
        LineRenderer line = pairLine.AddComponent<LineRenderer>();
        Vector2 globalPos = transform.TransformPoint(gameObject.transform.parent.transform.position);
        pairLine.transform.position = new Vector3(globalPos.x, globalPos.y, 0);
        line.widthMultiplier = lineWidth;
        line.material.color = Color.black;
        Vector3[] points = new Vector3[2];
        points[0] = new Vector3( globalPos.x - pairOffSet / 2, globalPos.y, 0);
        points[1] = new Vector3( globalPos.x + pairOffSet / 2, globalPos.y, 0);
        line.positionCount = points.Count();
        line.SetPositions(points);
        // Debug.Log(" group pos : " + groupPos.ToString());
        // Debug.Log(" line pos : " + globalPos.ToString());
        pairLine.transform.parent = gameObject.transform;
    }
    public void FamilyLine(){
        if(childrenGroup != null){
            foreach(Group group in childrenGroup){
                GameObject pairLine = new("Line");
                LineRenderer line = pairLine.AddComponent<LineRenderer>();
                Vector2 globalPos = transform.TransformPoint(gameObject.transform.parent.transform.position);
                Vector2 globalChildPos = group.transform.position;
                pairLine.transform.position = new Vector3(globalChildPos.x, globalChildPos.y, 0);
                line.widthMultiplier = lineWidth;
                line.material.color = Color.black;
                Vector3[] points = new Vector3[4];
                points[0] = new Vector3( globalPos.x, globalPos.y, 0);
                points[1] = new Vector3( globalPos.x, globalPos.y - halfY - offSetY / 2, 0);
                points[2] = new Vector3( globalChildPos.x, globalChildPos.y + halfY + offSetY / 2, 0);
                points[3] = new Vector3( globalChildPos.x , globalChildPos.y, 0);
                line.positionCount = points.Count();
                line.SetPositions(points);
                // Debug.Log("child pos : " + group.transform.position.ToString());
                // Debug.Log("globalchild pos : " + globalChildPos.ToString());
                // Debug.Log(" line pos : " + globalPos.ToString());
                pairLine.transform.parent = gameObject.transform;
            }
        }
    }
}
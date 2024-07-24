using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Group : MonoBehaviour
{
    public TreePair treePair;
    public FamilyTreePrefabSO familyTreePrefabSO;
    public GameObject selectedCard;
    public Vector2 groupPosition;
    public Vector2 leftNodePosition;
    public Vector2 rightNodePosition;
    public GameObject leftNode;
    public GameObject rightNode;
    public GameObject childButtonOn;
    public GameObject childButtonOff;
    public GameObject highLight;
    public Group parentGroup;
    public List<Group> childGroupList;
    public List<float[]> value;
    public List<string> content;
    public float lineWidth = 0.05f;
    public void SetValues(AddChildSO addChildSO){
        float weight, probability, cost;
        value = new();
        content = new();
        List<int> check = new();
        int i = 0;
        while(i < 3){
            int idx = Random.Range(0, addChildSO.values.Count);
            if(check != null && check.Count != 0){
                bool same = false;
                foreach(int n in check){
                    if(n == idx){
                        same = true;
                        break;
                    }
                }
                if(same){
                    continue;
                }
            }
            weight = addChildSO.values[idx].weight;
            probability = addChildSO.values[idx].probability;
            cost = addChildSO.values[idx].cost;
            check.Add(idx);
            value.Add(new float[3] {weight, probability, cost});
            int sentenceIndex = Random.Range(0,addChildSO.values[idx].sentences.Count);
            content.Add(addChildSO.values[idx].sentences[sentenceIndex] + " : " + cost.ToString());
            i++;
        }
    }
    public void SetUI(GameObject selectedCard){
        this.selectedCard = selectedCard;
    }
    public Group DisplayNodes(){
        leftNode = CreateNode(treePair.pair.male);
        leftNode.transform.position = leftNodePosition;
        rightNode = CreateNode(treePair.pair.female);
        rightNode.transform.position = rightNodePosition;
        return this;
    }

    public GameObject CreateNode(Node node){
        GameObject display;
        if(!node.empty){
            if(node.isDead){
                display = Instantiate(familyTreePrefabSO.treePrefab.deadNodePrefab, new Vector2(0, 0), Quaternion.identity);
                InteractObjInfo inter = display.AddComponent<InteractObjInfo>();
                inter._interactType = InteractType.FamilyTree;
                inter._familyTreeType = FamilyTreeType.Node;
                NodeDisplay nodeDisplay = display.GetComponent<NodeDisplay>();
                nodeDisplay.SetDeadData(node);
                nodeDisplay.MakeBoxCollider();
                BoxCollider2D box = nodeDisplay.gameObject.GetComponent<BoxCollider2D>();
                box.enabled = false;
            }
            else{
                display = Instantiate(familyTreePrefabSO.treePrefab.nodePrefab, new Vector2(0, 0), Quaternion.identity);
                InteractObjInfo inter = display.AddComponent<InteractObjInfo>();
                inter._interactType = InteractType.FamilyTree;
                inter._familyTreeType = FamilyTreeType.Node;
                NodeDisplay nodeDisplay = display.GetComponent<NodeDisplay>();
                nodeDisplay.SetNodeData(node);
                SpriteRenderer spriteRenderer = display.transform.GetChild(0).GetComponent<SpriteRenderer>();
                GameManager.Instance.imageLoad.SetSprite(node.sex, node.imageIdx, spriteRenderer);
                nodeDisplay.MakeBoxCollider();
                BoxCollider2D box = nodeDisplay.gameObject.GetComponent<BoxCollider2D>();
                box.enabled = false;
            }
            
        }
        else{
            display = Instantiate(familyTreePrefabSO.treePrefab.emptyNodePrefab, new Vector2(0, 0), Quaternion.identity);
            InteractObjInfo inter = display.AddComponent<InteractObjInfo>();
            inter._interactType = InteractType.FamilyTree;
            inter._familyTreeType = FamilyTreeType.EmptyNode;
            EmptyDisplay emptyDisplay = display.GetComponent<EmptyDisplay>();
            emptyDisplay.MakeBoxCollider();
            BoxCollider2D box = emptyDisplay.gameObject.GetComponent<BoxCollider2D>();
            box.enabled = false;
        }
        return display;
    }
    public void MakeBoxCollider(){
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        box.size = new Vector2(familyTreePrefabSO.treePrefab.pairLength, familyTreePrefabSO.treePrefab.nodeHalfLength[1] * 2);
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
    public GameObject GetGameObject(){
        return gameObject;
    }

    public void PairLine(){
        GameObject pairLine = new("PairLine");
        LineRenderer line = pairLine.AddComponent<LineRenderer>();
        line.sortingOrder = 4;
        Material material = line.material;
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.EnableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        Vector2 globalPos = transform.TransformPoint(gameObject.transform.parent.transform.position);
        pairLine.transform.position = new Vector3(globalPos.x, globalPos.y, 1);
        line.widthMultiplier = lineWidth;
        line.material.color = Color.black;
        Vector3[] points = new Vector3[2];
        points[0] = new Vector3( globalPos.x - familyTreePrefabSO.treePrefab.nodeOffset / 2, globalPos.y, 1);
        points[1] = new Vector3( globalPos.x + familyTreePrefabSO.treePrefab.nodeOffset / 2, globalPos.y, 1);
        line.positionCount = points.Count();
        line.SetPositions(points);
        // Debug.Log(" group pos : " + groupPos.ToString());
        // Debug.Log(" line pos : " + globalPos.ToString());
        pairLine.transform.parent = gameObject.transform;
    }
    public void FamilyLine(){
        if(childGroupList != null){
            foreach(Group group in childGroupList){
                GameObject pairLine = new("Line");
                LineRenderer line = pairLine.AddComponent<LineRenderer>();
                line.sortingOrder = 4;
                Vector2 globalPos = transform.TransformPoint(gameObject.transform.parent.transform.position);
                Vector2 globalChildPos = group.transform.position;
                pairLine.transform.position = new Vector3(globalChildPos.x, globalChildPos.y, 1);
                line.widthMultiplier = lineWidth;
                line.material = new Material(Shader.Find("Sprites/Default"));
                line.material.color = new Color(0.45f,0.45f,0.45f);
                Vector3[] points = new Vector3[4];
                points[0] = new Vector3( globalPos.x, globalPos.y, 1);
                points[1] = new Vector3( globalPos.x, globalPos.y - familyTreePrefabSO.treePrefab.nodeHalfLength[1] - familyTreePrefabSO.treePrefab.pairOffSetLength[1] / 2, 1);
                points[2] = new Vector3( globalChildPos.x, globalChildPos.y + familyTreePrefabSO.treePrefab.nodeHalfLength[1] + familyTreePrefabSO.treePrefab.pairOffSetLength[1] / 2, 1);
                points[3] = new Vector3( globalChildPos.x , globalChildPos.y, 1);
                line.positionCount = points.Count();
                line.SetPositions(points);
                // Debug.Log("child pos : " + group.transform.position.ToString());
                // Debug.Log("globalchild pos : " + globalChildPos.ToString());
                // Debug.Log(" line pos : " + globalPos.ToString());
                pairLine.transform.parent = gameObject.transform;
            }
        }
    }

    public void MakeChildButton(){
        childButtonOn =  Instantiate(familyTreePrefabSO.treePrefab.childButtonOnPrefab, groupPosition, Quaternion.identity);
        InteractObjInfo inter = childButtonOn.AddComponent<InteractObjInfo>();
        inter._interactType = InteractType.FamilyTree;
        inter._familyTreeType = FamilyTreeType.ChildButton;
        childButtonOff = Instantiate(familyTreePrefabSO.treePrefab.childButtonOffPrefab, groupPosition, Quaternion.identity);
        childButtonOn.SetActive(false);
        childButtonOff.SetActive(false);
        ChildButton childButton = childButtonOn.AddComponent<ChildButton>();
        childButton.group = this;
        BoxCollider2D box = childButtonOn.AddComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = childButtonOn.GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size * 0.8f;
        box.size = spriteSize;
        childButtonOn.transform.parent = transform;
        childButtonOff.transform.parent = transform;
        if(treePair.pair.isPair && treePair.pair.childNum == 0 && treePair.pair.male.age < 60 && treePair.pair.male.age < 60){
            childButtonOn.SetActive(true);
        }
    }
    public void ChangeButton(){
        childButtonOn.SetActive(false);
        childButtonOff.SetActive(true);
    }
    public Vector2[] GetCameraColliderPos(){
        Vector2 pos = transform.position;
        float top = 0, bottom = 0, left = 0, right = 0;
        top = this.transform.TransformPoint(pos).y;
        Group rootGroup = this;
        Group nowGroup = rootGroup;
        if(nowGroup.childGroupList != null){
            while(nowGroup.childGroupList != null && nowGroup.childGroupList.Count != 0){
                nowGroup = nowGroup.childGroupList[0];
            }
            left = this.transform.TransformPoint(nowGroup.transform.position).x;
            nowGroup = rootGroup;
            while(nowGroup.childGroupList != null && nowGroup.childGroupList.Count != 0){
                nowGroup = nowGroup.childGroupList[nowGroup.childGroupList.Count - 1];
            }
            right = this.transform.TransformPoint(nowGroup.transform.position).x;
            bottom = GetBottom(rootGroup);
        }
        top += familyTreePrefabSO.treePrefab.nodeHalfLength[1] * 3f;
        bottom -= familyTreePrefabSO.treePrefab.nodeHalfLength[1] * 3f;
        left -= familyTreePrefabSO.treePrefab.unit;
        right += familyTreePrefabSO.treePrefab.unit;
        Vector2[] colliderPos = new Vector2[]{
            new(left, bottom),
            new(right, bottom),
            new(right, top),
            new(left, top),
        };
        return colliderPos;
    }
    float GetBottom(Group nowGroup){
        float min = this.transform.TransformPoint(nowGroup.transform.position).y;
        if(nowGroup.childGroupList != null){
            foreach(Group group in nowGroup.childGroupList){
                float yPos = this.transform.TransformPoint(group.transform.position).y;
                if(group.childGroupList != null && group.childGroupList.Count != 0){
                    yPos = GetBottom(group);
                }
                if(min > yPos){
                    min = yPos;
                }
            }
        }
        return min;
    }
}
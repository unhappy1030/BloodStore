using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
public class PosTree
{
    public Vector2 centerPos;
    public PosTree parent;
    public GameObject male;
    public GameObject female;
    public List<PosTree> branch;
    private float halfX, halfY;
    public PosTree(){
        branch = new List<PosTree>();
    }
    public void setData(PairSO pairData, GameObject nodePrefab, GameObject emptyPrefab)
    {
        SetPrefabSize(nodePrefab);
        Pair nowPair = pairData.pairs[0];
        SetObject(nowPair, nodePrefab, emptyPrefab, this);
        if(nowPair.childNum != 0)
        {
            int n = nowPair.childNum;
            for (int i = 0; i < n; i++){
                Debug.Log(nowPair.childNum);
                PosTree posTree = new PosTree();
                posTree.SetObject(nowPair.children[i], nodePrefab, emptyPrefab, posTree);
                this.branch.Add(posTree);
            }
        }
    }
    public void SetPrefabSize(GameObject prefab)
    {
        halfX = prefab.GetComponent<SpriteRenderer>().bounds.extents.x;
        halfY = prefab.GetComponent<SpriteRenderer>().bounds.extents.y;
    }
    void SetObject(Pair pair, GameObject nodePrefab, GameObject emptyPrefab, PosTree posTree)
    {
        // Debug.Log("야이병신아");
        if (pair.male.empty == false)
        {
            posTree.male = UnityEngine.Object.Instantiate(nodePrefab, new Vector2(0, 0), Quaternion.identity);
            NodeDisplay nodeDisplay = posTree.male.GetComponent<NodeDisplay>();
            nodeDisplay.SetNodeData(pair, pair.male);
        }
        else
        {
            posTree.male = UnityEngine.Object.Instantiate(emptyPrefab, new Vector2(0, 0), Quaternion.identity);
            EmptyNodeDisplay emptyNodeDisplay = posTree.male.GetComponent<EmptyNodeDisplay>();
            emptyNodeDisplay.SetNodeData(pair);
        }

        if (pair.female.empty == false)
        {
            posTree.female = UnityEngine.Object.Instantiate(nodePrefab, new Vector2(0, 0), Quaternion.identity);
            NodeDisplay nodeDisplay = posTree.female.GetComponent<NodeDisplay>();
            nodeDisplay.SetNodeData(pair, pair.female);
        }
        else
        {
            posTree.female = UnityEngine.Object.Instantiate(emptyPrefab, new Vector2(0, 0), Quaternion.identity);
            EmptyNodeDisplay emptyNodeDisplay = posTree.female.GetComponent<EmptyNodeDisplay>();
            emptyNodeDisplay.SetNodeData(pair);
        }
    }
    public void SetParent()
    {
        PlaceParent();
    }

    public void SetChildren()
    {
        if (branch != null)
        {
            int childNum = branch.Count;
            int idx = 0;
            for (float i = (((float)childNum * 5 / 2) - 2.5f) * -1; i <= (((float)childNum * 5 / 2) - 2.5f); i += 5)
            {
                PlaceChild(branch[idx], i);
                idx++;
            }
        }
    }
    void PlaceParent()
    {
        this.centerPos = new Vector2(0, halfY * 1.3f);
        Vector2 malePos = centerPos - new Vector2(halfX * 1.1f, 0);
        Vector2 femalePos = centerPos + new Vector2(halfX * 1.1f, 0);
        male.transform.position = malePos;
        female.transform.position = femalePos;
    }

    void PlaceChild(PosTree posTree, float x)
    {
        posTree.centerPos = new Vector2(x * halfX, -1 * halfY * 1.3f);
        Vector2 malePos = posTree.centerPos - new Vector2(halfX * 1.1f, 0);
        Vector2 femalePos = posTree.centerPos + new Vector2(halfX * 1.1f, 0);
        posTree.male.transform.position = malePos;
        posTree.female.transform.position = femalePos;
    }
}

public class TreeManager : MonoBehaviour
{
    public CardSO cardData;
    public NodeSO nodeData;
    public PairSO pairData;
    PosTree posTree;
    public GameObject nodePrefab;
    public GameObject EmptyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("TreeManager");
        LoadTree();
    }

    // Update is called once per frame
    // void Update()
    // {

    // }
    void LoadTree()
    {
        // Debug.Log(pairData.pairs.Count);
        if (pairData.pairs.Count == 0)
        {
            Node node = new Node();
            node.SetAllRandom();
            AddFirstNode(node);
        }
        //현재 PairSO에 들어있는 값 위에서 1개의 Family출력
        pairData.pairs[0].SetData(nodePrefab, EmptyPrefab);
        pairData.pairs[0].SetParent();
        pairData.pairs[0].SetChildren();
    }
    public void AddFirstNode(Node node)
    {
        if (node.sex == "Male")
        {
            Pair first = new Pair
            {
                parent = null,
                male = node,
                female = new Node(),
                isPair = false,
            };
            pairData.pairs.Add(first);
        }
        if (node.sex == "Female")
        {
            Pair first = new Pair
            {
                parent = null,
                male = new Node(),
                female = node,
                isPair = false,
            };
            pairData.pairs.Add(first);
        }
    }
}

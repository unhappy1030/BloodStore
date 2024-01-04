using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TreeManager : MonoBehaviour
{
    public NodeSO nodeData;
    public PairSO pairData;
    public GameObject nodePrefab;
    public GameObject emptyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("TreeManager");
        LoadTree();
    }

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
        pairData.pairs[0].SetData(nodePrefab, emptyPrefab);
        pairData.pairs[0].SetParent();
        pairData.pairs[0].SetChildren();
    }
    public void AddFirstNode(Node node)
    {
        if (node.sex == "Male")
        {
            Pair first = new Pair
            {
                parentPair = null,
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
                parentPair = null,
                male = new Node(),
                female = node,
                isPair = false,
            };
            pairData.pairs.Add(first);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreePrefab{
    public GameObject nodePrefab;
    public GameObject emptyNodePrefab;
    public GameObject deadNodePrefab;
    public GameObject childButtonOnPrefab;
    public GameObject childButtonOffPrefab;
    public GameObject highLightPrefab;
    public float[] nodeHalfLength;
    public float[] pairOffSetLength;
    public float nodeOffset; //Pair안의 node와 node 사이의 간격
    public float pairLength; //가로 길이
    public float unit; //Pair길이 + 가로 Offset 하나의 길이
    public void SetLength(){
        nodeHalfLength = new float[2];
        nodeHalfLength[0] = nodePrefab.GetComponent<SpriteRenderer>().bounds.extents.x;
        nodeHalfLength[1] = nodePrefab.GetComponent<SpriteRenderer>().bounds.extents.y;
        pairLength = nodeOffset + (nodeHalfLength[0] * 4);
        unit = pairLength + pairOffSetLength[0];
        Debug.Log("Set Length Complete");
    }
}


[CreateAssetMenu(fileName = "FamilyTreePrefabSO", menuName = "Scriptable Object/FamilyTreePrefabSO")]
public class FamilyTreePrefabSO : ScriptableObject
{
    public TreePrefab treePrefab = new();
}

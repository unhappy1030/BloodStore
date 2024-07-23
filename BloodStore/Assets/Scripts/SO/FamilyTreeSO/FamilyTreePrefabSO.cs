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
}


[CreateAssetMenu(fileName = "FamilyTreePrefabSO", menuName = "Scriptable Object/FamilyTreePrefabSO")]
public class FamilyTreePrefabSO : ScriptableObject
{
    public TreePrefab treePrefab = new();
}

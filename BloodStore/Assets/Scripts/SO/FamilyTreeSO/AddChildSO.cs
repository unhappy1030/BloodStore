using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="AddChildSO", menuName = "Scriptable Object/AddChildSO")]
public class AddChildSO : ScriptableObject
{
    public List<ChildButtonValue> values;
}

[Serializable]
public class ChildButtonValue
{
    public float weight;
    public float probability;
    public float cost; 
    public List<string> sentences;
}
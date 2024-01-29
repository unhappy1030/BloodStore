using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="BloodTasteSO", menuName = "Scriptable Object/BloodTasteSO")]
public class BloodTasteSO : ScriptableObject
{
    public string group;
    public List<Taste> tastes;
}

[Serializable]
public class Taste{
    public string name;
    public List<BloodTasteInfo> tasteInfo;
}

[Serializable]
public class BloodTasteInfo{
    public List<string> words;
    public List<string> sentences;
}

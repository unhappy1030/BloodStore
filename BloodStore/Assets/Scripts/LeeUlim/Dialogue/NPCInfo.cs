using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Info{
    public string NPCName;
    [Tooltip("0 : Normal / 1 : Happy / 2 : Sad / 3 : Angry")]
    public List<SpriteRenderer> sprites;
}

[CreateAssetMenu(fileName ="NPCInfo", menuName = "Scriptable Object/NPCInfo")]
public class NPCInfo : ScriptableObject
{
    public List<Info> infos;
}

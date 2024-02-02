using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="NormalNPCSO", menuName = "Scriptable Object/NormalNPCSO")]
public class NormalNPCSO : ScriptableObject
{
    public List<Sprite> normalNPCSprites;
}

public class NormalNPC{
    public string npcName;
    public Sprite sprite;
}

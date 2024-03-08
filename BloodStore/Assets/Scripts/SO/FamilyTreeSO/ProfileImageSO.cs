using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName ="ProfileImageSO", menuName = "Scriptable Object/ProfileImageSO")]
public class ProfileImageSO : ScriptableObject
{
    public List<ProfileImage> images;
}
[System.Serializable]
public class ProfileImage
{
    public Sprite image;
}

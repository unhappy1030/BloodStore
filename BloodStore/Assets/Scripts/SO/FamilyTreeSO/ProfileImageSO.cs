using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName ="ProfileImageSO", menuName = "Scriptable Object/ProfileImageSO")]
public class ProfileImageSO : ScriptableObject
{
    public List<ProfileImage> images;
    public Sprite GetSprite(string sex){
        Dictionary<int,int> sexIndexDic = GetSeperateImage(sex); // idx : useCount
        float avg = GetAverageCount(sexIndexDic);
        int randomIdx = -1;
        while(randomIdx == -1){
            randomIdx = GetRandomKeyFromDictionary(sexIndexDic);
            if(sexIndexDic[randomIdx] < avg + 2){
                images[randomIdx].useCount++;
            }
            else{
                randomIdx = -1;
            }
        }
        return images[randomIdx].image;
    }
    private Dictionary<int, int> GetSeperateImage(string sex){
        Dictionary<int,int> indexDic = new();
        for(int i = 0; i < images.Count; i++){
            if(images[i].sex == sex){
                indexDic.Add(i, images[i].useCount);
            }
        }
        return indexDic;
    }
    private float GetAverageCount(Dictionary<int,int> sexIndexDic){
        float avg = 0f;
        foreach(int count in sexIndexDic.Keys){
            avg += count;
        }
        avg = avg / sexIndexDic.Count;
        return avg;
    }
    private int GetRandomKeyFromDictionary(Dictionary<int, int> dictionary)
    {
        // 딕셔너리의 키들을 리스트로 변환
        List<int> keys = new List<int>(dictionary.Keys);
        
        // 리스트에서 랜덤하게 인덱스 선택
        int randomIndex = Random.Range(0, keys.Count);

        // 선택된 키 반환
        return keys[randomIndex];
    }
}
[System.Serializable]
public class ProfileImage
{
    public string sex;
    public Sprite image;
    public int useCount;
}

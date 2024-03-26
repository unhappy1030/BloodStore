using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoad : MonoBehaviour
{
    public ProfileImageSO maleProfileImageSO;
    public ProfileImageSO femaleProfileImageSO;
    List<int> maleUseCount;
    List<int> femaleUseCount;
    void Start(){
        maleUseCount = new List<int>(new int[maleProfileImageSO.images.Count]);
        femaleUseCount = new List<int>(new int[femaleProfileImageSO.images.Count]);
    }
    public void SetSprite(string sex ,int idx, SpriteRenderer spriteRenderer){
        if(sex == "Male"){
            spriteRenderer.sprite = maleProfileImageSO.images[idx].image;
        }
        else{
            spriteRenderer.sprite = femaleProfileImageSO.images[idx].image;
        }
        spriteRenderer.transform.localScale = new Vector2(0.9f, 0.9f);
    }
    public void SetSprite(string sex ,int idx, Image image){
        if(sex == "Male"){
            image.sprite = maleProfileImageSO.images[idx].image;
        }
        else{
            image.sprite = femaleProfileImageSO.images[idx].image;
        }
    }
    public void LoadImageUseCount(List<Pair> pairs){
        foreach(Pair pair in pairs){
            if(!pair.male.empty){
                maleUseCount[pair.male.imageIdx]++;
            }
            if(!pair.female.empty){
                femaleUseCount[pair.female.imageIdx]++;
            }
        }
    }
    public int GetSpriteIndex(string sex){
        float avg;
        if(sex == "Male"){
            avg = GetAverageCount(maleUseCount);
            int randomIdx = -1;
            while(randomIdx == -1){
                randomIdx = Random.Range(0, maleUseCount.Count);
                if(maleUseCount[randomIdx] < avg + 2){
                    maleUseCount[randomIdx]++;
                    return randomIdx;
                }
                else{
                    randomIdx = -1;
                }
            }
        }
        else{
            avg = GetAverageCount(femaleUseCount);
            int randomIdx = -1;
            while(randomIdx == -1){
                randomIdx = Random.Range(0, femaleUseCount.Count);
                if(femaleUseCount[randomIdx] < avg + 2){
                    femaleUseCount[randomIdx]++;
                    return randomIdx;
                }
                else{
                    randomIdx = -1;
                }
            }
        }
        return -1;
    }
    private float GetAverageCount(List<int> useCounts){
        float avg = 0f;
        if(useCounts != null && useCounts.Count != 0){
            foreach(int count in useCounts){
                avg += count;
            }
            avg /=useCounts.Count;
        }
        return avg;
    }
}

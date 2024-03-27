using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBackground : MonoBehaviour
{
    public Sprite backgroundRST;
    public Sprite backgroundRFT;
    public void Start(){
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if(GameManager.Instance.lastSceneName == "ResultStore"){
            sprite.sprite = backgroundRST;
        }
        else if(GameManager.Instance.lastSceneName == "ResultFamilyTree"){
            sprite.sprite = backgroundRFT;
        }
    }
}

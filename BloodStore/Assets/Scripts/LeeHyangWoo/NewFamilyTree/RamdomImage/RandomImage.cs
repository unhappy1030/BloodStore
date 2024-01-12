using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomImage : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // 프리팹에 붙어있는 SpriteRenderer
    public List<Sprite> spriteList = new List<Sprite>(); // 사용할 이미지 리스트

    void Start()
    {
        SetRandomImage();
    }

    void SetRandomImage()
    {
        int index = Random.Range(0, spriteList.Count); // 랜덤 인덱스 생성
        spriteRenderer.sprite = spriteList[index]; // 선택된 스프라이트로 설정
        spriteRenderer.transform.localScale = new Vector2(0.35f, 0.22f);
    }
}

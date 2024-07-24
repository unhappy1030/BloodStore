using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeperateImage : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // 프리팹에 붙어있는 SpriteRenderer
    public List<Sprite> maleList = new List<Sprite>(); // 사용할 이미지 리스트
    public List<Sprite> femaleList = new List<Sprite>();

    void Start()
    {
        SetImage();
    }

    void SetImage()
    {
        Node node = transform.parent.GetComponent<NodeDisplay>().data;
        if(node.sex == "Male"){
            int index = Random.Range(0, maleList.Count); // 랜덤 인덱스 생성
            spriteRenderer.sprite = maleList[index]; // 선택된 스프라이트로 설정
            spriteRenderer.transform.localScale = new Vector2(0.6f, 0.6f);
        }
        else{
            int index = Random.Range(0, femaleList.Count); // 랜덤 인덱스 생성
            spriteRenderer.sprite = femaleList[index]; // 선택된 스프라이트로 설정
            spriteRenderer.transform.localScale = new Vector2(0.6f, 0.6f);
        }
    }
}

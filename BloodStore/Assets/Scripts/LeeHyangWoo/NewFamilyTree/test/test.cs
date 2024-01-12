using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject cardPrefab;
    public float offsetX;
    public float offsetY;

    void Start()
    {
        // 가계도의 루트 카드를 생성합니다.
        GameObject rootCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);

        // 자식 카드들을 생성합니다.
        CreateChildCards(rootCard, offsetX, offsetY, 0);
    }

    void CreateChildCards(GameObject parentCard, float offsetX, float offsetY, int depth)
    {
        // 자식 카드의 위치를 계산합니다.
        Vector3 parentPosition = parentCard.transform.position;
        Vector3 leftChildPosition = new Vector3(parentPosition.x - offsetX, parentPosition.y - offsetY, parentPosition.z);
        Vector3 rightChildPosition = new Vector3(parentPosition.x + offsetX, parentPosition.y - offsetY, parentPosition.z);

        // 자식 카드를 생성합니다.
        GameObject leftChildCard = Instantiate(cardPrefab, leftChildPosition, Quaternion.identity);
        GameObject rightChildCard = Instantiate(cardPrefab, rightChildPosition, Quaternion.identity);

        // 일정 깊이에 도달하면 재귀를 종료합니다.
        if (depth < 10)
        {
            // 다음 단계의 자식 카드를 생성합니다.
            CreateChildCards(leftChildCard, offsetX * 0.5f, offsetY, depth + 1);
            CreateChildCards(rightChildCard, offsetX * 0.5f, offsetY, depth + 1);
        }
    }

    
}

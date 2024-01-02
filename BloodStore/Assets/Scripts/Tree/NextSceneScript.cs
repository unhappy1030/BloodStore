using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NextSceneScript : MonoBehaviour
{
    public CardSO cardData;
    public NodeSO nodeData;
    void Start()
    {
        Debug.Log("Start NextSceneScript");
        // 게임 매니저에서 선택된 카드 데이터 가져오기
        Card card = cardData.cards[GameManager.Instance.GetSelectedCard()];

        // Card를 Node로 변환하여 NodeSO의 리스트에 추가
        Node newNode = ConvertCardToNode(card);
        nodeData.nodes.Add(newNode);
    }

    Node ConvertCardToNode(Card card)
    {
        // Card의 정보를 사용하여 Node를 생성
        Node node = new Node
        {
            name = card.name,
            sex = card.sex,
            bloodType = card.bloodType,
            hp = card.hp,
            age = card.age,
            isDead = card.isDead,
            empty = false,
            // 추가적인 변환 규칙이 있다면 여기에 추가
        };
        return node;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public CardSO cardData;
    public GameObject cardPrefab;

    //posX, posY NMN(No Magic Number)수정 -> use Prefab size
    private float[] posX = {-15f, -5f, 5f, 15f};
    private float[] posY = {6f, -6f};
    void Start()
    {
        InstantiateCards();
    }
    void InstantiateCards()
    {
        int i = 0;
        foreach(float Y in posY){
            foreach(float X in posX){
                Vector2 pos = new Vector2(X, Y);
                GameObject cardInstance = Instantiate(cardPrefab, pos, Quaternion.identity);
                Card card = cardData.cards[i];

                CardDisplay cardDisplay = cardInstance.GetComponent<CardDisplay>();

                if (cardDisplay != null)
                {
                    cardDisplay.SetCardData(card, i);
                }
                else
                {
                    Debug.LogError("CardDisplay component not found on the prefab.");
                }
                i++;
            }
        }
    }
}

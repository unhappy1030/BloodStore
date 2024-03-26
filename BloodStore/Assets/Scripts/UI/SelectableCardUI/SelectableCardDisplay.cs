using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class SelectableCardDisplay : MonoBehaviour
{
    
    int idx;
    public GameObject index;
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI sexLabel;
    public TextMeshProUGUI bloodTypeLabel;

    public Color disabledColor; // 비활성화 될 때의 색상
    public bool costCheck = false; // 조건 충족 여부

    // Update is called once per frame
    public void OnButtonClick()
    {
        Index indexManager = index.GetComponent<Index>();
        indexManager.SetIndex(idx);
        Debug.Log(idx);
    }
    public void SetCardData(SelectableCard card, int idx)
    {
        card.SetAllRandom();
        this.idx = idx;
        nameLabel.text = card.name;
        sexLabel.text = card.sex;
        bloodTypeLabel.text = "BloodType : " + card.bloodType[0];
        Image image = transform.GetChild(0).GetComponent<Image>();
        GameManager.Instance.imageLoad.SetSprite(card.sex, card.imageIdx, image);
        CheckCost(card);
    }
    public void CheckCost(SelectableCard card){
        Button button = this.GetComponent<Button>();
        if(card.cost > GameManager.Instance.money){
            SVGImage svg = button.GetComponent<SVGImage>();
            svg.color = disabledColor;
            for(int i = 0; i < gameObject.transform.childCount; i++){
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            costCheck = false;
        }
        else{
            SVGImage svg = button.GetComponent<SVGImage>();
            svg.color = Color.white;
            for(int i = 0; i < gameObject.transform.childCount; i++){
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }
            costCheck = true;
        }
    }
}

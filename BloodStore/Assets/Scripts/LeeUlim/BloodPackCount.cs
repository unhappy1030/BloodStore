using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BloodPackCount : MonoBehaviour
{
    public List<TextMeshProUGUI> texts;
    BloodPacks bloodPacks;

    void Start(){
        bloodPacks = GameManager.Instance.bloodPackList;
        SetCount();
    }
    void SetCount(){
        bloodPacks.UpdateCategory();

        texts[0].text = bloodPacks.categoryNum["Male"].ToString();
        texts[1].text = bloodPacks.categoryNum["Female"].ToString();
        texts[2].text = bloodPacks.categoryNum["A"].ToString();
        texts[3].text = bloodPacks.categoryNum["B"].ToString();
        texts[4].text = bloodPacks.categoryNum["AB"].ToString();
        texts[5].text = bloodPacks.categoryNum["O"].ToString();
        texts[6].text = bloodPacks.categoryNum["-"].ToString();
        texts[7].text = bloodPacks.categoryNum["+"].ToString();
    }
}

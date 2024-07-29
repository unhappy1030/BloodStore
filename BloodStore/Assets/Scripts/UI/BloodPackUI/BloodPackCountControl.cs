using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 혈액팩 선택 UI에서 현재 혈액팩 개수를 업데이트
/// </summary>
public class BloodPackCountControl : MonoBehaviour
{
    public List<TextMeshProUGUI> textList;
    BloodPacks bloodPacks;

    void Start(){
        bloodPacks = GameManager.Instance.bloodPackList;
        SetCount();
    }

    /// <summary>
    /// 현재 혈액팩 개수 업데이트
    /// </summary>
    public void SetCount(){
        bloodPacks.UpdateCategory();

        textList[0].text = bloodPacks.categoryNum["Male"].ToString();
        textList[1].text = bloodPacks.categoryNum["Female"].ToString();
        textList[2].text = bloodPacks.categoryNum["+"].ToString();
        textList[3].text = bloodPacks.categoryNum["-"].ToString();
        textList[4].text = bloodPacks.categoryNum["A"].ToString();
        textList[5].text = bloodPacks.categoryNum["B"].ToString();
        textList[6].text = bloodPacks.categoryNum["AB"].ToString();
        textList[7].text = bloodPacks.categoryNum["O"].ToString();
    }
}

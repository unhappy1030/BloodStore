using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectableCardDisplay : MonoBehaviour
{
    
    int idx;
    public GameObject index;
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI sexLabel;
    public TextMeshProUGUI bloodTypeLabel;

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
    }
}

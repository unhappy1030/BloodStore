using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCardDisplay : MonoBehaviour
{
    
    int idx;
    public GameObject index;
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI sexLabel;
    public TextMeshProUGUI bloodTypeLabel;

    public void SetCardData(Node node, int idx)
    {
        this.idx = idx;
        nameLabel.text = node.name;
        sexLabel.text = node.sex;
        bloodTypeLabel.text = "BloodType : " + node.bloodType[0];
        Image image = transform.GetChild(0).GetComponent<Image>();
        GameManager.Instance.imageLoad.SetSprite(node.sex, node.imageIdx, image);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCardDisplay : MonoBehaviour
{
    
    int idx;
    public GameObject index;
    public TextMeshProUGUI infoLabel;
    public void SetCardData(Node node, int idx)
    {
        this.idx = idx;
        infoLabel.text = node.sex + ", " + node.bloodType[0];
        Image image = transform.GetChild(0).GetComponent<Image>();
        GameManager.Instance.imageLoad.SetSprite(node.sex, node.imageIdx, image);
    }
}


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }
}

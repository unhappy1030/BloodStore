using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeDisplay : MonoBehaviour
{
    public TextMeshPro nameLabel;
    public TextMeshPro sexLabel;
    public TextMeshPro bloodTypeLabel;

    public void SetNodeData(Node node)
    {
        nameLabel.text = node.name;
        sexLabel.text = node.sex;
        bloodTypeLabel.text = "BloodType : " + node.bloodType[0];
    }
}

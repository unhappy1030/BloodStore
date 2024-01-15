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
    public void MakeBoxCollider(){
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        box.size = new Vector2(1, 1);
    }
    public void ActiveCollider(){
        BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
        if(box != null){
            box.enabled = true;
        }
    }
    public void DeActiveCollider(){
        BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
        if(box != null){
            box.enabled = false;
        }
    }
}

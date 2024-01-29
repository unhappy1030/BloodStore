using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeDisplay : MonoBehaviour
{
    public Node data;
    public TextMeshPro textLabel;
    public void SetNodeData(Node node)
    {
        data = node;
        textLabel.text = node.name + "(" + node.sex[0] + "/" + node.bloodType[0] + node.bloodType[1] +")";
    }
    public void SetDeadData(Node node)
    {
        data = node;
    }
    public void MakeBoxCollider(){
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        box.size = spriteSize;
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

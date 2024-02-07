using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Downs : MonoBehaviour
{
    public GameObject downPrefab;
    public GameObject AddDown(int num){
        GameObject downUI = Instantiate(downPrefab,new Vector2(0, 0), Quaternion.identity);
        Down down = downUI.GetComponent<Down>();
        down.numberLabel.text = num.ToString();
        downUI.transform.parent = transform;
        return downUI;
    }
}

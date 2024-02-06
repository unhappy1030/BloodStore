using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Notice : MonoBehaviour
{
    public GameObject NoticeUI;
    public void Confirm(){
        NoticeUI.SetActive(false);
    }
}

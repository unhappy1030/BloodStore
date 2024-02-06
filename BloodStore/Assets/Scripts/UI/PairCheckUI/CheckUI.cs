using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairCheckUI : MonoBehaviour
{
    public GameObject checkPair;
    public GameObject notice;
    void Start()
    {
        if (checkPair.activeSelf){
            checkPair.SetActive(false);
        }
        if(notice.activeSelf){
            notice.SetActive(false);
        }
    }
}

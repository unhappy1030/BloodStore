using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFiltering : MonoBehaviour
{
    public BloodSellProcessManager bloodSellProcess; // assign at inspector

    void Start(){
        gameObject.SetActive(false);
    }

    void OnEnable(){
        StartCoroutine(FilterBlood());
    }

    IEnumerator FilterBlood(){
        yield return new WaitForSeconds(1.5f);
        bloodSellProcess.FilterBlood();
        gameObject.SetActive(false);
    }
}

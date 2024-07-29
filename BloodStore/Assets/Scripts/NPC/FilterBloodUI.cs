using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 혈액 필터링 과정과 UI 관련
/// </summary>
public class FilterBloodUI : MonoBehaviour
{
    public BloodSellProcessManager bloodSellProcess; // assign at inspector

    void Start(){
        gameObject.SetActive(false);
    }

    /// <summary>
    /// UI 켜짐(InteractObjInfo)과 동시에 FilterBlood 코루틴 시작
    /// </summary>
    void OnEnable(){
        StartCoroutine(FilterBlood());
    }

    /// <summary>
    /// 1.5초 기다린 후 자기 자신 setActive false
    /// </summary>
    /// <returns></returns>
    IEnumerator FilterBlood(){
        yield return new WaitForSeconds(1.5f);
        bloodSellProcess.FilterBlood();
        gameObject.SetActive(false);
    }
}

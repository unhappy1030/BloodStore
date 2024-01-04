using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.SceneManagement;

public class BloodPacking : MonoBehaviour
{
    public PairSO pairData;
    public BloodPackSO bloodPackData;
    void Start()
    {
        // Button 컴포넌트 가져오기
        Button button = GetComponent<Button>();

        // Button에 클릭 이벤트 핸들러 등록
        button.onClick.AddListener(HandleButtonClick);
    }
    void HandleButtonClick()
    {
        GetBloodPack(pairData.pairs[0]);
        // SceneManager.LoadScene("SelectCard");
    }
    public void GetBloodPack(Pair nowPair){
        if(nowPair.male.empty == false){
            bloodPackData.AddBloodPack(nowPair.male);
        }
        if(nowPair.female.empty == false){
            bloodPackData.AddBloodPack(nowPair.female);
        }
        if(nowPair.childNum != 0){
            foreach(Pair pair in nowPair.children){
                GetBloodPack(pair);
            }
        }
    }

}
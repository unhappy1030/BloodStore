using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using Unity.VisualScripting;

public class ResultStore : MonoBehaviour
{
    public TextMeshProUGUI earningText; // assign at inspector
    public TextMeshProUGUI spendingText; // assign at inpsector
    public TextMeshProUGUI resultText; // assign at inspector
    public TextMeshProUGUI currentMoneyText; // assign at inspector

    public TextMeshProUGUI pointText; // assign at inspector

    public MoneyControl moneyControl;

    private void Start()
    {
        ChangeStoreResultTexts();
        GameManager.Instance.ableToFade = true;
    }

    public void ChangeStoreResultTexts(){
        earningText.text = "Earning : " + moneyControl.earning.ToString();
        spendingText.text = "Spending : " + moneyControl.spending.ToString();
        resultText.text = "Result : " + (moneyControl.earning + moneyControl.spending).ToString();
        currentMoneyText.text = GameManager.Instance.money.ToString();

        float averagePoint = (GameManager.Instance.sellCount != 0) ? GameManager.Instance.totalPoint/GameManager.Instance.sellCount : 0;
        
        if(GameManager.Instance.day != 1)
        {
            float dist = averagePoint - GameManager.Instance.currentAveragePoint;
            dist = (float)Mathf.Round(dist * 100.0f) / 100.0f;
            string distStr = (dist > 0) ? "+" + dist.ToString() : dist.ToString();
            
            averagePoint = (float)Mathf.Round(averagePoint * 100.0f) / 100.0f;
            pointText.text = "Point : " + averagePoint.ToString() + " (" + distStr +  ")";
        }
        else
        {   
            averagePoint = (float)Mathf.Round(averagePoint * 100.0f) / 100.0f;
            pointText.text = "Point : " + averagePoint.ToString();
        }

        GameManager.Instance.currentAveragePoint = averagePoint;

        moneyControl.ResetEarnAndSpend();
    }
}

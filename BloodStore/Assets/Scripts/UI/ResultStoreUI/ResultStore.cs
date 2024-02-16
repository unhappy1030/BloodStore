using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

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
    }

    public void ChangeStoreResultTexts(){
        earningText.text = "Earning : " + moneyControl.earning.ToString();
        spendingText.text = "Spending : " + moneyControl.spending.ToString();
        resultText.text = "Result : " + (moneyControl.earning + moneyControl.spending).ToString();
        currentMoneyText.text = GameManager.Instance.money.ToString();

        float averagePoint = GameManager.Instance.totalPoint/GameManager.Instance.sellCount;
        
        if(GameManager.Instance.day != 1)
        {
            float dist = averagePoint - GameManager.Instance.currentAveragePoint;
            string distStr = (dist > 0) ? "+" + dist.ToString("F2") : dist.ToString("F2");

            pointText.text = "Point : " + averagePoint.ToString("F2") + " (" + distStr +  ")";
        }
        else
        {
            pointText.text = "Point : " + averagePoint.ToString("F2");
        }

        GameManager.Instance.currentAveragePoint = averagePoint;

        moneyControl.ResetEarnAndSpend();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultMoney : MonoBehaviour
{
    public GameObject resultCanvas; // assign at inspector
    public TextMeshProUGUI earningText; // assign at inspector
    public TextMeshProUGUI spendingText; // assign at inpsector
    public TextMeshProUGUI resultText; // assign at inspector
    public TextMeshProUGUI currentMoneyText; // assign at inspector

    public MoneyControl moneyControl;

    private void Start()
    {
        ChangeStoreResultTexts();
        GameManager.Instance.ableToFade = true;
        
        resultCanvas.SetActive(false);
        StartCoroutine(ShowResultStore());
    }

    IEnumerator ShowResultStore(){
        yield return new WaitForSeconds(2);
        resultCanvas.SetActive(true);
    }

    public void ChangeStoreResultTexts(){
        earningText.text = "Earning : " + moneyControl.earning.ToString();
        spendingText.text = "Spending : " + moneyControl.spending.ToString();
        resultText.text = "Result : " + (moneyControl.earning + moneyControl.spending).ToString();
        currentMoneyText.text = "Current Money : " + GameManager.Instance.money.ToString();

        moneyControl.ResetEarnAndSpend();
    }
}

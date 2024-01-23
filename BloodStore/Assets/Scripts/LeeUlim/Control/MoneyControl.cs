using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MoneyControl : MonoBehaviour
{
    public float money;
    public TextMeshProUGUI moneyText;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetMoney();
        UpdateMoneyUI();
    }

    void OnSceneUnloaded(Scene currentScene)
    {
        SetMoney();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }


    // Check Money Status : dept, bankruptcy ...
    void CheckMoneyStatus(){
        
    }

    void UpdateMoneyUI(){
        moneyText.text = money.ToString();
    }

    public float CalculateMoney(float amount){
        money += amount;
        SetMoney();
        UpdateMoneyUI();
        return money;
    }

    public void GetMoney(){
        money = GameManager.Instance.money;
    }

    public void SetMoney(){
        GameManager.Instance.money = money;
    }
}

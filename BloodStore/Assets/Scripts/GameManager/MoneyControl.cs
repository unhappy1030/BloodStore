using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.Tilemaps;

public class MoneyControl : MonoBehaviour
{
    public float earning;
    public float spending;
    public TextMeshProUGUI moneyText;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMoneyUI();
    }

    void OnSceneUnloaded(Scene currentScene)
    {

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
        moneyText.text = GameManager.Instance.money.ToString();
    }

    public float CalculateMoney(float amount){
        GameManager.Instance.money += amount;

        if(amount > 0)
        {
            earning += amount;
        }
        else
        {
            spending += amount;
        }

        UpdateMoneyUI();
        return GameManager.Instance.money;
    }

    public void ResetEarnAndSpend(){
        earning = 0;
        spending = 0;
    }

}

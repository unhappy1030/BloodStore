using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public GameObject moneyCanvas;
    public GameObject moneyPanel;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI ratingText;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        moneyCanvas.SetActive(true);
        
        if(SceneManager.GetActiveScene().name == "Store" || SceneManager.GetActiveScene().name == "ResultStore" 
            || SceneManager.GetActiveScene().name == "FamilyTree" || SceneManager.GetActiveScene().name == "ResultFamilyTree")
        {
            moneyPanel.SetActive(true);
        }
        else
        {
            moneyPanel.SetActive(false);
        }
        
        moneyText.gameObject.SetActive(false);
        ratingText.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ShowMoneyTextAnimation(float num){
        moneyText.gameObject.SetActive(true);
        moneyText.text = num.ToString();
        moneyText.gameObject.SetActive(false);
    }

    public void ShowRatingTextAnimation(float num){
        ratingText.gameObject.SetActive(true);
        ratingText.text = num.ToString();
        ratingText.color = (num >= 2.5) ? Color.green : Color.red;
        ratingText.gameObject.SetActive(false);
    }
}

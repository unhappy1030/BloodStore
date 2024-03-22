using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.Tilemaps;

public class MoneyUI : MonoBehaviour
{
    public GameObject moneyCanvas;
    public GameObject moneyPanel;
    public GameObject textParent;
    public GameObject moneyText;
    public GameObject ratingText;

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


    IEnumerator Test(){
        yield return new WaitForSeconds(5);
        StartCoroutine(ShowMoneyTextAnimation(300));
        yield return new WaitForSeconds(2);
        StartCoroutine(ShowMoneyTextAnimation(-300));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShowMoneyTextAnimation(-400000000));
    }

    public IEnumerator ShowMoneyTextAnimation(float num){
        GameObject newText = Instantiate(moneyText);
        newText.SetActive(false);
        newText.transform.SetParent(textParent.transform);

        TextMeshProUGUI newTmp = newText.GetComponent<TextMeshProUGUI>();
        if(newTmp == null){
            Debug.Log("There is no TMP in new moneyText...");
            yield break;
        }

        newTmp.text = (num > 0)? "+" + num.ToString("N0") : num.ToString("N0");
        newTmp.color = (num > 0)? Color.green : Color.red;
        Debug.Log("newTmp.color : " + newTmp.color);

        Animator newAnim = newText.GetComponent<Animator>();
        if(newAnim == null){
            Debug.Log("There is no Animator in new moneyText...");
            yield break;
        }

        newText.SetActive(true);
        yield return new WaitForSeconds(2);
        
        newText.SetActive(false);
        Destroy(newText);
    }

    // public void ShowRatingTextAnimation(float num){
    //     ratingText.gameObject.SetActive(true);
    //     ratingText.text = num.ToString();
    //     ratingText.color = (num >= 2.5) ? Color.green : Color.red;
    //     ratingText.gameObject.SetActive(false);
    // }
}

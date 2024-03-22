using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RatingTextControl : MonoBehaviour
{
    public GameObject textParent;
    public GameObject ratingText;

    void Start()
    {
        textParent.SetActive(true);
        ratingText.SetActive(false);
    }

    public IEnumerator ShowRatingTextAnimation(float num){
        GameObject newText = Instantiate(ratingText);
        newText.SetActive(false);
        newText.transform.SetParent(textParent.transform);

        TextMeshProUGUI newTmp = newText.GetComponent<TextMeshProUGUI>();
        if(newTmp == null){
            Debug.Log("There is no TMP in new moneyText...");
            yield break;
        }

        newTmp.text = "Rating : " + num.ToString();
        // newTmp.color = (num > 2.5)? Color.green : Color.red;
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
}

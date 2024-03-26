using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DayShowing : MonoBehaviour
{
    public TextMeshProUGUI dayText;

    private void Start()
    {
        dayText.text = "Day " + GameManager.Instance.day.ToString();
        if(SceneManager.GetActiveScene().name == "ResultStore")
        {
            dayText.text += " Closed...";
        }
    }
}

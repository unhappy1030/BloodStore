using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadStatus : MonoBehaviour
{
    public TextMeshProUGUI pointLabel;
    public TextMeshProUGUI moneyLabel;
    void Start()
    {
        float averagePoint = (GameManager.Instance.sellCount != 0) ? GameManager.Instance.totalPoint/GameManager.Instance.sellCount : 0;
        averagePoint = (float)Mathf.Round(averagePoint * 100.0f) / 100.0f;
        pointLabel.text = "Point : " + averagePoint.ToString();
        moneyLabel.text = "Money : " + GameManager.Instance.money;

        GameManager.Instance.ableToFade = true;
    }
}

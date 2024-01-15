using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle_check : MonoBehaviour
{
    ToggleGroup tg;
    public List<Toggle> ts;
    public List<Toggle> activeToggles;

    void Start()
    {
        tg = GetComponent<UnityEngine.UI.ToggleGroup>();
        ts = new List<Toggle>(tg.GetComponentsInChildren<Toggle>());
    
        foreach(Toggle t in ts)
        {
            t.onValueChanged.AddListener(delegate {ToggleValueChanged(); });
        }
    }

    void ToggleValueChanged()
    {
        activeToggles = new List<Toggle>();
        foreach (Toggle t in ts)
        {
            if (t.isOn)
            {
                activeToggles.Add(t);
                // Debug.Log(t.name);
            }
        }

        string names = "";
        foreach (Toggle t in activeToggles)
        {
            names += t.name + " ";
        }
        Debug.Log(names);
    }
    
}
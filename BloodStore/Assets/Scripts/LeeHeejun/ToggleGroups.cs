using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleGroups : MonoBehaviour
{
    public GameObject parentObject;
    public List<Toggle> activeToggles;
    public BloodPackSO bloodPackSO;
    public TMP_Text NumofFiltered;

    void Start()
    {
        ToggleGroup[] toggleGroups = parentObject.GetComponentsInChildren<ToggleGroup>();

        foreach(ToggleGroup tg in toggleGroups)
        {
            List<Toggle> ts = new List<Toggle>(tg.GetComponentsInChildren<Toggle>());
            foreach(Toggle t in ts)
            {
                t.onValueChanged.AddListener(delegate {ToggleValueChanged(); });
            }
        }
    }

    void ToggleValueChanged()
    {
        int Count = 0;
        activeToggles = new List<Toggle>();
        ToggleGroup[] toggleGroups = parentObject.GetComponentsInChildren<ToggleGroup>();

        foreach (ToggleGroup tg in toggleGroups)
        {
            List<Toggle> ts = new List<Toggle>(tg.GetComponentsInChildren<Toggle>());
            foreach (Toggle t in ts)
            {
                if (t.isOn)
                {
                    activeToggles.Add(t);
                }
            }
        }

        // string names = "";
        // foreach (Toggle t in activeToggles)
        // {
        //     names += t.name + " ";
        // }
        // Debug.Log(names);

        List<BloodPack> filteredBloodPacks = new List<BloodPack>();

        foreach (BloodPack bloodPack in bloodPackSO.bloodPacks)
        {
            bool shouldAdd = false;

            foreach(Toggle t in activeToggles)
            {
                string toggleName = t.name;

                if(toggleName == "Male" || toggleName == "Female")
                {
                    if(toggleName == bloodPack.node.sex)
                    {
                        shouldAdd = true;
                    }
                }
                else if(toggleName == "+" || toggleName == "-")
                {
                    if(toggleName == bloodPack.node.bloodType[1])
                    {
                        shouldAdd = true;
                    }
                }
                else if(toggleName == "A" || toggleName == "B" || toggleName == "AB" || toggleName == "O")
                {
                    if(toggleName == bloodPack.node.bloodType[0])
                    {
                        shouldAdd = true;
                    }
                }

                if (!shouldAdd) continue;
            }

            if (shouldAdd)
            {
                filteredBloodPacks.Add(bloodPack);
                Debug.Log(bloodPack.node.name);
                Count++;
            }
        }

        string a = "재고 : " + Count + " 개";
        NumofFiltered.text = a;

    }

}

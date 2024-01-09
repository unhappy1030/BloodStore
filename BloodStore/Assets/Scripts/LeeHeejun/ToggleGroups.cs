using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroups : MonoBehaviour
{
public GameObject parentObject; // 토글 그룹들을 자식으로 가지는 부모 오브젝트
    public List<Toggle> activeToggles;

    void Start()
    {
        ToggleGroup[] toggleGroups = parentObject.GetComponentsInChildren<ToggleGroup>(); // 부모 오브젝트의 자식 오브젝트 중에서 토글 그룹들을 가져옴

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

        string names = "";
        foreach (Toggle t in activeToggles)
        {
            names += t.name + " ";
        }
        Debug.Log(names);
    }
}

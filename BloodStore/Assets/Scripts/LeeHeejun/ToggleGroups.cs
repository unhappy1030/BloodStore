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
    public BloodPacks bloodPackSO;
    public TMP_Text NumofFiltered;
    public GameObject bloodPackUIPrefab;
    public Transform uiContainer;

    private List<GameObject> uiInstances = new List<GameObject>();

    public List<string> categories = new List<string> { "Sex", "BloodType", "Rh" };
    public List<List<Toggle>> categoryToggles = new List<List<Toggle>>();

    public List<Toggle> FindTogglesByCategory(string categoryName)
    {
        int index = categories.IndexOf(categoryName);

        if (index != -1)
        {
            return categoryToggles[index];
        }
        else
        {
            Debug.LogError("Invalid category name: " + categoryName);
            return null;
        }
    }


    void Start()
    {
        this.bloodPackSO = GameManager.Instance.bloodPackList;
        categoryToggles = new List<List<Toggle>>();

        for (int i = 0; i < categories.Count; i++)
        {
            categoryToggles.Add(new List<Toggle>());
        }

        ToggleGroup[] toggleGroups = parentObject.GetComponentsInChildren<ToggleGroup>();

        foreach (ToggleGroup tg in toggleGroups)
        {
            List<Toggle> ts = new List<Toggle>(tg.GetComponentsInChildren<Toggle>());
            foreach (Toggle t in ts)
            {
                t.onValueChanged.AddListener((bool value) => { if (value) ToggleValueChanged(); });

                string toggleName = t.name;

                if (toggleName == "Male" || toggleName == "Female")
                {
                    categoryToggles[categories.IndexOf("Sex")].Add(t);
                }
                else if (toggleName == "+" || toggleName == "-")
                {
                    categoryToggles[categories.IndexOf("Rh")].Add(t);
                }
                else if (toggleName == "A" || toggleName == "B" || toggleName == "AB" || toggleName == "O")
                {
                    categoryToggles[categories.IndexOf("BloodType")].Add(t);
                }
            }
        }
    }

    public void ToggleValueChanged()
    {
        int Count = 0;
        List<BloodPack> filteredBloodPacks = new List<BloodPack>();

        foreach (BloodPack bloodPack in bloodPackSO.bloodPacks)
        {
            if (IsMatchWithActiveToggles(bloodPack))
            {
                filteredBloodPacks.Add(bloodPack);
                Count++;

                // 필터링된 항목 로그 출력
                // Debug.Log("Filtered BloodPack: " + bloodPack.node.name);
                Debug.Log(bloodPack.node.name);
            }
            // Debug.Log(bloodPack.node.name);
            // Debug.Log(bloodPack.node.sex);
            // Debug.Log(bloodPack.node.bloodType[0]);
            // Debug.Log(bloodPack.node.bloodType[1]);
        }

        // categoryToggles의 각 토글 이름 출력
        // foreach (List<Toggle> toggleList in categoryToggles)
        // {
        //     foreach (Toggle t in toggleList)
        //     {
        //         Debug.Log("Toggle Name: " + t.name);
        //     }
        // }

        // foreach (string category in categories)
        // {
        //     int index = categories.IndexOf(category);
        //     if (index >= 0 && index < categoryToggles.Count)
        //     {
        //         Debug.Log("Category: " + category);
        //         foreach (Toggle t in categoryToggles[index])
        //         {
        //             Debug.Log("Toggle: " + t.name + ", IsOn: " + t.isOn);
        //         }
        //     }
        // }

        string a = "재고 : " + Count + " 개";
        NumofFiltered.text = a;

        StartCoroutine(DisplayBloodPacks(filteredBloodPacks));
    }


    bool IsMatchWithActiveToggles(BloodPack bloodPack)
    {
        bool isSexMatch = CheckCategoryMatch("Sex", bloodPack.node.sex);
        bool isBloodTypeMatch = CheckCategoryMatch("BloodType", bloodPack.node.bloodType[0]);
        bool isRhMatch = CheckCategoryMatch("Rh", bloodPack.node.bloodType[1]);

        Debug.Log(bloodPack.node.sex);
        // Debug.Log(bloodPack.node.bloodType[0]);
        // Debug.Log(bloodPack.node.bloodType[1]);

        return isSexMatch || isBloodTypeMatch || isRhMatch;
    }

    bool CheckCategoryMatch(string categoryName, string bloodPackProperty)
    {
        int categoryIndex = categories.IndexOf(categoryName);

        // 카테고리가 존재하지 않거나, 해당 카테고리의 토글이 없는 경우는 일치하지 않는 것으로 간주
        if(categoryIndex == -1 || categoryToggles[categoryIndex].Count == 0)
            return false;

        // 해당 카테고리의 토글 중에서 활성화되어 있고, 이름이 BloodPack의 속성과 일치하는 것이 있는지 확인
        for (int i = 0; i < categoryToggles[categoryIndex].Count; i++)
        {
            if (categoryToggles[categoryIndex][i].isOn && categoryToggles[categoryIndex][i].name == bloodPackProperty)
            {
                Debug.Log("aaaa");
                return true;
            }
        }
        
        return false;
    }


    IEnumerator DisplayBloodPacks(List<BloodPack> filteredBloodPacks)
    {
        // 기존에 생성된 UI 요소들을 모두 삭제
        foreach (GameObject uiInstance in uiInstances)
        {
            Destroy(uiInstance);
        }

        uiInstances.Clear();  // 리스트를 비웁니다.

        foreach (BloodPack bloodPack in filteredBloodPacks)
        {
            // 프리팹 인스턴스를 생성하면서 부모를 설정
            GameObject newUI = Instantiate(bloodPackUIPrefab, uiContainer);

            yield return null;  // 한 프레임 대기

            // Debug.Log("Node Name: " + bloodPack.node.name);
            // Debug.Log("Node Age: " + bloodPack.node.age);
            // Debug.Log("Node Sex: " + bloodPack.node.sex);
            // Debug.Log("Node Blood Type: " + bloodPack.node.bloodType[0] + bloodPack.node.bloodType[1]);

            // 텍스트 컴포넌트를 찾습니다.
            TMP_Text nameText = newUI.transform.Find("Name").GetComponent<TMP_Text>();
            TMP_Text ageText = newUI.transform.Find("Age").GetComponent<TMP_Text>();
            TMP_Text sexText = newUI.transform.Find("Sex").GetComponent<TMP_Text>();
            TMP_Text bloodTypeText = newUI.transform.Find("Type").GetComponent<TMP_Text>();
            TMP_Text bloodTypeSignText = newUI.transform.Find("Rh").GetComponent<TMP_Text>();

            // 각 텍스트 컴포넌트에 BloodPack 정보를 설정합니다.
            nameText.text = "이름 : " + bloodPack.node.name;
            ageText.text = "나이 : " + bloodPack.node.age.ToString();
            sexText.text = "성별 : " + bloodPack.node.sex;
            bloodTypeText.text = "혈액형 : " + bloodPack.node.bloodType[0];
            bloodTypeSignText.text = "Rh" + bloodPack.node.bloodType[1];

            uiInstances.Add(newUI); 
        }
    }

    // 모든 토글 비활성화(리셋)
    public void DeactivateAllToggles()
    {
        ToggleGroup[] toggleGroups = parentObject.GetComponentsInChildren<ToggleGroup>();

        foreach(ToggleGroup tg in toggleGroups)
        {
            List<Toggle> ts = new List<Toggle>(tg.GetComponentsInChildren<Toggle>());
            foreach(Toggle t in ts)
            {
                t.isOn = false;
            }
        }
    }
}

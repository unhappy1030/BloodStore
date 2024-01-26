using System;
using System.Linq;
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
    List<BloodPack> filteredBloodPacks;

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
        bloodPackSO.Load();
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
                t.onValueChanged.AddListener(ToggleValueChanged);
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

        filteredBloodPacks = new List<BloodPack>();

        foreach (BloodPack bloodPack in bloodPackSO.bloodPacks)
        {
            Debug.Log(bloodPack.node.name);
            filteredBloodPacks.Add(bloodPack);
        }
        StartCoroutine(DisplayBloodPacks(filteredBloodPacks));
        
    }


    public void ToggleValueChanged(bool value)
    {
        filteredBloodPacks = new List<BloodPack>();

        activeToggles = FindActiveTogglesInActiveCategories();

            foreach (BloodPack bloodPack in bloodPackSO.bloodPacks)
            {
                bool satisfiesAllConditions = true;

                // 성별에 해당하는 토글이 활성화되어 있는지 확인
                List<Toggle> sexToggles = categoryToggles[categories.IndexOf("Sex")];
                if (sexToggles.Any(toggle => toggle.isOn) && !sexToggles.Any(toggle => toggle.isOn && toggle.name == bloodPack.node.sex))
                {
                    satisfiesAllConditions = false;
                }

                // 혈액형에 해당하는 토글이 활성화되어 있는지 확인
                List<Toggle> bloodTypeToggles = categoryToggles[categories.IndexOf("BloodType")];
                if (bloodTypeToggles.Any(toggle => toggle.isOn) && !bloodTypeToggles.Any(toggle => toggle.isOn && toggle.name == bloodPack.node.bloodType[0]))
                {
                    satisfiesAllConditions = false;
                }

                // Rh에 해당하는 토글이 활성화되어 있는지 확인
                List<Toggle> rhToggles = categoryToggles[categories.IndexOf("Rh")];
                if (rhToggles.Any(toggle => toggle.isOn) && !rhToggles.Any(toggle => toggle.isOn && toggle.name == bloodPack.node.bloodType[1]))
                {
                    satisfiesAllConditions = false;
                }

                // 모든 토글 조건을 만족하면 필터링된 목록에 추가
                if(satisfiesAllConditions)
                {
                    filteredBloodPacks.Add(bloodPack);
                }

        }

        Debug.Log("aa");
        StartCoroutine(DisplayBloodPacks(filteredBloodPacks));
    }

    bool IsCategorySelected(string category)
    {
        foreach (Toggle toggle in activeToggles)
        {
            if (toggle.name.Contains(category))
            {
                return true;
            }
        }
        return false;
    }

    public List<Toggle> FindActiveTogglesInActiveCategories()
    {
        List<Toggle> activeToggles = new List<Toggle>();

        // 카테고리별로 확인
        foreach (string category in categories)
        {
            // 카테고리가 활성화되었는지 확인
            if (IsCategorySelected(category))
            {
                // 해당 카테고리에서 활성화된 토글들을 찾음
                List<Toggle> togglesInCategory = FindTogglesByCategory(category);
                foreach (Toggle toggle in togglesInCategory)
                {
                    if (toggle.isOn)
                    {
                        activeToggles.Add(toggle);
                    }
                }
            }
        }

        return activeToggles;
    }

    IEnumerator DisplayBloodPacks(List<BloodPack> filteredBloodPacks)
    {
        Debug.Log("bbbb");
        string a = "재고 : " + filteredBloodPacks.Count + " 개";
        NumofFiltered.text = a;
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

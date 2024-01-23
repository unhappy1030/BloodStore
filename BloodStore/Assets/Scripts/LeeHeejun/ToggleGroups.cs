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

    void Start()
    {
        this.bloodPackSO = GameManager.Instance.bloodPackList;
        ToggleGroup[] toggleGroups = parentObject.GetComponentsInChildren<ToggleGroup>();

        foreach (ToggleGroup tg in toggleGroups)
        {
            List<Toggle> ts = new List<Toggle>(tg.GetComponentsInChildren<Toggle>());
            foreach (Toggle t in ts)
            {
                t.onValueChanged.AddListener(delegate { ToggleValueChanged(); });
            }
        }
        ToggleValueChanged();
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

        List<BloodPack> filteredBloodPacks = new List<BloodPack>();

        foreach (BloodPack bloodPack in bloodPackSO.bloodPacks)
        {
            bool isSexMatch = false;
            bool isBloodTypeSignMatch = false;
            bool isBloodTypeLetterMatch = false;

            bool sexToggleSelected = false;
            bool bloodTypeSignToggleSelected = false;
            bool bloodTypeLetterToggleSelected = false;

            foreach (Toggle t in activeToggles)
            {
                string toggleName = t.name;

                if (toggleName == "Male" || toggleName == "Female")
                {
                    sexToggleSelected = true;
                    if (toggleName == bloodPack.node.sex)
                    {
                        isSexMatch = true;
                    }
                }
                else if (toggleName == "+" || toggleName == "-")
                {
                    bloodTypeSignToggleSelected = true;
                    if (toggleName == bloodPack.node.bloodType[1])
                    {
                        isBloodTypeSignMatch = true;
                    }
                }
                else if (toggleName == "A" || toggleName == "B" || toggleName == "AB" || toggleName == "O")
                {
                    bloodTypeLetterToggleSelected = true;
                    if (toggleName == bloodPack.node.bloodType[0])
                    {
                        isBloodTypeLetterMatch = true;
                    }
                }
            }

            // 선택된 토글이 있을 경우에만 검사하고, 그 토글에 맞는 경우에만 추가
            if ((!sexToggleSelected || isSexMatch) && (!bloodTypeSignToggleSelected || isBloodTypeSignMatch) && (!bloodTypeLetterToggleSelected || isBloodTypeLetterMatch))
            {
                filteredBloodPacks.Add(bloodPack);
                Debug.Log(bloodPack.node.name);
                Count++;
            }
        }

        string a = "재고 : " + Count + " 개";
        NumofFiltered.text = a;

        StartCoroutine(DisplayBloodPacks(filteredBloodPacks));
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

            Debug.Log("Node Name: " + bloodPack.node.name);
            Debug.Log("Node Age: " + bloodPack.node.age);
            Debug.Log("Node Sex: " + bloodPack.node.sex);
            Debug.Log("Node Blood Type: " + bloodPack.node.bloodType[0] + bloodPack.node.bloodType[1]);

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

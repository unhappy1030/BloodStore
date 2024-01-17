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
    public GameObject bloodPackUIPrefab;  
    public Transform uiContainer;

    private List<GameObject> uiInstances = new List<GameObject>();

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

        List<BloodPack> filteredBloodPacks = new List<BloodPack>();

        foreach (BloodPack bloodPack in bloodPackSO.bloodPacks)
        {
            bool isSexMatch = false;
            bool isBloodTypeSignMatch = false;
            bool isBloodTypeLetterMatch = false;

            bool sexToggleSelected = false;
            bool bloodTypeSignToggleSelected = false;
            bool bloodTypeLetterToggleSelected = false;

            foreach(Toggle t in activeToggles)
            {
                string toggleName = t.name;

                if(toggleName == "Male" || toggleName == "Female")
                {
                    sexToggleSelected = true;
                    if(toggleName == bloodPack.node.sex)
                    {
                        isSexMatch = true;
                    }
                }
                else if(toggleName == "+" || toggleName == "-")
                {
                    bloodTypeSignToggleSelected = true;
                    if(toggleName == bloodPack.node.bloodType[1])
                    {
                        isBloodTypeSignMatch = true;
                    }
                }
                else if(toggleName == "A" || toggleName == "B" || toggleName == "AB" || toggleName == "O")
                {
                    bloodTypeLetterToggleSelected = true;
                    if(toggleName == bloodPack.node.bloodType[0])
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
            // 프리팹 인스턴스를 생성
            GameObject newUI = Instantiate(bloodPackUIPrefab);
            yield return null;  // 한 프레임 대기

            // 인스턴스를 컨테이너의 자식으로 설정
            newUI.transform.SetParent(uiContainer, false);
            
            // 인스턴스 내부의 Text 요소들을 찾아서 BloodPack 정보로 설정
            Text[] texts = newUI.GetComponentsInChildren<Text>();
            
            if(texts.Length >= 5)  // Text 컴포넌트가 충분한지 확인
            {
                texts[0].text = bloodPack.node.name;
                texts[1].text = bloodPack.node.age.ToString();
                texts[2].text = bloodPack.node.sex;
                texts[3].text = bloodPack.node.bloodType[0];
                texts[4].text = bloodPack.node.bloodType[1];
            }
            else
            {
                Debug.LogError("Insufficient Text components in the prefab.");
            }

            uiInstances.Add(newUI);  // 생성한 인스턴스를 리스트에 추가합니다.
        }
    }
}

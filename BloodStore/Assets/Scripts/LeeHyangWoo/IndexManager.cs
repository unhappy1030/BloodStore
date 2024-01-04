using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexManger : MonoBehaviour
{
    private static IndexManger instance;

    // 현재 선택된 카드 데이터를 저장할 변수
    private int idx;
    private int dayCount;

    public static IndexManger Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("IndexManger");
                instance = go.AddComponent<IndexManger>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    // 다음 씬에서 사용할 카드 데이터를 설정하는 함수
    public void SetSelectedCard(int idx)
    {
        this.idx = idx;
    }

    // 현재 선택된 카드 데이터를 가져오는 함수
    public int GetSelectedCard()
    {
        return idx;
    }
    public void DayCountUpdate(){
        dayCount++;
    }
    public int GetDayCount(){
        return dayCount;
    }
}

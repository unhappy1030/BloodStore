using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // 현재 선택된 카드 데이터를 저장할 변수
    private int idx;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
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
}

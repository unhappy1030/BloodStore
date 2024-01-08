using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle_check : MonoBehaviour
{
    public Toggle toggle;

    void Start()
    {
        // 토글의 상태 변경 이벤트에 함수 연결
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void OnToggleValueChanged(bool isToggled)
    {
        // 토글 상태에 따라 실행할 동작을 여기에 추가
        if (isToggled)
        {
            Debug.Log("토글이 켜졌습니다.");
            // 켜진 상태에서 수행할 동작 추가
        }
        else
        {
            Debug.Log("토글이 꺼졌습니다.");
            // 꺼진 상태에서 수행할 동작 추가
        }
    }
}

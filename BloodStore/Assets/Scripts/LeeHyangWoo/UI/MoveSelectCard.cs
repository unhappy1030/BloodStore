using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveSelectCard : MonoBehaviour
{
    void Start()
    {
        // Button 컴포넌트 가져오기
        Button button = GetComponent<Button>();

        // Button에 클릭 이벤트 핸들러 등록
        button.onClick.AddListener(HandleButtonClick);
    }

    void HandleButtonClick()
    {
        // 버튼이 클릭되었을 때 수행할 동작
        SceneManager.LoadScene("SelectCard");
    }
}

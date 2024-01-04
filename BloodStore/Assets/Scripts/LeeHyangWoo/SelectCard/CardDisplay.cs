using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CardDisplay : MonoBehaviour
{   
    int idx;
    public TextMeshPro nameLabel;
    public TextMeshPro sexLabel;
    public TextMeshPro bloodTypeLabel;
    public TextMeshPro hpLabel;
    public TextMeshPro ageLabel;
    void Update()
    {
        // 마우스 왼쪽 버튼이 클릭되었을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치를 2D 좌표로 변환
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 마우스 위치에 collider가 있는지 확인
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            // collider가 있다면 클릭된 것으로 간주
            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                // 현재 선택된 카드를 게임 매니저에 저장
                IndexManger.Instance.SetSelectedCard(idx);

                // 다음 씬으로 이동
                SceneManager.LoadScene("FamilyTree");
            }
        }
    }
    public void SetCardData(Card card, int idx)
    {
        card.SetAllRandom();
        this.idx = idx;
        nameLabel.text = "Name: " + card.name;
        sexLabel.text = "Sex: " + card.sex;
        bloodTypeLabel.text = "Blood Type: " + string.Join(", ", card.bloodType[0]);
        hpLabel.text = "HP: " + card.hp.ToString();
        ageLabel.text = "Age: " + card.age.ToString();
    }
}

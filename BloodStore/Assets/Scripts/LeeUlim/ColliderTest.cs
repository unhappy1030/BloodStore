using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    // Warning! : object should have collider2D for bounds

    void Update(){
        Check();
    }


    void Check(){ 
        Vector2 boxPosition = transform.position; // 상자의 위치
        Vector2 boxSize = GetComponent<Collider2D>().bounds.size; // 상자의 크기

        Collider2D[] results = new Collider2D[10]; // 최대 10개의 겹치는 물체를 찾도록 설정
        
        // 자신의 Collider2D를 잠시 비활성화
        Collider2D myCollider = GetComponent<Collider2D>();
        myCollider.enabled = false;

        int numberOfColliders = Physics2D.OverlapBoxNonAlloc(boxPosition, boxSize, 0f, results);

        // 검사가 끝났으니 다시 활성화
        myCollider.enabled = true;


        if (numberOfColliders > 0)
        {
            for (int i = 0; i < numberOfColliders; i++)
            {
                Debug.Log(gameObject + " : " + results[i].name + " 물체가 겹쳐 있습니다.");
            }
        }
        else
        {
            Debug.Log("물체가 겹쳐 있지 않습니다.");
        }

    }


    // // 다른 GameObject의 참조 변수
    // public GameObject otherObject;

    // void Update()
    // {
    //     // 현재 GameObject와 다른 GameObject의 BoxCollider2D를 가져옴
    //     BoxCollider2D boxCollider1 = GetComponent<BoxCollider2D>();
    //     BoxCollider2D boxCollider2 = otherObject.GetComponent<BoxCollider2D>();

    //     // Collider가 존재하면 Rect를 생성하고 확인
    //     if (boxCollider1 != null && boxCollider2 != null)
    //     {
    //         // Collider의 크기를 가져옴
    //         Vector2 colliderSize1 = boxCollider1.size;
    //         Vector2 colliderSize2 = boxCollider2.size;

    //         // 현재 GameObject와 다른 GameObject의 위치를 가져옴
    //         Vector2 objectPosition1 = (Vector2)transform.position;
    //         Vector2 objectPosition2 = (Vector2)otherObject.transform.position;

    //         // Rect 생성
    //         Rect rect1 = new Rect(objectPosition1.x - colliderSize1.x / 2, 
    //                               objectPosition1.y - colliderSize1.y / 2, 
    //                               colliderSize1.x, colliderSize1.y);

    //         Rect rect2 = new Rect(objectPosition2.x - colliderSize2.x / 2, 
    //                               objectPosition2.y - colliderSize2.y / 2, 
    //                               colliderSize2.x, colliderSize2.y);

    //         // Rect 사용 예시: Rect와 다른 Rect가 겹치는지 확인
    //         Rect intersection = Rect.MinMaxRect(
    //             Mathf.Max(rect1.xMin, rect2.xMin),
    //             Mathf.Max(rect1.yMin, rect2.yMin),
    //             Mathf.Min(rect1.xMax, rect2.xMax),
    //             Mathf.Min(rect1.yMax, rect2.yMax)
    //         );

    //         if (intersection.width > 0 && intersection.height > 0)
    //         {
    //             Debug.Log("두 Rect가 겹칩니다.");
    //         }
    //         else
    //         {
    //             Debug.Log("두 Rect가 겹치지 않습니다.");
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError("BoxCollider2D를 찾을 수 없습니다.");
    //     }
    // }
}

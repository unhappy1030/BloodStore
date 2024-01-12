// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class EmptyNodeDisplay : MonoBehaviour
// {
//     public NodeSO nodeData;
//     private Pair pair;
//     void Update()
//     {
//         // 마우스 왼쪽 버튼이 클릭되었을 때
//         if (Input.GetMouseButtonDown(0))
//         {
//             // 마우스 위치를 2D 좌표로 변환
//             Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//             // 마우스 위치에 collider가 있는지 확인
//             Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

//             // collider가 있다면 클릭된 것으로 간주
//             if (hitCollider != null && hitCollider.gameObject == gameObject)
//             {
//                 if(pair.BlankNodeCheck() == nodeData.node.sex){
//                     MakePair();
//                     pair.AddChild();
//                 }
//                 else{
//                     DeleteNode();
//                 }
//             }
//         }
//     }
//     public void SetNodeData(Pair pair){
//         this.pair = pair;
//     }
//     void MakePair(){
//         Node node = new Node();
//         node = nodeData.node;
//         pair.MakePair(node);
//         pair.isPair = true;
//     }
//     void DeleteNode(){
//         pair.isPair = false;
//     }
// }

// using System.Collections;
// using System.Collections.Generic;
// using System.Runtime.InteropServices;
// using Unity.Jobs;
// using UnityEngine;
// using UnityEngine.Animations;

// [System.Serializable]
// public class Pair
// {
//     public Node male;
//     public Node female;
//     public bool isPair;
//     public int childNum;
//     public bool isParent;
//     public PairTree Deserialize(){
//         return new PairTree(this);
//     }
// }
// public class PairTree
// {
//     public Pair pair;
//     public PairTree parent;
//     public List<PairTree> children;
//     public PairTree(Pair data){
//         this.children = new();
//         this.pair = data;
//     }
//     public void AddParent(PairTree parent){
//         this.parent = parent;
//     }
//     public void AddChild(PairTree child){
//         this.children.Add(child);
//     }
// }

// [CreateAssetMenu(fileName = "PairSo", menuName = "Scriptable Object/PairSo")]
// public class PairSO : ScriptableObject
// {
//     public List<Pair> pairs = new();
//     public void SerializeFromTree(PairTree pairTree)
//     {
//         pairs.Clear();
//         Local_SerializeAll(pairTree);
//         void Local_SerializeAll(PairTree current)
//         {
//             // 직렬화용 노드 생성, 리스트에 추가
//             pairs.Add(current.pair);

//             foreach(PairTree child in current.children)
//             {
//                 Local_SerializeAll(child);
//             }
//         }
//     }
//     public PairTree Deserialize()
//     {
//         if (pairs.Count == 0) return null;

//         int index = 0;
//         PairTree root = Local_DeserializeAll();

//         return root;

//         // 재귀 : 루트로부터 모든 자식들 역직렬화 및 트리 생성
//         PairTree Local_DeserializeAll()
//         {
//             int currentIndex = index;
//             PairTree current = pairs[currentIndex].Deserialize();

//             index++;

//             // 자식이 있을 경우, 자식을 역직렬화해서 자식목록에 추가
//             // 그리고 다시 그 자식에서 재귀
//             for (int i = 0; i < pairs[currentIndex].childNum; i++)
//             {
//                 current.AddChild(Local_DeserializeAll());
//                 current.children[i].AddParent(current);
//             }

//             return current;
//         }
//     }
//     public void MakeOlder(){
//         foreach(Pair pair in pairs){
//             if(!pair.male.empty){
//                 pair.male.age += 10;
//             }
//             if(!pair.female.empty){
//                 pair.female.age += 10;
//             }
//         }
//     }
// }

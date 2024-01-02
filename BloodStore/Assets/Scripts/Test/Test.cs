// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [System.Serializable]
// public class PosTree
// {
//     public PosTree parent;
//     public GameObject male;
//     public GameObject female;
//     public List<PosTree> children;

//     public void setData(GameObject prefab, int n)
//     {
//         male = prefab;
//         female = prefab;
//         children = new List<PosTree>();
//         for (int i = 0; i < n; i++)
//         {
//             PosTree posTree =  new PosTree{
//                 parent = this,
//                 male = prefab,
//                 female = prefab,
//                 children = new List<PosTree>(),
//             };
//             children.Add(posTree);
//         }
//     }

//     public void SetParent()
//     {
//         PlaceParent();
//     }

//     public void SetChildren()
//     {
//         int childNum = children.Count;
//         int idx = 0;
//         for (float i = (((float)childNum * 5 / 2) - 2.5f) * -1 ; i <= (((float)childNum * 5 / 2) - 2.5f) ; i += 5)
//         {
//             PlaceChild(children[idx].male, children[idx].female, i);
//             idx++;
//         }
//     }


//     void PlaceParent(){
//         float halfX = male.GetComponent<SpriteRenderer>().bounds.extents.x;
//         float halfY = male.GetComponent<SpriteRenderer>().bounds.extents.y;
//         Vector2 centerPos = new Vector2(0,halfY * 1.3f);
//         Vector2 malePos = centerPos - new Vector2(halfX * 1.1f,0);
//         Vector2 femalePos = centerPos + new Vector2(halfX * 1.1f,0);
//         GameObject personInstance1 = Object.Instantiate(male, malePos, Quaternion.identity);
//         GameObject personInstance2 = Object.Instantiate(female, femalePos, Quaternion.identity);
//     }

//     void PlaceChild(GameObject male, GameObject female, float x){
//         float halfX = male.GetComponent<SpriteRenderer>().bounds.extents.x;
//         float halfY = male.GetComponent<SpriteRenderer>().bounds.extents.y;
//         Vector2 centerPos = new Vector2(x * halfX,-1 * halfY * 1.3f);
//         Vector2 malePos = centerPos - new Vector2(halfX * 1.1f,0);
//         Vector2 femalePos = centerPos + new Vector2(halfX * 1.1f,0);
//         GameObject personInstance1 = Object.Instantiate(male, malePos, Quaternion.identity);
//         GameObject personInstance2 = Object.Instantiate(female, femalePos, Quaternion.identity);
//     }
// }
// public class Test : MonoBehaviour
// {
//     public GameObject prefab;
//     public int n;
//     void Start()
//     {
//         TestPosTree();
//     }

//     void TestPosTree()
//     {
//         PosTree posTree =  new PosTree();
//         posTree.setData(prefab, n);
//         posTree.SetParent();
//         posTree.SetChildren();
//     }
// }

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
//     [System.NonSerialized]public Pair parentPair;
//     public List<Pair> children;
//     public Vector2 centerPos;
//     public GameObject maleDP;
//     public GameObject femaleDP;
//     public bool isParent;


//     public void MakeFamilyTree(){

//     }

//     // public void SetData(GameObject nodePrefab, GameObject emptyPrefab)
//     // {
//     //     SetPrefabSize(nodePrefab);
//     //     SetObject(this, nodePrefab, emptyPrefab);
//     //     if(childNum != 0)
//     //     {
//     //         int n = childNum;
//     //         for (int i = 0; i < n; i++){
//     //             children[i].parentPair = this;
//     //             SetObject(children[i], nodePrefab, emptyPrefab);
//     //         }
//     //     }
//     // }
//     // public void SetDataView(GameObject nodePrefab, GameObject emptyPrefab)
//     // {
//     //     SetPrefabSize(nodePrefab);
//     //     if(childNum != 0)
//     //     {
//     //         int n = childNum;
//     //         for (int i = 0; i < n; i++){
//     //             children[i].parentPair = this;
//     //             SetObject(children[i], nodePrefab, emptyPrefab);
//     //         }
//     //     }
//     // }
//     // public void SetPrefabSize(GameObject prefab)
//     // {
//     //     halfX = prefab.GetComponent<SpriteRenderer>().bounds.extents.x;
//     //     halfY = prefab.GetComponent<SpriteRenderer>().bounds.extents.y;
//     // }
//     // void SetObject(Pair pair, GameObject nodePrefab, GameObject emptyPrefab)
//     // {
//     //     if (pair.male.empty == false)
//     //     {
//     //         pair.maleDP = Object.Instantiate(nodePrefab, new Vector2(0, 0), Quaternion.identity);
//     //         NodeDisplay nodeDisplay = pair.maleDP.GetComponent<NodeDisplay>();
//     //         // Debug.Log(  "<pair.male>\n" +
//     //         //             "name : " + pair.male.name + "\n" +
//     //         //             "sex : " + pair.male.sex);
//     //         nodeDisplay.SetNodeData(pair, pair.male);
//     //     }
//     //     else
//     //     {
//     //         pair.maleDP = Object.Instantiate(emptyPrefab, new Vector2(0, 0), Quaternion.identity);
//     //         EmptyNodeDisplay emptyNodeDisplay = pair.maleDP.GetComponent<EmptyNodeDisplay>();
//     //         emptyNodeDisplay.SetNodeData(pair);
//     //     }

//     //     if (pair.female.empty == false)
//     //     {
//     //         pair.femaleDP = Object.Instantiate(nodePrefab, new Vector2(0, 0), Quaternion.identity);
//     //         NodeDisplay nodeDisplay = pair.femaleDP.GetComponent<NodeDisplay>();
//     //         // Debug.Log(  "<pair.female>\n" +
//     //         //             "name : " + pair.female.name + "\n" +
//     //         //             "sex : " + pair.female.sex);
//     //         nodeDisplay.SetNodeData(pair, pair.female);
//     //     }
//     //     else
//     //     {
//     //         pair.femaleDP = Object.Instantiate(emptyPrefab, new Vector2(0, 0), Quaternion.identity);
//     //         EmptyNodeDisplay emptyNodeDisplay = pair.femaleDP.GetComponent<EmptyNodeDisplay>();
//     //         emptyNodeDisplay.SetNodeData(pair);
//     //     }
//     // }
//     // public void SetParent()
//     // {
//     //     isParent = true;
//     //     PlaceParent();
//     // }
//     // public void SetChildren(){
//     //     if (childNum != 0)
//     //     {
//     //         int idx = 0;
//     //         for (float i = (((float)childNum * 5 / 2) - 2.5f) * -1; i <= (((float)childNum * 5 / 2) - 2.5f); i += 5)
//     //         {
//     //             children[idx].isParent = false;
//     //             PlaceChild(children[idx], i);
//     //             idx++;
//     //         }
//     //     }
//     // }
//     // public void SetActiveParent(){
//     //     isParent = true;
//     //     ActivePair();
//     // }
//     // public void SetActivehildren(){
//     //     if (childNum != 0)
//     //     {
//     //         for(int i = 0; i < childNum; i++){
//     //             children[i].isParent = false;
//     //             children[i].ActivePair();
//     //         }
//     //     }
//     // }
//     // void PlaceParent()
//     // {
//     //     centerPos = new Vector2(0, halfY * 1.3f);
//     //     Vector2 malePos = centerPos - new Vector2(halfX * 1.1f, 0);
//     //     Vector2 femalePos = centerPos + new Vector2(halfX * 1.1f, 0);
//     //     maleDP.transform.position = malePos;
//     //     femaleDP.transform.position = femalePos;
//     // }
//     // void PlaceChild(Pair pair, float x)
//     // {
//     //     // Debug.Log("PlaceChild Pos: (" + pair.parent.centerPos.x.ToString() + ", " + pair.parent.centerPos.y.ToString());
//     //     // Debug.Log( "pair.parentPair : "+ (pair.parentPair == null ? "Null" : "Not Null"));
//     //     pair.centerPos = pair.parentPair.centerPos - new Vector2(0,halfY * 1.3f) + new Vector2(x * halfX, -1 * halfY * 1.3f);
//     //     Vector2 malePos = pair.centerPos - new Vector2(halfX * 1.1f, 0);
//     //     Vector2 femalePos = pair.centerPos + new Vector2(halfX * 1.1f, 0);
//     //     pair.maleDP.transform.position = malePos;
//     //     pair.femaleDP.transform.position = femalePos;
//     // }
//     // private void ActivePair(){
//     //     maleDP.SetActive(true);
//     //     femaleDP.SetActive(true);
//     // }
//     // public void DeActive(){
//     //     if(isParent){
//     //         isParent = false;
//     //         if(childNum != 0){
//     //             foreach(Pair nowPair in children){
//     //                 nowPair.DeActivePair();
//     //             }
//     //         }
//     //     }
//     //     else{
//     //         parentPair.isParent = false;
//     //         parentPair.DeActivePair();
//     //         foreach(Pair nowPair in parentPair.children){
//     //             if(this != nowPair){
//     //                 nowPair.DeActivePair();
//     //             }
//     //         }
//     //     }

//     // }
//     // void DeActivePair(){

//     //     maleDP.SetActive(false);
//     //     femaleDP.SetActive(false);
//     // }

//     public string BlankNodeCheck(){
//         return  male.empty == true ? "Male" : "Female";
//     }
//     public void MakePair(Node node){
//         if(male.empty == true) male = node;
//         else{
//             female = node;
//         }
//     }
//     public void AddChild(){
//         if(isPair == true && childNum == 0){ //테스트용 조건문
//             childNum = Random.Range(1,5);
//             for(int i = 0; i < childNum; i++){
//                 Node node = new Node();
//                 node = SetByParent();
//                 if(children == null) children = new List<Pair>();
//                 if(node.sex == "Male"){
//                     Pair child = new Pair
//                     {
//                         parentPair = this,
//                         male = node,
//                         female = new Node(),
//                         isPair = false,
//                     };
//                     children.Add(child);
//                 }
//                 else{
//                     Pair child = new Pair
//                     {
//                         parentPair = this,
//                         male = new Node(),
//                         female = node,
//                         isPair = false,
//                     };
//                     children.Add(child);
//                 }
//             }
//         }
//         if(childNum != 0){
//             foreach(Pair pair in children){
//                 pair.AddChild();
//             }
//         }

//     }
//     private Node SetByParent(){
//         Node node = new Node{
//             name = GenerateRandomName(),
//             sex = Random.Range(0, 2) == 0 ? "Male" : "Female",
//             bloodType = GenerateBloodTypeArr(),
//             hp = Random.Range(50, 101),
//             age = Random.Range(20, 60),
//             isDead = false,
//             empty = false,
//         };
//         return node;
//     }
//     private string GenerateRandomName()
//     {
//         string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank" };
//         return names[Random.Range(0, names.Length)];
//     }
//     private string[] GenerateBloodTypeArr()
//     {
//         string BTGeno = GenerateBloodGenoType();
//         string BT = GenerateBloodType(BTGeno);
//         string RH = Random.Range(0, 2) == 0 ? male.bloodType[1] : female.bloodType[1];
//         return new string[] {BT, RH, BTGeno};
//     }
//     private string GenerateBloodType(string BTGeno){
//         if(BTGeno == "AB") return BTGeno;
//         else{
//             return BTGeno[0].ToString();
//         }
//     }
//     private string GenerateBloodGenoType(){
//         List<string> BTGenos = new List<string>();
//         for(int i = 0; i < male.bloodType[2].Length; i++){
//             for(int j = 0; j < female.bloodType[2].Length; j++){
//                 BTGenos.Add(FilterBloodGeno(male.bloodType[2][i].ToString() + female.bloodType[2][j].ToString()));
//             }
//         }
//         int idx = Random.Range(0,4);
//         return BTGenos[idx];
//     }

//     private string FilterBloodGeno(string BTGeno){
//         string newGeno;
//         if(BTGeno[0] > BTGeno[1]){
//             newGeno = BTGeno[1].ToString() + BTGeno[0].ToString();
//         }
//         else{
//             newGeno = BTGeno;
//         }
//         return newGeno;
//     }
// }

// [CreateAssetMenu(fileName = "PairSo", menuName = "Scriptable Object/PairSo")]
// public class PairSO : ScriptableObject
// {
//     public List<Pair> pairs;
// }
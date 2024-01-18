using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pairs : MonoBehaviour
{
    public List<Pair> pairs = new();
    public void Serialize(PairTree pairTree)
    {
        pairs.Clear();
        Local_SerializeAll(pairTree);
        void Local_SerializeAll(PairTree current)
        {
            // 직렬화용 노드 생성, 리스트에 추가
            pairs.Add(current.pair);

            foreach(PairTree child in current.children)
            {
                Local_SerializeAll(child);
            }
        }
    }
    public PairTree Deserialize()
    {
        if (pairs.Count == 0) return null;

        int index = 0;
        PairTree root = Local_DeserializeAll();

        return root;

        // 재귀 : 루트로부터 모든 자식들 역직렬화 및 트리 생성
        PairTree Local_DeserializeAll()
        {
            int currentIndex = index;
            PairTree current = pairs[currentIndex].Deserialize();

            index++;

            // 자식이 있을 경우, 자식을 역직렬화해서 자식목록에 추가
            // 그리고 다시 그 자식에서 재귀
            for (int i = 0; i < pairs[currentIndex].childNum; i++)
            {
                current.AddChild(Local_DeserializeAll());
                current.children[i].AddParent(current);
            }

            return current;
        }
    }
    public void MakeOlder(){
        foreach(Pair pair in pairs){
            if(!pair.male.empty){
                pair.male.age += 10;
            }
            if(!pair.female.empty){
                pair.female.age += 10;
            }
        }
    }
}

public class Pair
{
    public Node male;
    public Node female;
    public bool isPair;
    public int childNum;
    public PairTree Deserialize(){
        return new PairTree(this);
    }
}
public class PairTree
{
    public Pair pair;
    public PairTree parent;
    public List<PairTree> children;
    public PairTree(Pair data){
        this.children = new();
        this.pair = data;
    }
    public void AddParent(PairTree parent){
        this.parent = parent;
    }
    public void AddChild(PairTree child){
        this.children.Add(child);
    }
    public string BlankNodeCheck(){
        return  pair.male.empty == true ? "Male" : "Female";
    }
    public void MakePair(Node node){
        if(pair.male.empty == true) pair.male = node;
        else{
            pair.female = node;
        }
    }
    public void AddChild(){
        if(pair.isPair == true && pair.childNum == 0){ //테스트용 조건문
            pair.childNum = Random.Range(1,5);
            for(int i = 0; i < pair.childNum; i++){
                Node node = new Node();
                node = SetByParent();
                if(node.sex == "Male"){
                    Pair child = new Pair
                    {
                        male = node,
                        female = new Node(),
                        isPair = false,
                        childNum = 0,
                    };
                    AddChild(new PairTree(child));
                }
                else{
                    Pair child = new Pair
                    {
                        male = new Node(),
                        female = node,
                        isPair = false,
                        childNum = 0,
                    };
                    AddChild(new PairTree(child));
                }
            }
        }
        if(pair.childNum != 0){
            foreach(PairTree child in children){
                child.AddChild();
            }
        }

    }
    private Node SetByParent(){
        Node node = new Node{
            name = GenerateRandomName(),
            sex = Random.Range(0, 2) == 0 ? "Male" : "Female",
            bloodType = GenerateBloodTypeArr(),
            hp = 50,
            age = Random.Range(1, 11),
            isDead = false,
            empty = false,
        };
        return node;
    }
    private string GenerateRandomName()
    {
        string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank" };
        return names[Random.Range(0, names.Length)];
    }
    private string[] GenerateBloodTypeArr()
    {
        string BTGeno = GenerateBloodGenoType();
        string BT = GenerateBloodType(BTGeno);
        string RH = Random.Range(0, 2) == 0 ? pair.male.bloodType[1] : pair.female.bloodType[1];
        return new string[] {BT, RH, BTGeno};
    }
    private string GenerateBloodType(string BTGeno){
        if(BTGeno == "AB") return BTGeno;
        else{
            return BTGeno[0].ToString();
        }
    }
    private string GenerateBloodGenoType(){
        List<string> BTGenos = new List<string>();
        for(int i = 0; i < pair.male.bloodType[2].Length; i++){
            for(int j = 0; j < pair.female.bloodType[2].Length; j++){
                BTGenos.Add(FilterBloodGeno(pair.male.bloodType[2][i].ToString() + pair.female.bloodType[2][j].ToString()));
            }
        }
        int idx = Random.Range(0,4);
        return BTGenos[idx];
    }

    private string FilterBloodGeno(string BTGeno){
        string newGeno;
        if(BTGeno[0] > BTGeno[1]){
            newGeno = BTGeno[1].ToString() + BTGeno[0].ToString();
        }
        else{
            newGeno = BTGeno;
        }
        return newGeno;
    }
}

// public class PairSO
// {
//     public List<Pair> pairs = new();
//     public void Serialize(PairTree pairTree)
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
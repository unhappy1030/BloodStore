using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Animations;

[System.Serializable]

public class Pair
{
    public Pair parent;
    public Node male;
    public Node female;
    public bool isPair;
    public int childNum;
    public List<Pair> children;
    public string BlankNodeCheck(){
        return  male.empty == true ? "Male" : "Female";
    }
    public void IsMarried(Node node){
        if(male.empty == true) male = node;
        else{
            female = node;
        }
    }
    public void AddChild(){
        if(isPair == true){
            childNum = Random.Range(1,5);
            for(int i = 0; i < childNum; i++){
                Node node = new Node();
                node = SetByParent();
                if(children == null) children = new List<Pair>();
                if(node.sex == "Male"){
                    Pair child = new Pair
                    {
                        parent = this,
                        male = node,
                        female = new Node(),
                        isPair = false,
                    };
                    children.Add(child);
                }
                else{
                    Pair child = new Pair
                    {
                        parent = this,
                        male = new Node(),
                        female = node,
                        isPair = false,
                    };
                    children.Add(child);
                }
            }
        }
    }
    public void AddRandomChild(){
        Node node = new Node();
        node.SetAllRandom();
        if(node.sex == "Male"){
            Pair child = new Pair
            {
                parent = this,
                male = node,
                isPair = false,
            };
            children.Add(child);
        }
        else{
            Pair child = new Pair
            {
                parent = this,
                female = node,
                isPair = false,
            };
            children.Add(child);
        }
    }
    private Node SetByParent(){
        Node node = new Node{
            name = GenerateRandomName(),
            sex = Random.Range(0, 2) == 0 ? "Male" : "Female",
            bloodType = GenerateBloodTypeArr(),
            hp = Random.Range(50, 101),
            age = Random.Range(20, 60),
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
        string RH = Random.Range(0, 2) == 0 ? male.bloodType[1] : female.bloodType[1];
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
        for(int i = 0; i < male.bloodType[2].Length; i++){
            for(int j = 0; j < female.bloodType[2].Length; j++){
                BTGenos.Add(FilterBloodGeno(male.bloodType[2][i].ToString() + female.bloodType[2][j].ToString()));
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
    // public void MakePair(Node node){
    //     if(node.sex == "Male"){
    //         Pair child = new Pair
    //         {
    //             male = node,
    //             childNum = 0,
    //             isPair = false,
    //         };
    //     }
    //     else{
    //         Pair child = new Pair
    //         {
    //             female = node,
    //             childNum = 0,
    //             isPair = false,
    //         };
    //     }
    // }
}

[CreateAssetMenu(fileName = "PairSo", menuName = "Scriptable Object/PairSo")]
public class PairSO : ScriptableObject
{
    public List<Pair> pairs;
}
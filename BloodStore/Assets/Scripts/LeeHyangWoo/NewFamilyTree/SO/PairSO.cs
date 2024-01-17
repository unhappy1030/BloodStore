using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Animations;

[System.Serializable]

public class Pair
{
    public Node male;
    public Node female;
    public bool isPair;
    public int childNum;
    public Pair parentPair;
    public List<Pair> children;
    public bool isParent;
    public string BlankNodeCheck(){
        return  male.empty == true ? "Male" : "Female";
    }
    public void MakePair(Node node){
        if(male.empty == true) male = node;
        else{
            female = node;
        }
    }
    public void AddChild(){
        if(isPair == true && childNum == 0){ //테스트용 조건문
            childNum = Random.Range(1,5);
            for(int i = 0; i < childNum; i++){
                Node node = new Node();
                node = SetByParent();
                if(children == null) children = new List<Pair>();
                if(node.sex == "Male"){
                    Pair child = new Pair
                    {
                        parentPair = this,
                        male = node,
                        female = new Node(),
                        isPair = false,
                    };
                    children.Add(child);
                }
                else{
                    Pair child = new Pair
                    {
                        parentPair = this,
                        male = new Node(),
                        female = node,
                        isPair = false,
                    };
                    children.Add(child);
                }
            }
        }
        if(childNum != 0){
            foreach(Pair pair in children){
                pair.AddChild();
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
}


[CreateAssetMenu(fileName = "PairSo", menuName = "Scriptable Object/PairSo")]
public class PairSO : ScriptableObject
{
    public List<Pair> root;

    public void MakeOlder(){
        Pair pair = root[0];
        if(!pair.male.empty){
            pair.male.age += 10;
        }   
        if(!pair.female.empty){
            pair.female.age += 10;
        }
        if(pair.childNum != 0){
            foreach(Pair nowPair in pair.children){
                if(!nowPair.male.empty){
                    pair.male.age += 10;
                }   
                if(!nowPair.female.empty){
                    pair.female.age += 10;
                }
            }
        }
    }
    public void MakeOlder(Pair pair){
        if(pair.childNum != 0){
            foreach(Pair nowPair in pair.children){
                if(!nowPair.male.empty){
                    pair.male.age += 10;
                }   
                if(!nowPair.female.empty){
                    pair.female.age += 10;
                }
            }
        }
    }
}
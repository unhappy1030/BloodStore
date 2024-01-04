using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public string name;
    public string sex;

    public string[] bloodType;
    public int hp;
    public int age;
    public bool isDead;
    public Dictionary<string, string>[] type;
    public bool empty;
    public Node Nullnode = null;
    public Node(){
        empty = true;
    }
    public void SetAllRandom()
    {
        this.name = GenerateRandomName();
        this.sex = Random.Range(0, 2) == 0 ? "Male" : "Female";
        this.bloodType = GenerateRandomBloodType();
        this.hp = Random.Range(50, 101);
        this.age = Random.Range(20, 60);
        this.isDead = false;
        this.empty = false;
    }

    private string GenerateRandomName()
    {
        string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank" };
        return names[Random.Range(0, names.Length)];
    }

    private string[] GenerateRandomBloodType()
    {
        string[] bloodTypes = { "A", "B", "AB", "O" };
        string bloodType = bloodTypes[Random.Range(0, bloodTypes.Length)];
        string RH = Random.Range(0, 2) == 0 ? "+" : "-";
        string bloodGenoType = GenerateRandomBloodGenoType(bloodType);
        return new string[] {bloodType, RH, bloodGenoType};
    }
    private string GenerateRandomBloodGenoType(string bloodType){
        if(bloodType == "O") return "OO";
        else if(bloodType == "AB") return "AB";
        else{
            string bloodGenoType = Random.Range(0, 2) == 0 ? bloodType + bloodType : bloodType + "O";
            return bloodGenoType;
        }
    }
}

[CreateAssetMenu(fileName = "NodeSo", menuName = "Scriptable Object/NodeSo")]
public class NodeSO : ScriptableObject
{
    public List<Node> nodes;
}
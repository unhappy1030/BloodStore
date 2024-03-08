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
    public int imageIdx;
    public bool empty;
    public Node(){
        empty = true;
    }
    public int GetMaxHp(){
        if(age > 0 && age < 10){
            return 50;
        }
        else if(age >= 10 && age < 20){
            return 75;
        }
        else if(age >= 65 && age < 75){
            return 75;
        }
        else if(age >= 75 && age < 85){
            return 65;
        }
        else if(age >= 85){
            return 50;
        }
        else{
            return 100;
        }
    }
    public void SetAllRandom()
    {
        this.name = GenerateRandomName();
        this.sex = Random.Range(0, 2) == 0 ? "Male" : "Female";
        this.bloodType = GenerateRandomBloodType();
        this.hp = 100;
        this.age = Random.Range(20, 36);
        this.isDead = false;
        this.empty = false;
        this.imageIdx = GameManager.Instance.imageLoad.GetSpriteIndex(this.sex);
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
    public Node node = new();
    public void SetNode(SelectableCard card){
        this.node = new Node
        {
            name = card.name,
            sex = card.sex,
            bloodType = card.bloodType,
            hp = card.hp,
            age = card.age,
            isDead = card.isDead,
            empty = false,
            imageIdx = card.imageIdx,
        };
    }
}
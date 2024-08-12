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
    public int maxHp;
    public int age;
    public int mentalScore;
    public bool isDead;
    public int imageIdx;
    public int synergyCode;
    public bool empty;
    public Node(){
        empty = true;
    }
    public void ChangeMentalScore(int value){
        mentalScore += value;
        if(mentalScore < 0){
            mentalScore = 0;
        }
        if(mentalScore > 100){
            mentalScore = 100;
        }
    }
    public void ChangeHp(int value){
        hp += value;
        if(hp < 0){
            hp = 0;
        }
        if(hp > maxHp){
            hp = maxHp;
        }
    }
    public void SetAllRandom()
    {
        name = GeneratePersonData.GenerateRandomName();
        sex = Random.Range(0, 2) == 0 ? "Male" : "Female";
        bloodType = GeneratePersonData.GenerateRandomBloodType();
        age = Random.Range(20, 36);
        mentalScore = 60;
        isDead = false;
        empty = false;
        imageIdx = GameManager.Instance.imageLoad.GetSpriteIndex(sex);
        synergyCode = GeneratePersonData.GenerateSynergyCode();
        maxHp = GeneratePersonData.SetMaxHp(synergyCode);
        hp = maxHp;
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
            maxHp = card.maxHp,
            age = card.age,
            mentalScore = 60,
            isDead = card.isDead,
            empty = false,
            imageIdx = card.imageIdx,
            synergyCode = card.synergyCode
        };
    }
}
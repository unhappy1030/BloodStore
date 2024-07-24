using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SelectableCard
{
    public string name;
    public string sex;
    public string[] bloodType;
    public int hp;
    public int age;
    public int cost;
    public bool isDead;
    public int imageIdx;

    public void SetAllRandom(){
        this.name = GenerateRandomName();
        this.sex = Random.Range(0, 2) == 0 ? "Male" : "Female";
        this.bloodType = GenerateRandomBloodType();
        this.hp = 100;
        this.age = Random.Range(20, 36);
        this.cost = Random.Range(5, 21);
        this.isDead = false;
        this.imageIdx = GameManager.Instance.imageLoad.GetSpriteIndex(this.sex);
    }
    private string GenerateRandomName()
    {
        string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank" };
        return names[Random.Range(0, names.Length)];
    }

    // 랜덤 혈액형 생성 예시
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
[CreateAssetMenu(fileName = "SelectableCardSo", menuName = "Scriptable Object/SelectableCardSo")]
public class SelectableCardSO : ScriptableObject
{
    public List<SelectableCard> cards;
    public void Clear(){
        int cardsNum = cards.Count;
        if(cardsNum != 0){
            cards.RemoveRange(0, cardsNum - 1);
        }
    }
    public void MakeData(int n){
        if(cards.Count != 0){
            cards.Clear();
        }
        for(int i = 0; i < n; i++){
            SelectableCard card = new SelectableCard();
            cards.Add(card);
        }
    }
    public void AllRandom(){
        if(cards.Count != 0){
            foreach(SelectableCard card in cards){
                card.SetAllRandom();
            }
        }
    }
}
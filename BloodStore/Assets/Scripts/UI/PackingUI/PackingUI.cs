using System.Collections;
using System.Collections.Generic;
using System.CommandLine;
using Cinemachine.Utility;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PackingUI : MonoBehaviour
{
    public int count;
    public float money;
    public float yearCost = 50;
    public float addtionalCost;
    public TextMeshProUGUI numberTMP;
    public TextMeshProUGUI addtionalCostTMP;
    public GameObject treeManager;
    private TreeManager tree;
    private PairTree root;
    void Start()
    {
        if(gameObject.activeSelf){
            gameObject.SetActive(false);
        }
    }
    public void PackingConfirm(){
        tree.SavePairData();
        if(count != 0){
            MoneyControl moneyControl = GameManager.Instance.gameObject.GetComponent<MoneyControl>();
            moneyControl.CalculateMoney((yearCost * count + addtionalCost) * -1);
            GameManager.Instance.bloodPackList.PackingResult(GameManager.Instance.pairList);
        }
        tree.SaveAndChangeData();
    }
    public void SetCountUp(){
        this.money = GameManager.Instance.money;
        addtionalCost = GetAddtionalCost();
        if(money >= addtionalCost){
            money -= addtionalCost;
            int maxCount = (int)money / (int)yearCost;
            if(maxCount > count && count < 10){
                count++;
                numberTMP.text = count.ToString();
            }
        }
    }
    public void SetCountDown(){
        if(count > 0){
            count--;
            numberTMP.text = count.ToString();
        }
    }
    public void SetFirstValue(){
        this.money = GameManager.Instance.money;
        count = 0;
        addtionalCost = GetAddtionalCost();
        if(money >= addtionalCost){
            money -= addtionalCost;
            count = (int)money / (int)yearCost;
            if(count > 10){
                count = 10;
            }
        }
        numberTMP.text = count.ToString();
        addtionalCostTMP.text = addtionalCost.ToString();

    }
    private float GetAddtionalCost(){
        tree = treeManager.GetComponent<TreeManager>();
        root = tree.mainGroup.transform.GetChild(0).GetComponent<Group>().pairTree;
        return GetAddtionalCost(root);
    }
    private float GetAddtionalCost(PairTree rootPair){
        float costSum = CheckPairCost(rootPair.pair);
        if(rootPair.pair.childNum != 0){
            foreach(PairTree nowPair in rootPair.children){
                costSum += GetAddtionalCost(nowPair);
            }
        }
        return costSum;
    }
    private float CheckPairCost(Pair pair){
        float sum = 0;
        if(!pair.male.isDead){
            sum += GetCostByAge(pair.male.age);
        }
        if(!pair.female.isDead){
            sum += GetCostByAge(pair.female.age);
        }
        return sum;
    }   
    private float GetCostByAge(int age){
        if(age >= 16 && age < 35){
            return 5;
        }
        else if(age >= 35 && age < 65){
            return 10;
        }
        else if(age >= 65){
            return 15;
        }
        else{
            return 0;
        }
    }
}

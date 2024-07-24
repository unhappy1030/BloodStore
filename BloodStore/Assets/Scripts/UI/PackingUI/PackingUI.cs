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
    public float peopleTotalCost;
    public int[] peopleNums = new int[]{0, 0 ,0};
    public float[] peopleCosts = new float[]{0, 0 ,0};
    public float totalCost;
    public TextMeshProUGUI yearTMP;
    public TextMeshProUGUI yearCostTMP;
    public TextMeshProUGUI peopleTotalCostTMP;
    public List<TextMeshProUGUI> peopleNumsTMP;
    public List<TextMeshProUGUI> peopleCostsTMP;
    public TextMeshProUGUI totalCostTMP;
    public GameObject treeManager;
    private TreeManager tree;
    private TreePair root;
    void Start()
    {
        if(gameObject.activeSelf){
            gameObject.SetActive(false);
        }
    }
    public void PackingConfirm(){
        tree.SavePairData();
        MoneyControl moneyControl = GameManager.Instance.gameObject.GetComponent<MoneyControl>();
        moneyControl.CalculateMoney(totalCost * -1);
        GameManager.Instance.bloodPackList.PackingResult(GameManager.Instance.pairManager, count);
        tree.SaveAndChangeData();
    }
    public void SetCountUp(){
        this.money = GameManager.Instance.money;
        peopleTotalCost = GetAdditionalCost();
        if(money >= peopleTotalCost){
            money -= peopleTotalCost;
            int maxCount = (int)money / (int)yearCost;
            if(maxCount > count && count < 10){
                count++;
                SetYearTMP();
            }
        }
    }
    public void SetCountDown(){
        if(count > 0){
            count--;
            SetYearTMP();
        }
    }
    public void SetFirstValue(){
        this.money = GameManager.Instance.money;
        count = 0;
        peopleTotalCost = GetAdditionalCost();
        if(money >= peopleTotalCost){
            money -= peopleTotalCost;
            count = (int)money / (int)yearCost;
            if(count > 10){
                count = 10;
            }
        }
        SetYearTMP();
        peopleTotalCostTMP.text = peopleTotalCost.ToString() + " $";
        for(int i = 0; i < 3; i++){
            peopleNumsTMP[i].text = peopleNums[i].ToString();
            peopleCostsTMP[i].text = peopleCosts[i].ToString();
        }
    }
    void SetYearTMP(){
        yearTMP.text = count.ToString();
        yearCostTMP.text = (count * yearCost).ToString() + " $";
        SetTotalCost();
        totalCostTMP.text = totalCost.ToString() + " $";
    }
    void SetTotalCost(){
        if(count == 0){
            totalCost = 0;
        }
        else{
            totalCost = (count * yearCost) + peopleTotalCost;
        }
    }

    //Additional Cost Part
    private float GetAdditionalCost(){
        tree = treeManager.GetComponent<TreeManager>();
        root = tree.mainGroup.transform.GetChild(0).GetComponent<Group>().treePair;
        return GetAdditionalCost(root);
    }
    private float GetAdditionalCost(TreePair rootPair){
        float costSum = CheckPairCost(rootPair.pair);
        if(rootPair.pair.childNum != 0){
            foreach(TreePair nowPair in rootPair.children){
                costSum += GetAdditionalCost(nowPair);
            }
        }
        return costSum;
    }
    private float CheckPairCost(SerializePair pair){
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
            peopleNums[0] += 1;
            peopleCosts[0] += 5;
            return 5;
        }
        else if(age >= 35 && age < 65){
            peopleNums[1] += 1;
            peopleCosts[1] += 10;
            return 10;
        }
        else if(age >= 65){
            peopleNums[2] += 1;
            peopleCosts[2] += 15;
            return 15;
        }
        else{
            return 0;
        }
    }
}

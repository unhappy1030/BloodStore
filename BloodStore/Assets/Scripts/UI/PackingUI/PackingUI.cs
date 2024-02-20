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
    public TextMeshProUGUI yearTMP;
    public TextMeshProUGUI yearCostTMP;
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
        MoneyControl moneyControl = GameManager.Instance.gameObject.GetComponent<MoneyControl>();
        moneyControl.CalculateMoney((yearCost * count + addtionalCost) * -1);
        GameManager.Instance.bloodPackList.PackingResult(GameManager.Instance.pairList, count);
        tree.SaveAndChangeData();
        string folderName = GameManager.Instance.loadfileName;
        if(folderName != ""){
            Pairs pair = GameManager.Instance.pairList;
            BloodPacks bloodPack = GameManager.Instance.bloodPackList;
            pair.SaveFile(pair.pairs, folderName);
            bloodPack.SaveFile(bloodPack.bloodPacks, folderName);
        }
    }
    public void SetCountUp(){
        this.money = GameManager.Instance.money;
        addtionalCost = GetAdditionalCost();
        if(money >= addtionalCost){
            money -= addtionalCost;
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
        addtionalCost = GetAdditionalCost();
        if(money >= addtionalCost){
            money -= addtionalCost;
            count = (int)money / (int)yearCost;
            if(count > 10){
                count = 10;
            }
        }
        SetYearTMP();
        addtionalCostTMP.text = "< " +addtionalCost.ToString() + " >";

    }
    void SetYearTMP(){
        yearTMP.text = "< " + count.ToString() + " >";
        yearCostTMP.text = "Cost :" + (count * yearCost).ToString();
    }

    //Additional Cost Part
    private float GetAdditionalCost(){
        tree = treeManager.GetComponent<TreeManager>();
        root = tree.mainGroup.transform.GetChild(0).GetComponent<Group>().pairTree;
        return GetAdditionalCost(root);
    }
    private float GetAdditionalCost(PairTree rootPair){
        float costSum = CheckPairCost(rootPair.pair);
        if(rootPair.pair.childNum != 0){
            foreach(PairTree nowPair in rootPair.children){
                costSum += GetAdditionalCost(nowPair);
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

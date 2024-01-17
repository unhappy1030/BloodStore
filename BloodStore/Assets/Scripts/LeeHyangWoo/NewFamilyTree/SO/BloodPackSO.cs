using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BloodPack
{
    public Node node;
    public int packNum;
    public int getBloodDate;
    public BloodPack beforeDatePack;
    public BloodPack nextDatePack;

    public void AddLast(BloodPack pack){
        if(this.nextDatePack != null){
            this.nextDatePack.AddLast(pack);
        }
        else{
            this.nextDatePack = pack;
            this.nextDatePack.beforeDatePack = this;
        }
    }
}

[CreateAssetMenu(fileName = "BloodPackSo", menuName = "Scriptable Object/BloodPackSo")]
public class BloodPackSO : ScriptableObject
{
    public PairSO pairSO;
    public List<BloodPack> bloodPacks;
    public void GetBloodPack(){
        Pair nowPair = pairSO.root[0];
        if(nowPair.male.empty == false && nowPair.male.age >= 16){
            AddBloodPack(nowPair.male);
        }
        if(nowPair.female.empty == false && nowPair.female.age >= 16){
            AddBloodPack(nowPair.female);
        }
        if(nowPair.childNum != 0){
            foreach(Pair pair in nowPair.children){
                GetBloodPack(pair);
            }
        }
    }
    public void GetBloodPack(Pair nowPair){
        if(nowPair.male.empty == false && nowPair.male.age >= 16){
            AddBloodPack(nowPair.male);
        }
        if(nowPair.female.empty == false && nowPair.female.age >= 16){
            AddBloodPack(nowPair.female);
        }
        if(nowPair.childNum != 0){
            foreach(Pair pair in nowPair.children){
                GetBloodPack(pair);
            }
        }
    }
    public int GetPackSumNum(Node node){
        foreach(BloodPack bp in bloodPacks){
            int num = 0;
            if(node == bp.node){
                BloodPack bloodPack = bp;
                while(bloodPack.nextDatePack != null){
                   num += bloodPack.packNum;
                   bloodPack = bloodPack.nextDatePack; 
                }
                return num;
            }
        }
        return -1;
    }
    public void ModifyAtFistPackNum(Node node ,int num){
        foreach(BloodPack bp in bloodPacks){
            if(node == bp.node){
                BloodPack bloodPack = bp;
                while(bloodPack.nextDatePack != null){
                    if(bloodPack.packNum >= num){
                        bloodPack.packNum += num;
                        break;
                    }
                    else{
                        num -= bloodPack.packNum;
                        bloodPack.packNum = 0;
                    }
                   bloodPack = bloodPack.nextDatePack; 
                }
                break;
            }
        }
    }
    public void AddBloodPack(Node node){
        if(bloodPacks.Count == 0){
            bloodPacks.Add(NodeToBloodPack(node));
        }
        else{
            foreach(BloodPack pack in bloodPacks){
                if(pack.node == node){
                    pack.AddLast(NodeToBloodPack(node));
                    return;
                }
            }
            bloodPacks.Add(NodeToBloodPack(node));
        }
    }
    public BloodPack NodeToBloodPack(Node nodeConvert){
        int date =GameManager.Instance.day;
        BloodPack pack = new BloodPack{
            node = nodeConvert,
            packNum = Random.Range(1,5),
            getBloodDate = date,
        };
        return pack;
    }
}
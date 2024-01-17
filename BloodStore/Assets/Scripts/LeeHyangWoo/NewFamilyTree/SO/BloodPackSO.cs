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
        Debug.Log("피 줘");
        Pair pair = pairSO.root[0];
        if(pair.male.empty == false && pair.male.age >= 16){
            AddBloodPack(pair.male);
        }
        if(pair.female.empty == false && pair.female.age >= 16){
            AddBloodPack(pair.female);
        }
        if(pair.childNum != 0){
            foreach(Pair child in pair.children){
                GetBloodPack(child);
            }
        }
    }
    public void GetBloodPack(Pair pair){
        if(pair.male.empty == false && pair.male.age >= 16){
            AddBloodPack(pair.male);
        }
        if(pair.female.empty == false && pair.female.age >= 16){
            AddBloodPack(pair.female);
        }
        if(pair.childNum != 0){
            foreach(Pair child in pair.children){
                GetBloodPack(child);
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
            node = new Node{
                name = nodeConvert.name,
                sex = nodeConvert.sex,
                bloodType = nodeConvert.bloodType,
                age = nodeConvert.age,
                hp = nodeConvert.hp,
                type = nodeConvert.type
            },
            packNum = Random.Range(1,5),
            getBloodDate = date,
        };
        return pack;
    }
}
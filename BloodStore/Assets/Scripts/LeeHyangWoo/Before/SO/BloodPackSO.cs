using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BloodPack
{
    public Node node;
    public int bloodPackNum;
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
    public List<BloodPack> bloodPacks;
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
        int date = IndexManger.Instance.GetDayCount();
        BloodPack pack = new BloodPack{
            node = nodeConvert,
            bloodPackNum = Random.Range(1,5),
            getBloodDate = date,
        };
        return pack;
    }
}
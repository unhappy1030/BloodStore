using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BloodPack
{
    public Node node;
    public int num;
    public int date;
    public bool existNext;
    public BloodPackLink Deserialize(){
        return new BloodPackLink(this);
    }
}

public class BloodPackLink
{
    public BloodPack pack;
    public BloodPackLink before;
    public BloodPackLink next;
    public BloodPackLink(BloodPack data){
        this.pack = data;
        this.before = null;
        this.next = null;
    }
    public void AddBefore(BloodPackLink before){
        this.before = before;
    }
    public void AddNext(BloodPackLink next){
        this.next = next;
    }
    public void AddLast(BloodPack pack){
        if(this.next != null){
            this.next.AddLast(pack);
        }
        else{
            this.next = new BloodPackLink(pack);
            this.next.before = this;
        }
    }
}


[CreateAssetMenu(fileName = "BloodPackSo", menuName = "Scriptable Object/BloodPackSo")]
public class BloodPackSO : ScriptableObject
{
    // public PairSO pairSO;
    public List<BloodPack> bloodPacks = new();
    [System.NonSerialized]public List<BloodPackLink> bloodPackList = new();
    public void Serialize()
    {
        if(bloodPackList.Count == 0) return ;
        bloodPacks.Clear();
        foreach(BloodPackLink current in bloodPackList){
            Local_SerializeAll(current);
        }
        void Local_SerializeAll(BloodPackLink current)
        {
            // 직렬화용 노드 생성, 리스트에 추가
            bloodPacks.Add(current.pack);
            if(current.pack.existNext){
                Local_SerializeAll(current.next);
            }
        }
    }
    public void Deserialize()
    {
        if (bloodPacks.Count == 0) return ;
        int index = 0;
        bloodPackList.Clear();
        Local_DeserializeAll();

        void Local_DeserializeAll()
        {
            BloodPackLink current = bloodPacks[index].Deserialize();
            if(current.before == null){
                bloodPackList.Add(current);
                BloodPackLink before = current;
                index++;
                while(current.pack.existNext){
                    current = bloodPacks[index].Deserialize();
                    before.AddNext(current);
                    current.AddBefore(before);
                    before = current;
                    index++;
                }
            }
            if(bloodPacks.Count > index){
                Local_DeserializeAll();
            }
        }
    }
    // public void Packing(){
    //     Deserialize();
    //     foreach(Pair pair in pairSO.pairs){
    //         if(pair.male.empty == false && pair.male.age >= 16){
    //             AddBloodPack(pair.male);
    //         }
    //         if(pair.female.empty == false && pair.female.age >= 16){
    //             AddBloodPack(pair.female);
    //         }
    //     }
    //     Serialize();
    // }
    public void AddBloodPack(Node node){
        if(bloodPacks.Count == 0){
            bloodPackList.Add(new BloodPackLink(NodeToBloodPack(node)));
        }
        else{
            foreach(BloodPackLink packLink in bloodPackList){
                if(packLink.pack.node == node){
                    packLink.AddLast(NodeToBloodPack(node));
                    return;
                }
            }
            bloodPackList.Add(new BloodPackLink(NodeToBloodPack(node)));
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
            num = Random.Range(1,5),
            date = date,
        };
        return pack;
    }
}
// [CreateAssetMenu(fileName = "BloodPackSo", menuName = "Scriptable Object/BloodPackSo")]
// public class BloodPackSO : ScriptableObject
// {
//     public PairSO pairSO;
//     public List<BloodPack> bloodPacks;
//     public void GetBloodPack(){
//         Debug.Log("피 줘");
//         Pair pair = pairSO.root[0];
//         if(pair.male.empty == false && pair.male.age >= 16){
//             AddBloodPack(pair.male);
//         }
//         if(pair.female.empty == false && pair.female.age >= 16){
//             AddBloodPack(pair.female);
//         }
//         if(pair.childNum != 0){
//             foreach(Pair child in pair.children){
//                 GetBloodPack(child);
//             }
//         }
//     }
//     public void GetBloodPack(Pair pair){
//         if(pair.male.empty == false && pair.male.age >= 16){
//             AddBloodPack(pair.male);
//         }
//         if(pair.female.empty == false && pair.female.age >= 16){
//             AddBloodPack(pair.female);
//         }
//         if(pair.childNum != 0){
//             foreach(Pair child in pair.children){
//                 GetBloodPack(child);
//             }
//         }
//     }
//     public int GetPackSumNum(Node node){
//         foreach(BloodPack bp in bloodPacks){
//             int num = 0;
//             if(node == bp.node){
//                 BloodPack bloodPack = bp;
//                 while(bloodPack.nextDatePack != null){
//                    num += bloodPack.packNum;
//                    bloodPack = bloodPack.nextDatePack; 
//                 }
//                 return num;
//             }
//         }
//         return -1;
//     }
//     public void ModifyAtFistPackNum(Node node ,int num){
//         foreach(BloodPack bp in bloodPacks){
//             if(node == bp.node){
//                 BloodPack bloodPack = bp;
//                 while(bloodPack.nextDatePack != null){
//                     if(bloodPack.packNum >= num){
//                         bloodPack.packNum += num;
//                         break;
//                     }
//                     else{
//                         num -= bloodPack.packNum;
//                         bloodPack.packNum = 0;
//                     }
//                    bloodPack = bloodPack.nextDatePack; 
//                 }
//                 break;
//             }
//         }
//     }
//     public void AddBloodPack(Node node){
//         if(bloodPacks.Count == 0){
//             bloodPacks.Add(NodeToBloodPack(node));
//         }
//         else{
//             foreach(BloodPack pack in bloodPacks){
//                 if(pack.node == node){
//                     pack.AddLast(NodeToBloodPack(node));
//                     return;
//                 }
//             }
//             bloodPacks.Add(NodeToBloodPack(node));
//         }
//     }
//     public BloodPack NodeToBloodPack(Node nodeConvert){
//         int date =GameManager.Instance.day;
//         BloodPack pack = new BloodPack{
//             node = new Node{
//                 name = nodeConvert.name,
//                 sex = nodeConvert.sex,
//                 bloodType = nodeConvert.bloodType,
//                 age = nodeConvert.age,
//                 hp = nodeConvert.hp,
//                 type = nodeConvert.type
//             },
//             packNum = Random.Range(1,5),
//             getBloodDate = date,
//         };
//         return pack;
//     }
// }
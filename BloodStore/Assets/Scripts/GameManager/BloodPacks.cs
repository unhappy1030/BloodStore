using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.Assertions.Must;
using JetBrains.Annotations;
using System.Security.Cryptography;
[System.Serializable]
public class SaveBloodPackArray{
    public BloodPack[] arr;
}

public class BloodPacks : MonoBehaviour
{
    public List<BloodPack> bloodPacks = new();
    public List<BloodPackLink> bloodPackLinks = new();
    public SaveBloodPackArray saveArray;
    public Dictionary<string,int> categoryNum;
    public Dictionary<string, int> before, after, gap;
    public void Save(List<BloodPack> bloodPackList){
        string _path = Application.persistentDataPath + "/BloodPack.json"; 
        saveArray = new();
        saveArray.arr = bloodPackList.ToArray();
        string json = JsonUtility.ToJson(saveArray);
        File.WriteAllText(_path, json);
    }
    public BloodPacks Load(){
        string _path = Application.persistentDataPath + "/BloodPack.json";
        if(File.Exists(_path)){
            string jsonData = File.ReadAllText(_path);
            saveArray = JsonUtility.FromJson<SaveBloodPackArray>(jsonData);
            if(saveArray == null){
                Debug.Log("NewGame Start!");
            }
            else{
                Debug.Log("Save Data Load!");
                bloodPacks = new List<BloodPack>(saveArray.arr);
            }
        }
        else{
            bloodPacks = new();
        }
        return this;
    }
    public BloodPacks LoadNew(){
        bloodPacks = new();
        Serialize();
        Save(bloodPacks);
        return this;
    }
    public void Serialize()
    {
        if(bloodPackLinks.Count == 0) return ;
        bloodPacks.Clear();
        foreach(BloodPackLink current in bloodPackLinks){
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
        bloodPackLinks.Clear();
        Local_DeserializeAll();

        void Local_DeserializeAll()
        {
            BloodPackLink current = bloodPacks[index].Deserialize();
            if(current.before == null){
                bloodPackLinks.Add(current);
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
    public void PackingResult(Pairs pairList){
        Load();
        Deserialize();
        ShowAll();
        before = ShowAllDic();
        Packing(pairList);
        after = ShowAllDic();
        gap = GetGap(before, after);
        // Debug.Log("Show Gap");
        // foreach(string key in gap.Keys){
        //     Debug.Log(key + " : " + gap[key].ToString());
        // }
    }
    public void Packing(Pairs pairList){
        Load();
        Deserialize();
        foreach(Pair pair in pairList.pairs){
            if(pair.male.empty == false && pair.male.age >= 16){
                AddBloodPack(pair.male);
            }
            if(pair.female.empty == false && pair.female.age >= 16){
                AddBloodPack(pair.female);
            }
        }
        Serialize();
        Save(bloodPacks);
    }
    public void AddBloodPack(Node node){
        if(node.isDead){
            return;
        }
        if(bloodPacks.Count == 0){
            bloodPackLinks.Add(new BloodPackLink(NodeToBloodPack(node)));
        }
        else{
            foreach(BloodPackLink packLink in bloodPackLinks){
                if( packLink.pack.node.sex == node.sex && packLink.pack.node.bloodType[0] == node.bloodType[0] && packLink.pack.node.bloodType[1] == node.bloodType[1]){
                    packLink.AddLast(NodeToBloodPack(node));
                    return;
                }
            }
            bloodPackLinks.Add(new BloodPackLink(NodeToBloodPack(node)));
        }
    }
    public void SubtractBloodPack(string sex ,string bloodType, string rh, int num){
        int idx = 0;
        BloodPackLink head = bloodPackLinks[idx];
        idx = SubtractCheck(sex, bloodType, rh, num);
        if(idx != -1){
            head = bloodPackLinks[idx];
            if(head.pack.num > num){
                head.pack.num -= num;
            }
            else{
                num -= head.pack.num;
                head = head.next;
                head.pack.num -= num;
                bloodPackLinks[idx] = head;
            }
            Serialize();
            Save(bloodPacks);
        }
    }
    public int SubtractCheck(string sex ,string bloodType, string rh, int num){
        UpdateSumList();
        int idx = -1;
        bool findCheck = false;
        for(int i = 0; i < bloodPackLinks.Count; i++){
            if(bloodPackLinks[i].pack.node.sex == sex && bloodPackLinks[i].pack.node.bloodType[0] == bloodType && bloodPackLinks[i].pack.node.bloodType[1] == rh){
                idx = i;
                findCheck = true;
                break;
            }
        }
        if(findCheck && bloodPackLinks[idx].sum >= num){
            return idx;
        }
        else{
            return -1;
        }
    }
    public BloodPack NodeToBloodPack(Node nodeConvert){
        int date =GameManager.Instance.day;
        BloodPack pack = new BloodPack{
            node = new Node{
                name = nodeConvert.name,
                sex = nodeConvert.sex,
                age = nodeConvert.age,
                bloodType = nodeConvert.bloodType,
            },
            num = Random.Range(1,5),
            date = date,
        };
        return pack;
    }
    public void UpdateSumList(){
        for(int i = 0; i < bloodPackLinks.Count; i++){
            bloodPackLinks[i].Sum();
        }
    }
    public void UpdateCategory(){
        Load();
        Deserialize();
        UpdateSumList();
        categoryNum = new();
        categoryNum.Add("Male", 0);
        categoryNum.Add("Female", 0);
        categoryNum.Add("A", 0);
        categoryNum.Add("B", 0);
        categoryNum.Add("AB", 0);
        categoryNum.Add("O", 0);
        categoryNum.Add("-", 0);
        categoryNum.Add("+", 0);
        foreach(BloodPackLink link in bloodPackLinks){
            if(link.pack.node.sex == "Male"){
                categoryNum["Male"] += link.sum;
            }
            else{
                categoryNum["Female"] += link.sum;
            }
            if(link.pack.node.bloodType[0] == "A"){
                categoryNum["A"] += link.sum;
            }
            else if(link.pack.node.bloodType[0] == "B"){
                categoryNum["B"] += link.sum;
            }
            else if(link.pack.node.bloodType[0] == "AB"){
                categoryNum["AB"] += link.sum;
            }
            else if(link.pack.node.bloodType[0] == "O"){
                categoryNum["O"] += link.sum;
            }
            if(link.pack.node.bloodType[1] == "+"){
                categoryNum["+"] += link.sum;
            }
            else{
                categoryNum["-"] += link.sum;
            }
        }
    }
    public void ShowAll(){
        UpdateSumList();
        Debug.Log("ShowAll");
        foreach(BloodPackLink link in bloodPackLinks){
            Debug.Log(link.pack.node.sex + link.pack.node.bloodType[0] + link.pack.node.bloodType[1] + " : " + link.sum.ToString());
        }
    }
    public Dictionary<string, int> ShowAllDic(){
        UpdateSumList();
        Debug.Log("ShowAllDictionary");
        Dictionary<string, int> result = new();
        foreach(BloodPackLink link in bloodPackLinks){
            Debug.Log(link.pack.node.sex + link.pack.node.bloodType[0] + link.pack.node.bloodType[1] + " : " + link.sum.ToString());
            result.Add(link.pack.node.sex + link.pack.node.bloodType[0] + link.pack.node.bloodType[1], link.sum);
        }
        return result;
    }
    public Dictionary<string, int> GetGap(Dictionary<string, int> before,Dictionary<string, int> after){
        Dictionary<string, int> gap = new();
        foreach(string key in after.Keys){
            if(!before.ContainsKey(key)){
                gap.Add(key, after[key]);
            }
            else{
                gap.Add(key, after[key] - before[key]);
            }
        }
        return gap;
    }
    public int GetCondition(string sex ,string bloodType, string rh){
        UpdateSumList();
        int num = 0;

        List<BloodPackLink> sexList = new();

        foreach(BloodPackLink link in bloodPackLinks){
            if(sex == ""){
                sexList = new(bloodPackLinks);
                break;
            }

            if(link.pack.node.sex == sex){
                sexList.Add(link);
            }
        }

        List<BloodPackLink> bloodList = new();

        foreach(BloodPackLink link in sexList){
            if(bloodType == ""){
                bloodList = new(sexList);
                break;
            }

            if(link.pack.node.bloodType[0] == bloodType){
                bloodList.Add(link);
            }

        }

        List<BloodPackLink> rhList = new();

        foreach(BloodPackLink link in bloodList){
            if(rh == ""){
                rhList = new(bloodList);
                break;
            }

            if(link.pack.node.bloodType[1] == rh){
                rhList.Add(link);
            }
        }

        foreach(BloodPackLink link in rhList){
            num += link.sum;
        }

        return num;
    }
}

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
    public int sum;
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
        if(this.pack.existNext){
            this.next.AddLast(pack);
        }
        else{
            this.next = new BloodPackLink(pack);
            this.pack.existNext =true;
            this.next.before = this;
        }
    }
    public void Sum(){
        sum = 0;
        BloodPackLink nowLink = this;
        sum = nowLink.pack.num;
        while(nowLink.pack.existNext){
            nowLink = nowLink.next;
            this.sum += nowLink.pack.num;
        }
    }
}


// public class BloodPackSO : ScriptableObject
// {
//     // public PairSO pairSO;
//     public List<BloodPack> bloodPacks = new();
//     [System.NonSerialized]public List<BloodPackLink> bloodPackList = new();
//     public void Serialize()
//     {
//         if(bloodPackList.Count == 0) return ;
//         bloodPacks.Clear();
//         foreach(BloodPackLink current in bloodPackList){
//             Local_SerializeAll(current);
//         }
//         void Local_SerializeAll(BloodPackLink current)
//         {
//             // 직렬화용 노드 생성, 리스트에 추가
//             bloodPacks.Add(current.pack);
//             if(current.pack.existNext){
//                 Local_SerializeAll(current.next);
//             }
//         }
//     }
//     public void Deserialize()
//     {
//         if (bloodPacks.Count == 0) return ;
//         int index = 0;
//         bloodPackList.Clear();
//         Local_DeserializeAll();

//         void Local_DeserializeAll()
//         {
//             BloodPackLink current = bloodPacks[index].Deserialize();
//             if(current.before == null){
//                 bloodPackList.Add(current);
//                 BloodPackLink before = current;
//                 index++;
//                 while(current.pack.existNext){
//                     current = bloodPacks[index].Deserialize();
//                     before.AddNext(current);
//                     current.AddBefore(before);
//                     before = current;
//                     index++;
//                 }
//             }
//             if(bloodPacks.Count > index){
//                 Local_DeserializeAll();
//             }
//         }
//     }
//     // public void Packing(){
//     //     Deserialize();
//     //     foreach(Pair pair in pairSO.pairs){
//     //         if(pair.male.empty == false && pair.male.age >= 16){
//     //             AddBloodPack(pair.male);
//     //         }
//     //         if(pair.female.empty == false && pair.female.age >= 16){
//     //             AddBloodPack(pair.female);
//     //         }
//     //     }
//     //     Serialize();
//     // }
//     public void AddBloodPack(Node node){
//         if(bloodPacks.Count == 0){
//             bloodPackList.Add(new BloodPackLink(NodeToBloodPack(node)));
//         }
//         else{
//             foreach(BloodPackLink packLink in bloodPackList){
//                 if(packLink.pack.node == node){
//                     packLink.AddLast(NodeToBloodPack(node));
//                     return;
//                 }
//             }
//             bloodPackList.Add(new BloodPackLink(NodeToBloodPack(node)));
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
//             num = Random.Range(1,5),
//             date = date,
//         };
//         return pack;
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//오브젝트 이름 : GameManger

//SerializePair를 저장
[System.Serializable]
public class PairSerializeArray{
    public SerializePair[] pairArray;
}

//Pair를 저장/불러옴
public class PairManager : MonoBehaviour
{
    private const string FamilyTreeFileName = "FamilyTree.json";

    public List<SerializePair> serializePairList = new();
    PairSerializeArray pairSerializeArray = new();

    
    //기능 : 플레이 중 데이터 저장
    //파리미터 설명 : 없음
    //반환값 설명 : 없음
    public void Save(){
        string path = Application.persistentDataPath + FamilyTreeFileName;
        pairSerializeArray = new();
        pairSerializeArray.pairArray = serializePairList.ToArray();
        string json = JsonUtility.ToJson(pairSerializeArray);
        File.WriteAllText(path, json);
    }

    //기능 : 세이브 파일에 해당하는 폴더에 저장
    //파리미터 설명 : 저장하려는 "세이브 파일의 이름"이 필요함(*세이브 파일의 이름 : 플레이어로 부터 입력 받은 것)
    //반환값 설명 : 없음
    public void SaveFile(string folderName){
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        string path = Path.Combine(folderPath, FamilyTreeFileName);
        pairSerializeArray = new();
        pairSerializeArray.pairArray = serializePairList.ToArray();
        string json = JsonUtility.ToJson(pairSerializeArray);
        if(!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
        }
        File.WriteAllText(path, json);
    }

    //기능 : 플레이 중 데이터를 불러옴
    //파리미터 설명 : 없음
    //반환값 설명 : 현재 PairManager 인스턴스 자체를 반환
//굳이 PairManager를 반환해야하는 지 점검해보기
    public PairManager Load(){
        string path = Application.persistentDataPath + FamilyTreeFileName;
        if(File.Exists(path)){
            string jsonData = File.ReadAllText(path);
            pairSerializeArray = JsonUtility.FromJson<PairSerializeArray>(jsonData);
            if(pairSerializeArray == null){
                Debug.Log("NewGame Start!");
            }
            else{
                Debug.Log("Save Data Load!");
                serializePairList = new List<SerializePair>(pairSerializeArray.pairArray);
            }
        }
        else{
            serializePairList = new();
        }
        GameManager.Instance.imageLoad.LoadImageUseCount(serializePairList);
        return this;
    }

    //기능 : 새게임을 시작할때 데이터 초기화
    //파리미터 설명 : 없음
    //반환값 설명 : 현재 PairManager 인스턴스 자체를 반환
    public PairManager LoadNew(){
        serializePairList = new();
        GameManager.Instance.imageLoad.LoadImageUseCount(serializePairList);
        return this;
    }

    //기능 : 세이브 파일 데이터를 불러옴
    //파리미터 설명 : 불러오려는 "세이브 파일의 이름"이 필요함(*세이브 파일의 이름 : 플레이어로 부터 입력 받은 것)
    //반환값 설명 : 현재 PairManager 인스턴스 자체를 반환
    public PairManager LoadFile(string folderName){
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        string _path = Path.Combine(folderPath, FamilyTreeFileName);
        if(File.Exists(_path)){
            string jsonData = File.ReadAllText(_path);
            pairSerializeArray = JsonUtility.FromJson<PairSerializeArray>(jsonData);
            if(pairSerializeArray == null){
                Debug.Log("NewGame Start!");
            }
            else{
                Debug.Log("Save Data Load!");
                serializePairList = new List<SerializePair>(pairSerializeArray.pairArray);
            }
        }
        else{
            serializePairList = new();
        }
        GameManager.Instance.imageLoad.LoadImageUseCount(serializePairList);
        Save();
        return this;
    }

    //기능 : TreePair를 SerializePair로 직렬화
    //파리미터 설명 : TreePair의 root 노드
    //반환값 설명 : 없음
    public void Serialize(TreePair treePair)
    {
        serializePairList.Clear();
        Local_SerializeAll(treePair);
        void Local_SerializeAll(TreePair current)
        {
            // 직렬화용 노드 생성, 리스트에 추가
            serializePairList.Add(current.pair);

            foreach(TreePair child in current.children)
            {
                Local_SerializeAll(child);
            }
        }
    }

    //기능 : SerializePair를 TreePair로 변환
    //파리미터 설명 : 없음
    //반환값 설명 : TreePair의 root 노드
    public TreePair Deserialize()
    {
        if (serializePairList.Count == 0) return null;

        int index = 0;
        TreePair root = Local_DeserializeAll();

        return root;

        // 재귀 : 루트로부터 모든 자식들 역직렬화 및 트리 생성
        TreePair Local_DeserializeAll()
        {
            int currentIndex = index;
            TreePair current = serializePairList[currentIndex].Deserialize();

            index++;

            // 자식이 있을 경우, 자식을 역직렬화해서 자식목록에 추가
            // 그리고 다시 그 자식에서 재귀
            for (int i = 0; i < serializePairList[currentIndex].childNum; i++)
            {
                current.AddChild(Local_DeserializeAll());
                current.children[i].AddParent(current);
            }

            return current;
        }
    }

    //기능 : SerializePair를 TreePair로 변환
    //파리미터 설명 : 없음
    //반환값 설명 : TreePair의 root 노드
    public void MakeOlder(){
        foreach(SerializePair pair in serializePairList){
            if(!pair.male.empty){
                pair.male.age += 10;
            }
            if(!pair.female.empty){
                pair.female.age += 10;
            }
        }
    }


    public bool CheckAllDead(){
        bool result = true;
        if(serializePairList == null || serializePairList.Count == 0){
            return false;
        }
        foreach(SerializePair pair in serializePairList){
            if(!pair.male.empty){
                if(!pair.male.isDead){
                    result = false;
                    break;
                }
            }
            if(!pair.female.empty){
                if(!pair.female.isDead){
                    result = false;
                    break;
                }
            }
        }
        return result;
    }
    public void MakeDead(){
        foreach(SerializePair pair in serializePairList){
            if(!pair.male.empty){
                if(CheckDead(pair.male.age)){
                    pair.male.isDead = true;
                }
            }
            if(!pair.female.empty){
                if(CheckDead(pair.female.age)){
                    pair.female.isDead = true;
                }
            }
        }
    }
    public bool CheckDead(int age){
        if(age >= 70){
            float prob = age / 200f; 
            float rand = Random.Range(0f,1f);
            if(rand <= prob){
                return true;
            }
            else{
                return false;
            }
        }
        else{
            return false;
        }
    }

}
[System.Serializable]
public class SerializePair
{
    public Node male;
    public Node female;
    public bool isPair;
    public int childNum;
    public TreePair Deserialize(){
        return new TreePair(this);
    }
}

public class TreePair
{
    public SerializePair pair;
    public TreePair parent;
    public List<TreePair> children;
    public TreePair(SerializePair data){
        this.children = new();
        this.pair = data;
    }
    public void AddParent(TreePair parent){
        this.parent = parent;
    }
    public void AddChild(TreePair child){
        this.children.Add(child);
    }
    public string BlankNodeCheck(){
        return  pair.male.empty == true ? "Male" : "Female";
    }
    public void MakePair(Node node){
        if(pair.male.empty == true) pair.male = node;
        else{
            pair.female = node;
        }
    }
    public void AddChildByValue(float weight, float probability){
        if(pair.isPair == true && pair.childNum == 0){ //테스트용 조건문
            pair.childNum = GetChildNum(weight, probability);
            if(pair.childNum > 5){
                pair.childNum = 5;
            }
            for(int i = 0; i < pair.childNum; i++){
                Node node = new Node();
                node = SetByParent();
                if(node.sex == "Male"){
                    SerializePair child = new SerializePair
                    {
                        male = node,
                        female = new Node(),
                        isPair = false,
                        childNum = 0,
                    };
                    AddChild(new TreePair(child));
                }
                else{
                    SerializePair child = new SerializePair
                    {
                        male = new Node(),
                        female = node,
                        isPair = false,
                        childNum = 0,
                    };
                    AddChild(new TreePair(child));
                }
            }
        }
    }
    int GetChildNum(float weight, float probability){
        int num = 0;
        float random = Random.Range(0f, 1f);
        float value = probability;
        while(random <= value){
            num++;
            value *= weight;
            random = Random.Range(0f, 1f);
        }
        return num;
    }
    public void AddChild(){
        if(pair.isPair == true && pair.childNum == 0){ //테스트용 조건문
            pair.childNum = Random.Range(1,5);
            for(int i = 0; i < pair.childNum; i++){
                Node node = new Node();
                node = SetByParent();
                if(node.sex == "Male"){
                    SerializePair child = new SerializePair
                    {
                        male = node,
                        female = new Node(),
                        isPair = false,
                        childNum = 0,
                    };
                    AddChild(new TreePair(child));
                }
                else{
                    SerializePair child = new SerializePair
                    {
                        male = new Node(),
                        female = node,
                        isPair = false,
                        childNum = 0,
                    };
                    AddChild(new TreePair(child));
                }
            }
        }
        if(pair.childNum != 0){
            foreach(TreePair child in children){
                child.AddChild();
            }
        }
    }
    private Node SetByParent(){
        Node node = new Node{
            name = GenerateRandomName(),
            sex = Random.Range(0, 2) == 0 ? "Male" : "Female",
            bloodType = GenerateBloodTypeArr(),
            hp = 50,
            age = Random.Range(-9, 0),
            isDead = false,
            empty = false,
        };
        node.imageIdx = GameManager.Instance.imageLoad.GetSpriteIndex(node.sex);
        return node;
    }
    private string GenerateRandomName()
    {
        string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank" };
        return names[Random.Range(0, names.Length)];
    }
    private string[] GenerateBloodTypeArr()
    {
        string BTGeno = GenerateBloodGenoType();
        string BT = GenerateBloodType(BTGeno);
        string RH = Random.Range(0, 2) == 0 ? pair.male.bloodType[1] : pair.female.bloodType[1];
        return new string[] {BT, RH, BTGeno};
    }
    private string GenerateBloodType(string BTGeno){
        if(BTGeno == "AB") return BTGeno;
        else{
            return BTGeno[0].ToString();
        }
    }
    private string GenerateBloodGenoType(){
        List<string> BTGenos = new List<string>();
        for(int i = 0; i < pair.male.bloodType[2].Length; i++){
            for(int j = 0; j < pair.female.bloodType[2].Length; j++){
                BTGenos.Add(FilterBloodGeno(pair.male.bloodType[2][i].ToString() + pair.female.bloodType[2][j].ToString()));
            }
        }
        int idx = Random.Range(0,4);
        return BTGenos[idx];
    }

    private string FilterBloodGeno(string BTGeno){
        string newGeno;
        if(BTGeno[0] > BTGeno[1]){
            newGeno = BTGeno[1].ToString() + BTGeno[0].ToString();
        }
        else{
            newGeno = BTGeno;
        }
        return newGeno;
    }
}
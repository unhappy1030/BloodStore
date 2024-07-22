using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//오브젝트 이름 : GameManger

/// <summary>
/// SerializePair를 저장
/// </summary>
[System.Serializable]
public class PairSerializeArray{
    public SerializePair[] pairArray;
}

/// <summary>
/// Pair를 저장/불러옴
/// </summary>
public class PairManager : MonoBehaviour
{
    private const string FamilyTreeFileName = "FamilyTree.json";

    public List<SerializePair> serializePairList = new();
    PairSerializeArray pairSerializeArray = new();

    
    /// <summary>
    /// 플레이 중 데이터 저장
    /// </summary>
    public void Save(){
        string path = Application.persistentDataPath + FamilyTreeFileName;
        pairSerializeArray = new();
        pairSerializeArray.pairArray = serializePairList.ToArray();
        string json = JsonUtility.ToJson(pairSerializeArray);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// 세이브 파일에 해당하는 폴더에 저장
    /// </summary>
    /// <param name="folderName">플레이어로 부터 입력 받은 세이브 파일의 이름</param>
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

    /// <summary>
    /// 세이브 파일에 해당하는 폴더에 저장
    /// </summary>
    /// <returns> 현재 PairManager 인스턴스 자체를 반환 </returns>
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

    /// <summary>
    /// 새게임을 시작할때 데이터 초기화
    /// </summary>
    /// <returns> 현재 PairManager 인스턴스 자체를 반환 </returns>
    public PairManager LoadNew(){
        serializePairList = new();
        GameManager.Instance.imageLoad.LoadImageUseCount(serializePairList);
        return this;
    }

    /// <summary>
    /// 세이브 파일 데이터를 불러옴
    /// </summary>
    /// <param name="folderName">플레이어로 부터 입력 받은 세이브 파일의 이름</param>
    /// <returns> 현재 PairManager 인스턴스 자체를 반환 </returns>
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

    /// <summary>
    /// TreePair를 SerializePair로 직렬화
    /// </summary>
    /// <param name="treePair">TreePair의 root 노드</param>
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

    /// <summary>
    /// SerializePair를 TreePair로 변환
    /// </summary>
    /// <returns>TreePair의 root 노드</returns>
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

    /// <summary>
    /// 직렬화된 데이터를 사용해서 전체 사람의 나이 증가
    /// </summary>
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

    /// <summary>
    /// 모든 Node가 죽었는 지 확인한다
    /// </summary>
    /// <returns>다 죽었다면 true, 살아있는 사람이 있다면 false</returns>
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
    /// <summary>
    /// 나이를 확인하여 확률적으로 죽은 노드로 만듦
    /// </summary>
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
    /// <summary>
    /// 70세 이상이면 죽을 수 있고 나이가 많을수록 죽을 확률이 올라감
    /// </summary>
    /// <param name="age">Node의 나이</param>
    /// <returns>죽으면 true, 살면 false</returns>
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

/// <summary>
/// TreePair직렬화를 위한 class
/// </summary>
[System.Serializable]
public class SerializePair
{
    public Node male;
    public Node female;
    public bool isPair;
    public int childNum;
    /// <summary>
    /// 현재 직렬화된 SerializePair를 TreePair로 바꿈
    /// </summary>
    /// <returns>TreePair로 바뀐 SerializePair의 값</returns>
    public TreePair Deserialize(){
        return new TreePair(this);
    }
}

/// <summary>
/// 가계도를 Tree구조로 저장
/// </summary>
public class TreePair
{
    public SerializePair pair;
    public TreePair parent;
    public List<TreePair> children;
    /// <summary>
    /// SerializePair의 값으로 TreePair로 바꿈
    /// </summary>
    /// <param name="data">SerializePair(직렬화된 Pair)</param>
    public TreePair(SerializePair data){
        this.children = new();
        this.pair = data;
    }

    /// <summary>
    /// TreePair에 부모 TreePair를 추가
    /// </summary>
    /// <param name="parent">부모로 지정해줄 TreePair</param>
    public void AddParent(TreePair parent){
        this.parent = parent;
    }

    /// <summary>
    /// TreePair를 자식으로 추가
    /// </summary>
    /// <param name="child">자식으로 추가할 TreePair</param>
    public void AddChild(TreePair child){
        this.children.Add(child);
    }
    /// <summary>
    /// 비어있는 노드의 남/여 판별
    /// </summary>
    /// <returns>비어있는 노드의 성별</returns>
    public string BlankNodeCheck(){
        return  pair.male.empty == true ? "Male" : "Female";
    }
    /// <summary>
    /// TreePair의 빈곳에 Node추가
    /// </summary>
    /// <param name="node">추가할 Node</param>
    public void MakePair(Node node){
        if(pair.male.empty == true) pair.male = node;
        else{
            pair.female = node;
        }
    }

    /// <summary>
    /// 가중치와 확룔을 통해서 자식을 추가
    /// </summary>
    /// <param name="weight">자식이 추가되는 확률에 곱해지는 가중치(자식이 하나 추가될때 마다 확률이 점점 감소하게 됨)</param>
    /// <param name="probability">처음 주어지는 자식이 추가될 수 있는 확률</param>
    public void AddChildByValue(float weight, float probability){
        if(pair.isPair == true && pair.childNum == 0){ //테스트용 조건문
            pair.childNum = GetChildNum(weight, probability);
            if(pair.childNum > 5){
                pair.childNum = 5;
            }
            for(int i = 0; i < pair.childNum; i++){
                Node node = new Node();
                node = SetChildByParent();
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
    /// <summary>
    /// 가중치와 확률을 통해서 자식의 수를 구함
    /// </summary>
    /// <param name="weight">자식이 추가되는 확률에 곱해지는 가중치(자식이 하나 추가될때 마다 확률이 점점 감소하게 됨)</param>
    /// <param name="probability">처음 주어지는 자식이 추가될 수 있는 확률</param>
    /// <returns>자식의 수</returns>
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
    /// <summary>
    ///   1~5 사이로 랜덤으로 자식를 생성(임의로 제작된 함수)
    /// </summary>
    public void AddChild(){
        if(pair.isPair == true && pair.childNum == 0){ //테스트용 조건문
            pair.childNum = Random.Range(1,5);
            for(int i = 0; i < pair.childNum; i++){
                Node node = new Node();
                node = SetChildByParent();
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
    /// <summary>
    /// 부모의 혈액형과 유전데이터로 자식 Node를 생성
    /// </summary>
    /// <returns>자식노드</returns>
    private Node SetChildByParent(){
        Node node = new Node{
            name = GenerateRandomName(),
            sex = Random.Range(0, 2) == 0 ? "Male" : "Female",
            bloodType = GenerateBloodTypeArray(),
            hp = 50,
            age = Random.Range(-9, 0),
            isDead = false,
            empty = false,
        };
        node.imageIdx = GameManager.Instance.imageLoad.GetSpriteIndex(node.sex);
        return node;
    }
    /// <summary>
    /// 랜덤으로 이름 생성(임시적으로 제작)
    /// </summary>
    /// <returns>이름</returns>
    private string GenerateRandomName()
    {
        string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank" };
        return names[Random.Range(0, names.Length)];
    }
    /// <summary>
    /// 부모의 혈액 유전 정보로 혈액 유전 정보 배열 생성
    /// </summary>
    /// <returns>문자열{혈액형, }</returns>
    private string[] GenerateBloodTypeArray()
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
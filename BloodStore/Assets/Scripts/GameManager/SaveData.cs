using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataList{
    public int sellCount ;
    public float totalPoint ;
    public float currentAveragePoint;
    public float filterDurability;
    public int day;
    public float money;
    public string SceneName;
}


public class SaveData : MonoBehaviour
{
    public void Save(){
        string _path = Application.persistentDataPath + "/SaveData.json";
        DataList dataList = GetDataList();
        string json = JsonUtility.ToJson(dataList);
        File.WriteAllText(_path, json);
    }
    public void SaveFile(string folderName){
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        string _path = Path.Combine(folderPath, "SaveData.json");
        DataList dataList = GetDataList();
        string json = JsonUtility.ToJson(dataList);
        if(!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
        }
        File.WriteAllText(_path, json);
    }
    public void Load(){
        string _path = Application.persistentDataPath + "/SaveData.json";
        if(File.Exists(_path)){
            string jsonData = File.ReadAllText(_path);
            DataList dataList =JsonUtility.FromJson<DataList>(jsonData);
            if(dataList == null){
                Debug.Log("NewGame Start!");
            }
            else{
                Debug.Log("Save Data Load");
                SetDataListToGameManager(dataList);
            }
        }
    }
    public void LoadFile(string folderName){
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        string _path = Path.Combine(folderPath, "SaveData.json");
        if(File.Exists(_path)){
            string jsonData = File.ReadAllText(_path);
            DataList dataList =JsonUtility.FromJson<DataList>(jsonData);
            if(dataList == null){
                Debug.Log("NewGame Start!");
            }
            else{
                Debug.Log("Save Data Load");
                SetDataListToGameManager(dataList);
            }
        }
    }
    private DataList GetDataList(){
        DataList dataList = new();
        dataList.sellCount = GameManager.Instance.sellCount;
        dataList.totalPoint = GameManager.Instance.totalPoint;
        dataList.currentAveragePoint = GameManager.Instance.currentAveragePoint;
        dataList.filterDurability = GameManager.Instance.filterDurability;
        dataList.day = GameManager.Instance.day;
        dataList.money = GameManager.Instance.money;
        dataList.SceneName = SceneManager.GetActiveScene().name;
        return dataList;
    }
    private void SetDataListToGameManager(DataList dataList){
        GameManager.Instance.sellCount = dataList.sellCount;
        GameManager.Instance.totalPoint = dataList.totalPoint;
        GameManager.Instance.currentAveragePoint = dataList.currentAveragePoint;
        GameManager.Instance.filterDurability = dataList.filterDurability;
        GameManager.Instance.day = dataList.day;
        GameManager.Instance.money = dataList.money;
    }
}

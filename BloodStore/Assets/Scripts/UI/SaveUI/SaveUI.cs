using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
public class SavetUI : MonoBehaviour
{
    public GameObject selectedFile;
    public List<GameObject> files;
    public GameObject addNewFileUI;
    public TextMeshProUGUI description;
    public TMP_InputField fileNameInput;
    void Start()
    {
        if(gameObject.activeSelf){
            gameObject.SetActive(false);
        }
        foreach(GameObject file in files){
            if(file.activeSelf){
                file.SetActive(false);
            }
        }
        if(addNewFileUI.activeSelf){
            addNewFileUI.SetActive(false);
        }
        ShowFiles();
    }
    public void SaveFile(){
        if(selectedFile != null){
            string folderName = selectedFile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            Pairs pair = GameManager.Instance.pairList;
            BloodPacks bloodPack = GameManager.Instance.bloodPackList;
            pair.SaveFile(pair.pairs, folderName);
            bloodPack.SaveFile(bloodPack.bloodPacks, folderName);
            gameObject.SetActive(false);
        }
    }
    public void DeleteFile(){
        if(selectedFile != null){
            string folderName = selectedFile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            string folderPath = Path.Combine(Application.persistentDataPath, folderName);
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
            selectedFile.SetActive(false);
        }
    }
    void ShowFiles(){
        List<string> directoryNames = GetDirectories(Application.persistentDataPath);
        for(int i = 0; i < directoryNames.Count; i++){
            files[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = directoryNames[i];
            files[i].SetActive(true);
        }
    }
    List<string> GetDirectories(string path)
    {
        List<string> directoryNames = new List<string>();
        if (Directory.Exists(path))
        {
            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                string directoryName = Path.GetFileName(directory);
                directoryNames.Add(directoryName);
            }
        }
        return directoryNames;
    }
    //Add New File UI Part
    public void SaveNewFile(){
        string folderName = fileNameInput.text;
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        if(Directory.Exists(folderPath)){
            description.text = "File Name Already Exists, Write New FileName.";
            description.color = Color.red;
        }
        else{
            Pairs pair = GameManager.Instance.pairList;
            BloodPacks bloodPack = GameManager.Instance.bloodPackList;
            pair.SaveFile(pair.pairs, folderName);
            bloodPack.SaveFile(bloodPack.bloodPacks, folderName);
            addNewFileUI.SetActive(false);
        }
    }
    public void AddNewFile(){
        addNewFileUI.SetActive(true);
        fileNameInput.text = "";
    }
    //Select File Part
    public void SelectFile()
    {
        string buttonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        switch (buttonName)
        {
            case "File (1)":
                selectedFile = files[0];
                break;
            case "File (2)":
                selectedFile = files[1];
                break;
            case "File (3)":
                selectedFile = files[2];
                break;
            case "File (4)":
                selectedFile = files[3];
                break;
            default:
                break;
        }
    }
}

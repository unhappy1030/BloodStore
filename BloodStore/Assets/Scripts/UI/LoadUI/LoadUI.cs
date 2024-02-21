using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;
public class LoadUI : MonoBehaviour
{
    public GameObject selectedFile;
    public List<GameObject> files;
    public GameObject addNewFileUI;
    public GameObject deleteCheckUI;
    public GameObject loadCheckUI;
    public TextMeshProUGUI description;
    public TextMeshProUGUI deleteCheckText;
    public TextMeshProUGUI loadCheckText;
    public TMP_InputField fileNameInput;
    void Start()
    {
        if(gameObject.activeSelf){
            gameObject.SetActive(false);
        }
        if(deleteCheckUI.activeSelf){
            deleteCheckUI.SetActive(false);
        }
        if(loadCheckUI.activeSelf){
            loadCheckUI.SetActive(false);
        }
        if(addNewFileUI.activeSelf){
            addNewFileUI.SetActive(false);
        }
        ShowFiles();
    }
    void ResetAllFiles(){
        foreach(GameObject file in files){
            file.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "(New Game)";
        }
    }
    //Load Part
    public void SetloadCheckUI(){
        if(selectedFile != null){
            string folderName = selectedFile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            loadCheckText.text = "Load  " + "\"" + folderName + "\"File?";
            loadCheckUI.SetActive(true);
        }
    }
    public void LoadFile(){
        if(selectedFile != null){
            string folderName = selectedFile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            string folderPath = Path.Combine(Application.persistentDataPath, folderName);
            Pairs pair = GameManager.Instance.pairList;
            BloodPacks bloodPack = GameManager.Instance.bloodPackList;
            GameManager.Instance.loadfileName = folderName;
            pair.LoadFile(folderName);
            bloodPack.LoadFile(folderName);
            gameObject.SetActive(false);
        }
    }
    public void SaveFile(){
        if(selectedFile != null){
            string folderName = selectedFile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            Pairs pair = GameManager.Instance.pairList;
            BloodPacks bloodPack = GameManager.Instance.bloodPackList;
            pair.SaveFile(pair.pairs, folderName);
            bloodPack.SaveFile(bloodPack.bloodPacks, folderName);
            selectedFile = null;
            loadCheckUI.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    //Delete Part
    public void SetDeleteCheckUI(){
        if(selectedFile != null){
            string folderName = selectedFile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            deleteCheckText.text = "Delete " + "\"" + folderName + "\"File?";
            deleteCheckUI.SetActive(true);
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
            ShowFiles();
            deleteCheckUI.SetActive(false);
        }
    }
    //Files Update
    public void ResetSelectedFile(){
        selectedFile = null;
    }
    void ShowFiles(){
        List<string> directoryNames = GetDirectories(Application.persistentDataPath);
        ResetAllFiles();
        for(int i = 0; i < directoryNames.Count; i++){
            files[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = directoryNames[i];
        }
        selectedFile = null;
    }
    List<string> GetDirectories(string path)
    {
        List<string> directoryNames = new List<string>();
        
        if (Directory.Exists(path))
        {
            string[] files = Directory.GetDirectories(path);
            List<string> directories = files.OrderBy(f => new FileInfo(f).LastWriteTime).ToList();

            foreach (string directory in directories)
            {
                string directoryName = Path.GetFileName(directory);
                directoryNames.Add(directoryName);
            }
        }
        return directoryNames;
    }
    //Add New File UI Part
    public void ResetDescription(){
        description.text = "Write New File Name";
        description.color = Color.black;
    }
    public void SaveNewFile(){
        List<string> directoryNames = GetDirectories(Application.persistentDataPath);
        string folderName = fileNameInput.text;
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        if(directoryNames.Count >= 4){
            description.text = "Please delete the other save files. You can have only four save files.";
            description.color = Color.red;
        }
        else if(Directory.Exists(folderPath)){
            description.text = "File Name Already Exists, Write New FileName.";
            description.color = Color.red;
        }
        else if(folderName == "(New Game)"){
            description.text = "The file name (New Game) cannot be used.";
            description.color = Color.red;
        }
        else{
            Pairs pair = GameManager.Instance.pairList;
            BloodPacks bloodPack = GameManager.Instance.bloodPackList;
            pair.SaveFile(pair.pairs, folderName);
            bloodPack.SaveFile(bloodPack.bloodPacks, folderName);
            ShowFiles();
            addNewFileUI.SetActive(false);
        }
    }
    public void AddNewFile(){
        fileNameInput.text = "";
        fileNameInput.Select();
        addNewFileUI.SetActive(true);
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
        if(selectedFile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == "(New Game)"){
            AddNewFile();
        }
    }

}

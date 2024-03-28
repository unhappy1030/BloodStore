using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
public class SaveUI : MonoBehaviour
{
    private EventSystem eventSystem;
    public GameObject selectedFile;
    public List<GameObject> files;
    public GameObject addNewFileUI;
    public GameObject deleteCheckUI;
    public GameObject saveCheckUI;
    public List<GameObject> buttons;
    public TextMeshProUGUI description;
    public TextMeshProUGUI deleteCheckText;
    public TextMeshProUGUI saveCheckText;
    public TMP_InputField fileNameInput;
    void Start()
    {
        if(gameObject.activeSelf){
            gameObject.SetActive(false);
        }
        if(deleteCheckUI.activeSelf){
            deleteCheckUI.SetActive(false);
        }
        if(saveCheckUI.activeSelf){
            saveCheckUI.SetActive(false);
        }
        if(addNewFileUI.activeSelf){
            addNewFileUI.SetActive(false);
        }
        MakeButtonsOff();
        ShowFiles();
        eventSystem = FindObjectOfType<EventSystem>();
    }
    void Update(){
        GameObject selectedObject = eventSystem.currentSelectedGameObject;

        // 선택된 오브젝트가 버튼인지 확인합니다.
        if (selectedObject != null && selectedObject.GetComponent<Button>() != null)
        {
            foreach(GameObject file in files){
                if(selectedObject == file){
                    Button selectedButton = selectedObject.GetComponent<Button>();
                    selectedButton.onClick.Invoke();
                }
            }
        }
    }
    void MakeButtonsOff(){
        foreach(GameObject buttonUI in buttons){
            Button button = buttonUI.GetComponent<Button>();
            button.interactable = false;
        }
    }
    void MakeButtonsOn(){
        foreach(GameObject buttonUI in buttons){
            Button button = buttonUI.GetComponent<Button>();
            button.interactable = true;
        }
    }
    //Arrow Control
    public void NowFile(){
        foreach(GameObject file in files){
            string fileName = file.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            file.transform.GetChild(1).gameObject.SetActive(false);
            if(fileName == GameManager.Instance.loadfileName){
                selectedFile = file;
                file.transform.GetChild(1).gameObject.SetActive(true);
                Button button = selectedFile.GetComponent<Button>();
                button.Select();
                button.onClick.Invoke();
            }
        }
    }
    void ResetAllFiles(){
        foreach(GameObject file in files){
            file.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "(New Game)";
        }
    }
    //Save Part
    public void SetSaveCheckUI(){
        if(selectedFile != null){
            string folderName = selectedFile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            saveCheckText.text = "Save on " + "\"" + folderName + "\"File?";
            saveCheckUI.SetActive(true);
        }
    }
    public void SaveFile(){
        if(selectedFile != null){
            string folderName = selectedFile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            GameManager.Instance.loadfileName = folderName;
            Pairs pair = GameManager.Instance.pairList;
            BloodPacks bloodPack = GameManager.Instance.bloodPackList;
            SaveData saveData = GameManager.Instance.saveData;
            pair.SaveFile(folderName);
            bloodPack.SaveFile(bloodPack.bloodPacks, folderName);
            saveData.SaveFile(folderName);
            GameManager.Instance.loadfileName = folderName;
            selectedFile = null;
            saveCheckUI.SetActive(false);
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
        MakeButtonsOff();
        selectedFile = null;
    }
    void ShowFiles(){
        List<string> directoryNames = GetDirectories(Application.persistentDataPath);
        ResetAllFiles();
        for(int i = 0; i < files.Count; i++){
            Button button = files[i].GetComponent<Button>();
            if(i < directoryNames.Count){
                files[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = directoryNames[i];
                Navigation navigation = button.navigation;
                navigation.mode = Navigation.Mode.Explicit;
                if(i != 0){
                    navigation.selectOnUp = files[i-1].GetComponent<Button>().GetComponent<Selectable>();
                }
                if(i != files.Count -1 && i + 1 < directoryNames.Count){
                    navigation.selectOnDown = files[i+1].GetComponent<Button>().GetComponent<Selectable>();
                }
                button.navigation = navigation;
                button.interactable = true;
            }
            else{
                button.interactable = false;
                button.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
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
            SaveData saveData = GameManager.Instance.saveData;
            pair.SaveFile(folderName);
            bloodPack.SaveFile(bloodPack.bloodPacks, folderName);
            saveData.SaveFile(folderName);
            ShowFiles();
            foreach(GameObject file in files){
                if(file.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == folderName){
                    selectedFile = file;
                }
            }
            addNewFileUI.SetActive(false);
            Button button = selectedFile.GetComponent<Button>();
            button.Select();
            button.onClick.Invoke();
        }
    }
    public void AddNewFile(){
        fileNameInput.text = "";
        addNewFileUI.SetActive(true);
        fileNameInput.ActivateInputField();
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
        if(selectedFile != null){
            MakeButtonsOn();
        }
    }
}

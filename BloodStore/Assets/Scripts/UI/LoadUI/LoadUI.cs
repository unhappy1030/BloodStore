using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class LoadUI : MonoBehaviour
{
    private EventSystem eventSystem;
    public GameObject selectedFile;
    public List<GameObject> files;
    public GameObject loadCheckUI;
    public List<GameObject> buttons;
    public TextMeshProUGUI loadCheckText;
    void Start()
    {
        if(gameObject.activeSelf){
            gameObject.SetActive(false);
        }
        if(loadCheckUI.activeSelf){
            loadCheckUI.SetActive(false);
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
    void ResetAllFiles(){
        foreach(GameObject file in files){
            file.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "(Empty)";
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
            SaveData saveData = GameManager.Instance.saveData;
            GameManager.Instance.loadfileName = folderName;
            pair.LoadFile(folderName);
            bloodPack.LoadFile(folderName);
            saveData.LoadFile(folderName);
            gameObject.SetActive(false);
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
            }
            else{
                button.interactable = false;
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

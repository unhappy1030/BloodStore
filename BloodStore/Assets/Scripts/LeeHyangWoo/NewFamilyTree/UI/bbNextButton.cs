using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class bbNextButton : MonoBehaviour
{
    public void OnButtonClick(){
        SceneManager.LoadScene("NewFamilyTreeTest");
    }
}

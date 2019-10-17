using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI salvageText;
    public int currentSalvage;


    void Start()
    {
        salvageText.text = " " + currentSalvage;
    }


    void Update()
    {
        salvageText.text = " " + currentSalvage;


    }



    public void PlayGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    
    public void OpenPanel()
    {
        SceneManager.LoadScene("Shop");
    }
     public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

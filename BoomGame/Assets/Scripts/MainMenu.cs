using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI salvageText;
    


    void Start()
    {
        salvageText.text = "" + GameMaster.currentSalvage;
    }


    void Update()
    {
        salvageText.text = "" + GameMaster.currentSalvage;


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
     public void OpenCampaing()
    {
        SceneManager.LoadScene("CampaingMap");
    }
     public void OpenLevel_02()
    {
        SceneManager.LoadScene("Level_02");
    }
     public void OpenLevel_03()
    {
        SceneManager.LoadScene("Level_03");
    }
     public void OpenLevel_04()
    {
        SceneManager.LoadScene("Level_04");
    }
     public void OpenLevel_05()
    {
        SceneManager.LoadScene("Level_05");
    }
     public void OpenLevel_06()
    {
        SceneManager.LoadScene("Level_06");
    }
     public void OpenLevel_07()
    {
        SceneManager.LoadScene("Level_07");
    }
     public void OpenLevel_08()
    {
        SceneManager.LoadScene("Level_08");
    }

}

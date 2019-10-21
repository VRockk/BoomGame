using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI salvageText;

    public GameMaster gameMaster;

    public GameObject privacyPolicyPanel;
    public GameObject mainMenuPanel;
    public GameObject campaignMapPanel;
    public GameObject shopPanel;

    bool showPrivacyPolicy = true;

    void Start()
    {
        UpdateSalvage();
        if (mainMenuPanel != null && privacyPolicyPanel != null)
        {
            if (showPrivacyPolicy)
            {
                mainMenuPanel.SetActive(false);
                privacyPolicyPanel.SetActive(true);
            }
            else
            {
                mainMenuPanel.SetActive(true);
                privacyPolicyPanel.SetActive(false);
            }
        }
    }


    void Update()
    {
        //Dont update text on every update tick. Instead they should only be updated when the value is changed
        //salvageText.text = "" + GameMaster.currentSalvage;


    }



    public void PlayGame()
    {
        //Starts the next level available in progression


        SceneManager.LoadScene("Tutorial");
    }

    public void OpenPanel()
    {
        SceneManager.LoadScene("Shop");
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void OpenCampaign()
    {
        SceneManager.LoadScene("CampaignMap");
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

    public void OpenLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void UpdateSalvage()
    {
        if (gameMaster != null)
            salvageText.text = "" + gameMaster.currentSalvage;
    }

    public void AcceptPrivacyPolicy()
    {
        mainMenuPanel.SetActive(true);
        privacyPolicyPanel.SetActive(false);
    }
    public void CloseCampaingMap()
    {
        mainMenuPanel.SetActive(true);
        campaignMapPanel.SetActive(false);
    }
    public void OpenCampaignMap()
    {
        mainMenuPanel.SetActive(false);
        campaignMapPanel.SetActive(true);
    }
    public void CloseShop()
    {
        mainMenuPanel.SetActive(true);
        shopPanel.SetActive(false);
    }
    public void OpenShop()
    {
        mainMenuPanel.SetActive(false);
        shopPanel.SetActive(true);
    }
}

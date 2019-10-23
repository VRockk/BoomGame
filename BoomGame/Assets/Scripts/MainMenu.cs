using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI salvageText;

    public GameMaster gameMaster;

    public GameObject privacyPolicyPanel;
    public GameObject mainMenuPanel;
    public GameObject campaignMapPanel;
    public GameObject shopPanel;


    void Start()
    {
        UpdateSalvage();
        if (privacyPolicyPanel != null)
        {
            if (!gameMaster.PrivacyPolicyAccepted)
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


    public void UpdateSalvage()
    {
        if (gameMaster != null)
            salvageText.text = "" + gameMaster.currentSalvage;
    }

    public void AcceptPrivacyPolicy()
    {
        mainMenuPanel.SetActive(true);
        privacyPolicyPanel.SetActive(false);
        gameMaster.PrivacyPolicyAccepted = true;
    }

    public void OpenPrivacyPolicy()
    {
        mainMenuPanel.SetActive(false);
        shopPanel.SetActive(false);
        privacyPolicyPanel.SetActive(true);
    }

    public void CloseCampaingMap()
    {
        mainMenuPanel.SetActive(true);
        campaignMapPanel.SetActive(false);
        shopPanel.SetActive(false);
    }
    public void OpenCampaignMap()
    {
        mainMenuPanel.SetActive(false);
        campaignMapPanel.SetActive(true);
        shopPanel.SetActive(false);
    }
    public void CloseShop()
    {
        mainMenuPanel.SetActive(true);
        shopPanel.SetActive(false);
        campaignMapPanel.SetActive(false);
    }
    public void OpenShop()
    {
        mainMenuPanel.SetActive(false);
        shopPanel.SetActive(true);
        campaignMapPanel.SetActive(false);
    }

}

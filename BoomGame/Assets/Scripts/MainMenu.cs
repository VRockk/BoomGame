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


    public GameObject privacyPolicyPanel;
    public GameObject mainMenuPanel;
    public GameObject campaignMapPanel;
    public GameObject shopPanel;
    public GameObject bombPanel;
    public GameObject bombSelectionPanel;
    public GameObject gameServicesPanel;

    public GameObject gameMasterPrefab;

    private GameMaster gameMaster;
    private AudioSource audioSource;
    public AudioClip menuMusic;


    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null)
        {
            gameMaster = Instantiate(gameMasterPrefab).GetComponent<GameMaster>();
        }

        if (menuMusic != null)
            gameMaster.SetMusic(menuMusic);

        UpdateSalvage();
        if (privacyPolicyPanel != null)
        {
            if (!gameMaster.PrivacyPolicyAccepted)
            {
                mainMenuPanel.SetActive(false);
                privacyPolicyPanel.SetActive(true);
                campaignMapPanel.SetActive(false);
                shopPanel.SetActive(false);
                bombPanel.SetActive(false);
                gameServicesPanel.SetActive(false);
            }
            else
            {
                gameServicesPanel.SetActive(true);   
                mainMenuPanel.SetActive(true);
                privacyPolicyPanel.SetActive(false);
                campaignMapPanel.SetActive(false);
                shopPanel.SetActive(false);
                bombPanel.SetActive(false);
            }
        }
        audioSource = GetComponent<AudioSource>();
    }



    void Update()
    {
        //Dont update text on every update tick. Instead they should only be updated when the value is changed
        //salvageText.text = "" + GameMaster.currentSalvage;


    }



    public void PlayGame()
    {
        SceneManager.LoadScene("CampaignMap");




        ////Starts the next level available in progression

        //int levelReached = PlayerPrefs.GetInt("LevelReached", 1);
        //if (levelReached == 1)
        //    SceneManager.LoadScene("Tutorial");
        //else
        //{
        //    //Paskaa
        //    if (levelReached < 10)
        //        SceneManager.LoadScene("Level_0" + levelReached.ToString());
        //    else
        //        SceneManager.LoadScene("Level_" + levelReached.ToString());

        //}
    }


    public void UpdateSalvage()
    {
        if (gameMaster != null)
            salvageText.text = "" + gameMaster.currentSalvage;
    }

    public void AcceptPrivacyPolicy()
    {
        gameServicesPanel.SetActive(true);
        mainMenuPanel.SetActive(true);
        privacyPolicyPanel.SetActive(false);
        campaignMapPanel.SetActive(false);
        shopPanel.SetActive(false);
        bombPanel.SetActive(false);
        bombSelectionPanel.SetActive(true);

        gameMaster.PrivacyPolicyAccepted = true;
    }
    public void OpenPrivacyPolicy()
    {
        mainMenuPanel.SetActive(false);
        shopPanel.SetActive(false);
        campaignMapPanel.SetActive(false);
        privacyPolicyPanel.SetActive(true);
        bombPanel.SetActive(false);
        bombSelectionPanel.SetActive(true);
    }

    public void CloseCampaingMap()
    {
        mainMenuPanel.SetActive(true);
        campaignMapPanel.SetActive(false);
        privacyPolicyPanel.SetActive(false);
        shopPanel.SetActive(false);
        bombPanel.SetActive(false);
        bombSelectionPanel.SetActive(true);
    }
    public void OpenCampaignMap()
    {
        mainMenuPanel.SetActive(false);
        privacyPolicyPanel.SetActive(false);
        campaignMapPanel.SetActive(true);
        shopPanel.SetActive(false);
        bombPanel.SetActive(false);
        bombSelectionPanel.SetActive(true);
    }
    public void CloseShop()
    {
        mainMenuPanel.SetActive(true);
        privacyPolicyPanel.SetActive(false);
        shopPanel.SetActive(false);
        campaignMapPanel.SetActive(false);
        bombPanel.SetActive(false);
        bombSelectionPanel.SetActive(true);
    }
    public void OpenShop()
    {
        mainMenuPanel.SetActive(false);
        privacyPolicyPanel.SetActive(false);
        shopPanel.SetActive(true);
        campaignMapPanel.SetActive(false);
        bombPanel.SetActive(false);
        bombSelectionPanel.SetActive(true);

    }
    public void OpenBombsPanel()
    {
        //mainMenuPanel.SetActive(false);
        privacyPolicyPanel.SetActive(false);
        shopPanel.SetActive(false);
        campaignMapPanel.SetActive(false);
        bombPanel.SetActive(true);
        bombSelectionPanel.SetActive(true);
        var upgradePanels = GameObject.FindGameObjectsWithTag("BombUpgradePanel");
        foreach (var upgradePanel in upgradePanels)
        {
            upgradePanel.SetActive(false);
        }
    }
    public void CloseBombsPanel()
    {
        var upgradePanels = GameObject.FindGameObjectsWithTag("BombUpgradePanel");
        foreach (var upgradePanel in upgradePanels)
        {
            upgradePanel.SetActive(false);
        }
        mainMenuPanel.SetActive(true);
        privacyPolicyPanel.SetActive(false);
        shopPanel.SetActive(false);
        campaignMapPanel.SetActive(false);
        bombPanel.SetActive(false);
        bombSelectionPanel.SetActive(false);
    }
    public void CloseGameServices()
    {
        mainMenuPanel.SetActive(true);
        privacyPolicyPanel.SetActive(false);
        shopPanel.SetActive(false);
        campaignMapPanel.SetActive(false);
        bombPanel.SetActive(false);
        bombSelectionPanel.SetActive(true);
        gameServicesPanel.SetActive(false);
    }
    public void ShowBombUpgradePanel(GameObject panel)
    {
        var upgradePanels = GameObject.FindGameObjectsWithTag("BombUpgradePanel");
        foreach (var upgradePanel in upgradePanels)
        {
            upgradePanel.SetActive(false);
        }
        bombSelectionPanel.SetActive(false);
        panel.SetActive(true);
    }

    public void PlaySound(AudioClip sound)
    {

        audioSource.PlayOneShot(sound);
    }

}

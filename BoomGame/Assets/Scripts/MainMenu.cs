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
    public GameObject shopPanel;
    public GameObject workshopPanel;
    public GameObject bombSelectionPanel;
    public GameObject optionPanel;
    public GameObject gameMasterPrefab;
    public GameObject gameServicesPanel;
    public GameObject bombSalvage;

    private GameMaster gameMaster;
    public AudioSource audioSource;
    [HideInInspector]
    public AudioClip menuMusic;
    private bool rumbleToggleFlag = true;

    private bool mWaitingForAuth = false;



    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null)
        {
            gameMaster = Instantiate(gameMasterPrefab).GetComponent<GameMaster>();
        }

        gameServicesPanel.SetActive(false);

        UpdateSalvage();
        if (privacyPolicyPanel != null)
        {
            if (!gameMaster.PrivacyPolicyAccepted)
            {
                mainMenuPanel.SetActive(false);
                privacyPolicyPanel.SetActive(true);
                shopPanel.SetActive(false);
            }
            else
            {
                mainMenuPanel.SetActive(true);
                privacyPolicyPanel.SetActive(false);
                shopPanel.SetActive(false);
            }


            if (gameMaster.signIn == true)
            {
                if (gameServicesPanel != null)
                {
                    gameServicesPanel.SetActive(false);
                }
                if (!Social.localUser.authenticated)
                {
                    Social.localUser.Authenticate((bool success) =>
                    {

                    });
                }
            }
            else
            {
                if (gameServicesPanel != null)
                {
                    gameServicesPanel.SetActive(false);
                }
            }
        }

        // Select the Google Play Games platform as our social platform implementation
        GooglePlayGames.PlayGamesPlatform.Activate();
    }



    void Update()
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("CampaignMap");
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
        shopPanel.SetActive(false);
        bombSalvage.SetActive(true);
        gameMaster.PrivacyPolicyAccepted = true;
    }

    public void CloseShop()
    {
        mainMenuPanel.SetActive(true);
        privacyPolicyPanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void OpenShop()
    {
        mainMenuPanel.SetActive(false);
        privacyPolicyPanel.SetActive(false);
        shopPanel.SetActive(true);

    }

    public void CloseWorkshop()
    {
        mainMenuPanel.SetActive(true);
        privacyPolicyPanel.SetActive(false);
        workshopPanel.SetActive(false);
    }

    public void OpenWorkshop()
    {
        mainMenuPanel.SetActive(false);
        privacyPolicyPanel.SetActive(false);
        workshopPanel.SetActive(true);

    }

    public void OpenBombsPanel()
    {
        //mainMenuPanel.SetActive(false);
        privacyPolicyPanel.SetActive(false);
        shopPanel.SetActive(false);
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
        bombSelectionPanel.SetActive(false);
    }

    public void OnLoginButtonClick()
    {
        if (!Social.localUser.authenticated)
        {
            // Authenticate
            mWaitingForAuth = true;
            //statusText.text = "Authenticating...";

            Social.localUser.Authenticate((bool success) =>
            {
                mWaitingForAuth = false;
                if (success)
                {
                    //statusText.text = "Welcome " + Social.localUser.userName;
                    //StartCoroutine("LoadImage");
                    //loginButton.GetComponentInChildren<TMP_Text>().text = "Sign out";
                    gameServicesPanel.SetActive(false);
                    gameMaster.SignIn = true;
                }
                else
                {
                    //statusText.text = "Authentication failed.";
                }
            });
        }
        else
        {
            //statusText.text = "";
            //((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
            //loginButton.GetComponentInChildren<TMP_Text>().text = "Sign in";
        }

    }
    public void CloseGameServices()
    {
        //mainMenuPanel.SetActive(true);
        //privacyPolicyPanel.SetActive(false);
        //shopPanel.SetActive(false);
        gameServicesPanel.SetActive(false);
    }
    public void ShowBombUpgradePanel(GameObject panel)
    {
        var upgradePanels = GameObject.FindGameObjectsWithTag("BombUpgradePanel");
        foreach (var upgradePanel in upgradePanels)
        {
            upgradePanel.SetActive(false);
        }
        panel.SetActive(true);
    }


    public void OpenOptionPanel()
    {
        optionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
    }

    public void PlaySound(AudioClip sound)
    {
        if (sound != null)
            audioSource.PlayOneShot(sound);
    }


    public void OnChangeRumble()
    {
        Toggle rumbleToggle = (Toggle)FindObjectOfType(typeof(Toggle));
        if (rumbleToggle.isOn)
        {
            rumbleToggleFlag = true;
            Debug.Log("switch is on");
        }
        else
        {
            rumbleToggleFlag = false;
            Debug.Log("switch is off");
        }
    }
}

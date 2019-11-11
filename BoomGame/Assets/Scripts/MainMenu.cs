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
    public GameObject optionPanel;
    public GameObject gameServicesPanel;
    public GameObject gameMasterPrefab;

    private GameMaster gameMaster;
    private AudioSource audioSource;
    public AudioClip menuMusic;
    private bool rumbleToggleFlag = true;

    [SerializeField] private Button loginButton;
    [SerializeField] private TextMeshProUGUI statusText;
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

        if (gameServicesPanel != null)
            {
                if (!gameMaster.SignIn)
                {
                    mainMenuPanel.SetActive(true);
                    privacyPolicyPanel.SetActive(false);
                    campaignMapPanel.SetActive(false);
                    shopPanel.SetActive(false);
                    bombPanel.SetActive(false);
                    gameServicesPanel.SetActive(true);

                }
                else
                {
                    mainMenuPanel.SetActive(true);
                    privacyPolicyPanel.SetActive(false);
                    campaignMapPanel.SetActive(false);
                    shopPanel.SetActive(false);
                    bombPanel.SetActive(false);
                    gameServicesPanel.SetActive(false);


                }
            }

            if (gameMaster.SignIn == true)
            {
                if (!Social.localUser.authenticated)
                {
                    // Authenticate
                    mWaitingForAuth = true;
                    statusText.text = "Authenticating...";

                    Social.localUser.Authenticate((bool success) =>
                    {
                        mWaitingForAuth = false;
                        if (success)
                        {
                            statusText.text = "Welcome " + Social.localUser.userName;
                            StartCoroutine("LoadImage");
                            loginButton.GetComponentInChildren<TMP_Text>().text = "Sign out";
                            gameServicesPanel.SetActive(false);
                            gameMaster.SignIn = true;
                        }
                        else
                        {
                            statusText.text = "Authentication failed.";
                        }
                    });

                }
            }
        }
        audioSource = GetComponent<AudioSource>();

        // Select the Google Play Games platform as our social platform implementation
        GooglePlayGames.PlayGamesPlatform.Activate();

        //playButton.interactable = false;
        if (!Social.localUser.authenticated)
        {
            loginButton.GetComponentInChildren<TMP_Text>().text = "Sign in";
        }
    }



    void Update()
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("CampaignMap");


        //int tutorial = PlayerPrefs.GetInt("Tutorial", 0);

        //if (tutorial == 1)
        //    SceneManager.LoadScene("CampaignMap");
        //else
        //{
        //    PlayerPrefs.SetInt("Tutorial", 1);
        //    SceneManager.LoadScene("TutorialLevel");
        //}
    }


    public void UpdateSalvage()
    {
        if (gameMaster != null)
            salvageText.text = "" + gameMaster.currentSalvage;
    }

    public void AcceptPrivacyPolicy()
    {
        //gameServicesPanel.SetActive(true);
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

    public void OnLoginButtonClick()
    {
        if (!Social.localUser.authenticated)
        {
            // Authenticate
            mWaitingForAuth = true;
            statusText.text = "Authenticating...";

            Social.localUser.Authenticate((bool success) =>
            {
                mWaitingForAuth = false;
                if (success)
                {
                    statusText.text = "Welcome " + Social.localUser.userName;
                    StartCoroutine("LoadImage");
                    loginButton.GetComponentInChildren<TMP_Text>().text = "Sign out";
                    gameServicesPanel.SetActive(false);
                    gameMaster.SignIn = true;
                }
                else
                {
                    statusText.text = "Authentication failed.";
                }
            });
        }
        else
        {
            statusText.text = "";
            ((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
            loginButton.GetComponentInChildren<TMP_Text>().text = "Sign in";
        }

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


    public void OpenOptionPanel()
    {
        mainMenuPanel.SetActive(false);
        privacyPolicyPanel.SetActive(false);
        optionPanel.SetActive(true);
        campaignMapPanel.SetActive(false);
        bombPanel.SetActive(false);
        bombSelectionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        mainMenuPanel.SetActive(true);
        privacyPolicyPanel.SetActive(false);
        optionPanel.SetActive(false);
        campaignMapPanel.SetActive(false);
        bombPanel.SetActive(false);
        bombSelectionPanel.SetActive(true);
    }
    public void PlaySound(AudioClip sound)
    {
        if (rumbleToggleFlag == true)
        {
            audioSource.PlayOneShot(sound);
        }

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

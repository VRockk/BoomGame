using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

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
    public Button loginButton;
    public TextMeshProUGUI loginText;

    private GameMaster gameMaster;
    public AudioSource audioSource;
    [HideInInspector]
    public AudioClip menuMusic;
    private bool rumbleToggleFlag = true;




    void Start()
    {
        // Select the Google Play Games platform as our social platform implementation
        GooglePlayGames.PlayGamesPlatform.Activate();

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

            if (!gameMaster.loginShowed)
            {
                if (gameMaster.SignIn == true)
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
                        gameMaster.loginShowed = true;
                        gameServicesPanel.SetActive(true);
                    }
                }
            }
        }


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
        gameServicesPanel.SetActive(true);
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
            loginText.text = "Authenticating...";
            loginButton.interactable = false;
            var buttonText = loginButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
                buttonText.color = new Color(1f, 1f, 1f, 0.2f);
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    loginText.text = "Authentication succesful";
                    //var canvasGroup = gameServicesPanel.GetComponent<CanvasGroup>();
                    //if (canvasGroup != null)
                    //    canvasGroup.DOFade(0f, 1f);
                    //else
                    gameServicesPanel.SetActive(false);
                    gameMaster.SignIn = true;
                }
                else
                {
                    loginText.text = "Authentication failed";

                }
            });
        }
        else
        {
            gameServicesPanel.SetActive(false);
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

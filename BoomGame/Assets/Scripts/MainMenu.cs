using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI salvageText;


    public GameObject privacyPolicyPanel;
    public GameObject mainMenuPanel;
    public GameObject shopPanel;
    public GameObject optionPanel;
    public GameObject workshopPanel;
    public GameObject gameMasterPrefab;
    public GameObject gameServicesPanel;
    public GameObject bombSalvage;
    public GameObject lootBoxPanel;
    public GameObject lootBoxButton;
    public TextMeshProUGUI lootBoxButtonText;
    public Image lootboxImage;
    public Button loginButton;
    public TextMeshProUGUI loginText;

    private GameMaster gameMaster;
    public AudioSource audioSource;

    public GameObject lootBoxAnim;


    [HideInInspector]
    public AudioClip menuMusic;
    private bool rumbleToggleFlag = true;


    private bool lootBoxAvailable = false;

    private DateTime lootBoxAvailableTime;
    private DateTime timeNow;

    private string timeFormat = "MM.dd.yyyy HH.mm.ss";


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
        lootBoxPanel.SetActive(false);

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




        timeNow = UtilityLibrary.GetNetTime();
        string lootBoxTime = PlayerPrefs.GetString("LootBoxTime");
        //print(lootBoxTime);
        //If lootbox time is not set then we opened game first time. Let user open box right away
        if (lootBoxTime == "")
        {
            lootBoxAvailableTime = timeNow.Add(new TimeSpan(0, 0, 0, -1));
            PlayerPrefs.SetString("LootBoxTime", lootBoxAvailableTime.ToString(timeFormat));
            //string lootBoxTime = ;
            //print(PlayerPrefs.GetString("LootBoxTime"));
            LootboxNotAvailable();
        }
        else
        {
            lootBoxAvailableTime = DateTime.ParseExact(lootBoxTime, timeFormat, System.Globalization.CultureInfo.InvariantCulture);

            if (lootBoxAvailableTime < timeNow)
            {
                LootboxAvailable();

            }
            else
            {
                LootboxNotAvailable();
            }
        }
        //print(lootBoxAvailableTime);
    }
    Tweener lootBoxShakePos;
    Tweener lootBoxShakeRot;
    private void LootboxNotAvailable()
    {
        if (lootBoxShakePos != null)
            lootBoxShakePos.Kill();
        if (lootBoxShakeRot != null)
            lootBoxShakeRot.Kill();

        Sprite image = Resources.Load<Sprite>("lootbox_grey");
        lootboxImage.sprite = image;
        lootBoxAvailable = false;
        lootBoxButton.GetComponent<Button>().interactable = false;
        InvokeRepeating("CheckLootBoxTime", 1f, 1f);

    }

    private void LootboxAvailable()
    {
        if (lootBoxShakePos != null)
            lootBoxShakePos.Kill();
        if (lootBoxShakeRot != null)
            lootBoxShakeRot.Kill();
        Sprite image = Resources.Load<Sprite>("lootbox_closed");
        lootboxImage.sprite = image;
        lootBoxAvailable = true;
        lootBoxButtonText.text = "LOOTBOX AVAILABLE";
        lootBoxButton.GetComponent<Button>().interactable = true;

        lootBoxShakePos = lootboxImage.gameObject.transform.DOShakePosition(1f, 2, 15, 45, false, false).SetLoops(-1);
        lootBoxShakeRot = lootboxImage.gameObject.transform.DOShakeRotation(1f, 5, 10, 45, false).SetLoops(-1);

    }

    private void CheckLootBoxTime()
    {

        timeNow = timeNow.Add(new TimeSpan(0, 0, 0, 1));
        //print(newTime);
        //print(timeNow);

        if (lootBoxAvailableTime < timeNow)
        {
            lootBoxAvailableTime = UtilityLibrary.GetNetTime();
            //Just making sure
            if (lootBoxAvailableTime < timeNow.Add(new TimeSpan(0, 0, 0, 5)))
            {
                LootboxAvailable();
                CancelInvoke("CheckLootBoxTime");
            }

        }
        else
        {
            var availableIn = lootBoxAvailableTime - timeNow;
            int hours = (int)availableIn.TotalHours;

            lootBoxButtonText.text = "AVAILABLE IN " + hours.ToString("00") + ":" + availableIn.ToString("mm") + ":" + availableIn.ToString("ss");
        }
    }



    void Update()
    {
    }

    public void PlayGame()
    {
        if (PlayerPrefs.GetInt("TutorialPassed", 0) == 1)
            SceneManager.LoadScene("CampaignMap");
        else
            SceneManager.LoadScene("Level_01");
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

    public void OnClickLeaderBoardButton()
    {
        if (Social.localUser.authenticated)
        {
            string lbId = "CgkI65f98LAPEAIQAQ";
            GooglePlayGames.PlayGamesPlatform.Instance.SetDefaultLeaderboardForUI(lbId);
            Social.Active.ShowLeaderboardUI();
        }
    }

    public void CloseLootBoxPanel()
    {
        lootBoxPanel.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        lootBoxPanel.GetComponent<CanvasGroup>().interactable = false;
        lootBoxPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        lootBoxPanel.SetActive(false);


    }

    public void OpenDailyLootbox()
    {
        timeNow = UtilityLibrary.GetNetTime();


        //Add day to open new loot box
        lootBoxAvailableTime = timeNow.Add(new TimeSpan(1, 0, 0, 0));
        PlayerPrefs.SetString("LootBoxTime", lootBoxAvailableTime.ToString(timeFormat));
        var timeToOpen = lootBoxAvailableTime - timeNow;
        //print((int)timeToOpen.TotalSeconds);
        //Create notification for user when box can be opened
        PushNotification lootboxNotification = new PushNotification("LootBoxNotificationId", (int)timeToOpen.TotalSeconds, "OK Boomer", "Claim your free daily Salvage! Time to get back to blowing stuff up.");




        LootboxNotAvailable();


        //Show loot box opening animation and stuffs....
        LootBoxOpening();
    }

    private void LootBoxOpening()
    {
        int salvageGain = UnityEngine.Random.Range(300, 600);

        lootBoxPanel.SetActive(true);
        lootBoxPanel.GetComponent<CanvasGroup>().interactable = true;
        lootBoxPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        lootBoxPanel.GetComponent<CanvasGroup>().DOFade(1f, 1f);

        var lootBoxBG = GameObject.Find("LootBoxBG");
        var salvageGainText = GameObject.Find("SalvageGain").GetComponent<TextMeshProUGUI>();
        salvageGainText.text = salvageGain.ToString();
        lootBoxBG.GetComponent<RectTransform>().localScale = new Vector3(0.01f, 0.01f, 0.1f);
        var lootbox = Instantiate(lootBoxAnim, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), null);
        lootbox.transform.localScale = new Vector3(0f, 0f, 1);

        float animDelay = lootbox.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - 1.2f;

        var lootboxtween = DOTween.Sequence()
            .Append(lootbox.transform.DOScale(3f, 0.5f).SetEase(Ease.InExpo).SetUpdate(true))
                        .Insert(animDelay, lootbox.transform.DOScale(0, 1f).SetEase(Ease.OutExpo).SetUpdate(true));

        gameMaster.AddSalvage(salvageGain);

        DOTween.Sequence()
            .Append(lootBoxBG.GetComponent<RectTransform>().DOScale(1f, 1f).SetEase(Ease.InBack).SetUpdate(true).SetDelay(animDelay - 1f)).OnComplete(UpdateSalvage);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            timeNow = UtilityLibrary.GetNetTime();
        }
    }

}

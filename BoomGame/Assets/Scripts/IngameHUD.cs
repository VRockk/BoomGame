using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class IngameHUD : MonoBehaviour
{

    public Sprite detonatorUp;
    public Sprite detonatorDown;

    private GameController gameController;

    public GameObject detonatePanel;
    public GameObject roundPanel;
    public GameObject levelFinishPanel;
    public GameObject bombPanel;
    public GameObject mainMenuButton;
    public GameObject detonateButton;
    public GameObject resetButton;
    public GameObject nextLevelButton;
    public GameObject salvagePanel;

    public GameObject bombCardPrefab;

    public GameObject endScreen;
    public AudioClip menuMusic;
    private GameMaster gameMaster;
    private CameraHandler cameraHandler;

    private TextMeshProUGUI bombCountText;

    private Canvas canvas;

    private int salvageAmount = 0;
    private int bonusSalvage = 0;
    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = GameObject.FindObjectOfType<GameMaster>();
        gameController = GameObject.FindObjectOfType<GameController>();
        cameraHandler = GameObject.FindObjectOfType<CameraHandler>();

        if (gameController == null)
            Debug.LogError("GameController not found in the scene for the IngameHUD");

        if (cameraHandler == null)
            Debug.LogError("CameraHandler not found in the scene for the IngameHUD");


        detonatePanel.SetActive(true);

        roundPanel.SetActive(false);

        bombPanel.SetActive(true);

        mainMenuButton.SetActive(true);

        levelFinishPanel.SetActive(false);

        bombCountText = GameObject.Find("BombCount").GetComponent<TextMeshProUGUI>();
        bombCountText.text = gameController.bombCount.ToString();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DetonateAllBombs()
    {
        if (gameController.Detonation())
        {
            detonateButton.GetComponent<Image>().sprite = detonatorDown;
        }
    }

    public void ResetLevel()
    {
        //TODO: Show loading screens
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        //Application.LoadLevel();
    }

    public void NextLevel()
    {
        //To load video ads
        //AdsController.adsInstance.ShowVideoOrInterstitialAds();
        //TODO: Show loading screens
        SceneManager.LoadSceneAsync(gameController.nextLevelName, LoadSceneMode.Single);
        //Application.LoadLevel();
    }

    public void NextRound(int roundNumber, float delay)
    {
        detonatePanel.SetActive(true);
        detonateButton.GetComponent<Image>().sprite = detonatorUp;

        bombPanel.SetActive(true);

        levelFinishPanel.SetActive(false);

        ShowRoundText(delay, roundNumber);
    }

    public void LevelFinished(LevelClear levelClear, int salvage, int bonus)
    {
        salvageAmount = salvage;
        bonusSalvage = bonus;

        detonatePanel.SetActive(false);
        bombPanel.SetActive(false);
        mainMenuButton.SetActive(false);
        resetButton.SetActive(true);
        nextLevelButton.SetActive(true);




        if (levelClear == LevelClear.Failed)
        {
            //nextLevelButton.SetActive(true);
            nextLevelButton.GetComponent<Button>().interactable = false;
            nextLevelButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0.35f, 0.35f, 0.35f, 1);
        }

        ShowEndScreen(levelClear);

    }

    private void ShowEndScreen(LevelClear levelClear)
    {
        if (endScreen != null)
        {
            var endScreenClone = Instantiate(endScreen);
            var endscreenScript = endScreenClone.GetComponent<EndScreen>();

            salvagePanel.SetActive(true);

            if (levelClear == LevelClear.Failed)
            {
                endscreenScript.Enable(0);
                salvagePanel.SetActive(false);
                //salvagePanel.GetComponent<CanvasGroup>().alpha = 0;
                //salvagePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else if (levelClear == LevelClear.OnePentagram)
            {
                endscreenScript.Enable(1);
            }
            else if (levelClear == LevelClear.TwoPentagram)
            {
                endscreenScript.Enable(2);
            }
            else if (levelClear == LevelClear.ThreePentagram)
            {
                endscreenScript.Enable(3);
            }
        }
    }

    public void ShowFinishPanel()
    {
        levelFinishPanel.SetActive(true);
        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 1;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (salvagePanel.activeInHierarchy)
        {
            var salvageGain = GameObject.Find("SalvageGain");
            var salvageBonusGain = GameObject.Find("SalvageBonusGain");
            var salvageBonus = GameObject.Find("SalvageBonus");

            salvageGain.SetActive(true);
            salvageBonus.SetActive(false);
            salvageBonusGain.SetActive(false);
            salvageGain.GetComponent<TextMeshProUGUI>().text = salvageAmount.ToString();

            if (bonusSalvage > 0)
            {
                salvageBonus.SetActive(true);
                salvageBonusGain.SetActive(true);
                salvageBonusGain.GetComponent<TextMeshProUGUI>().text = bonusSalvage.ToString();
            }
        }

    }

    public void UpdateBombCount(int bombCount)
    {
        bombCountText.text = bombCount.ToString();
    }

    private void ShowRoundText(float hideDelay, int roundNumber)
    {
        roundPanel.SetActive(true);

        TextMeshProUGUI text = GameObject.Find("RoundText").GetComponent<TextMeshProUGUI>();
        text.text = "Round " + roundNumber;


        StartCoroutine(ActivateObjectWithDelay(hideDelay, roundPanel, false));
    }

    private IEnumerator ActivateObjectWithDelay(float delay, GameObject panel, bool active)
    {
        yield return new WaitForSeconds(delay);
        panel.SetActive(active);
    }

    public void OpenMainMenu()
    {
        if(menuMusic != null)
            gameMaster.SetMusic(menuMusic);
        else
            gameMaster.SetMusic(null);
        gameMaster.SetMusic(menuMusic);
        SceneManager.LoadScene("MainMenuScene");
    }

    public void CreateBombCard(GameObject bomb)
    {
        if (bomb == null)
            return;

        //Get current bomb cards and add position offset for each one
        float leftOffset = 0;
        var bombCards = GameObject.FindGameObjectsWithTag("BombCard");
        foreach (var bombCard in bombCards)
        {
            var bombCardTransform = bombCard.GetComponent<RectTransform>();
            leftOffset += bombCardTransform.sizeDelta.x + 5f;
        }

        //Create new bomb card and set sprite, size and positioning
        var card = Instantiate(bombCardPrefab);
        card.transform.SetParent(bombPanel.transform);
        var cardTransform = card.GetComponent<RectTransform>();

        var cardImage = card.GetComponentInChildren<Image>();
        cardImage.sprite = bomb.GetComponent<Bomb>().inventoryIcon;

        //Set sprite aspect ratio so different size icons fit correcly
        var cardAspectRatioFitter = card.GetComponent<AspectRatioFitter>();
        cardAspectRatioFitter.aspectRatio = cardImage.sprite.rect.width / cardImage.sprite.rect.height;

        cardTransform.localPosition = new Vector3(leftOffset, 0f, 0);
        cardTransform.localScale = new Vector3(1, 1, 1);

        var bombCardScript = card.GetComponent<BombCard>();
        bombCardScript.bombPrefab = bomb;
    }
}

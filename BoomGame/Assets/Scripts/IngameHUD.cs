using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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
    public GameObject resetButton1;
    public GameObject detonateButton;
    public GameObject resetButton2;
    public GameObject campaignMapButton;
    public GameObject nextLevelButton;
    public GameObject salvagePanel;
    public GameObject scorePanel;
    public GameObject bombCardPrefab;
    public GameObject progressPanel;
    public Slider slider;

    public GameObject endScreen;
    public AudioClip menuMusic;
    private GameMaster gameMaster;
    private CameraHandler cameraHandler;

    private TextMeshProUGUI bombCountText;

    private Canvas canvas;

    private int salvageAmount = 0;



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

        UpdateScore(0);

        detonatePanel.SetActive(true);

        roundPanel.SetActive(true);

        bombPanel.SetActive(true);

        mainMenuButton.SetActive(true);

        resetButton1.SetActive(true);

        levelFinishPanel.SetActive(false);

        scorePanel.SetActive(true);



        bombCountText = GameObject.Find("BombCount").GetComponent<TextMeshProUGUI>();
        bombCountText.text = gameController.bombCount.ToString();

        detonatePanel.GetComponent<RectTransform>().DOAnchorPosX(230f, 0.0f, true).SetEase(Ease.InBack).SetUpdate(true);
        resetButton1.GetComponent<RectTransform>().DOAnchorPosX(230f, 0.0f, true).SetEase(Ease.InBack).SetUpdate(true);
        mainMenuButton.GetComponent<RectTransform>().DOAnchorPosX(-230f, 0.0f, true).SetEase(Ease.InBack).SetUpdate(true);
        bombPanel.GetComponent<RectTransform>().DOAnchorPosX(-500f, 0.0f, true).SetEase(Ease.InBack).SetUpdate(true);

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DetonateAllBombs()
    {
        StartCoroutine(HideSlidingHUD());
        if (gameController.Detonation())
        {
            detonateButton.GetComponent<Image>().sprite = detonatorDown;
        }

    }

    private IEnumerator HideSlidingHUD()
    {
        yield return new WaitForSeconds(0.5f);
        detonatePanel.GetComponent<RectTransform>().DOAnchorPosX(230f, Random.Range(0.45f, 0.65f), false).SetEase(Ease.InBack).SetUpdate(true);
        resetButton1.GetComponent<RectTransform>().DOAnchorPosX(230f, Random.Range(0.45f, 0.65f), false).SetEase(Ease.InBack).SetUpdate(true);
        mainMenuButton.GetComponent<RectTransform>().DOAnchorPosX(-230f, Random.Range(0.45f, 0.65f), false).SetEase(Ease.InBack).SetUpdate(true);
        bombPanel.GetComponent<RectTransform>().DOAnchorPosX(-500f, Random.Range(0.45f, 0.65f), false).SetEase(Ease.InBack).SetUpdate(true);
    }

    private IEnumerator ShowSlidingHUD()
    {
        yield return new WaitForSeconds(1f);
        resetButton1.GetComponent<RectTransform>().DOAnchorPosX(-20f, Random.Range(0.45f, 0.65f), false).SetEase(Ease.OutBack).SetUpdate(true);
        mainMenuButton.GetComponent<RectTransform>().DOAnchorPosX(20f, Random.Range(0.45f, 0.65f), false).SetEase(Ease.OutBack).SetUpdate(true);
        detonatePanel.GetComponent<RectTransform>().DOAnchorPosX(0f, Random.Range(0.45f, 0.65f), false).SetEase(Ease.OutBack).SetUpdate(true);
        bombPanel.GetComponent<RectTransform>().DOAnchorPosX(0f, Random.Range(0.45f, 0.65f), false).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void ResetLevel()
    {
        //TODO: Show loading screens
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        //Application.LoadLevel();
    }

    public void NextLevel()
    {
        if (gameMaster.currentChapterLevels != null)
        {
            var nextLevel = gameMaster.currentChapterLevels.SkipWhile(x => x.name != SceneManager.GetActiveScene().name).Skip(1).First();
            if (nextLevel == null)
                return;
            //To load video ads
            //AdsController.adsInstance.ShowVideoOrInterstitialAds();
            //TODO: Show loading screens
            SceneManager.LoadSceneAsync(nextLevel.name, LoadSceneMode.Single);
        }
    }

    public void CampaignMap()
    {
        if (menuMusic != null)
            gameMaster.SetMusic(menuMusic);
        else
            gameMaster.SetMusic(null);
        gameMaster.SetMusic(menuMusic);
        //To load video ads
        //AdsController.adsInstance.ShowVideoOrInterstitialAds();
        //TODO: Show loading screens
        SceneManager.LoadSceneAsync("CampaignMap", LoadSceneMode.Single);
        //Application.LoadLevel();
    }

    public void NextRound(int roundNumber, float delay)
    {
        StartCoroutine(ShowSlidingHUD());
        detonateButton.GetComponent<Image>().sprite = detonatorUp;
        levelFinishPanel.SetActive(false);
        ShowRoundText(delay, roundNumber);
    }

    public void UpdateScore(int score)
    {
        TextMeshProUGUI scoreText = scorePanel.GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;

        scoreText.text = score.ToString();

        slider.value = (float)score / (float)gameController.maxScore;


    }
    public void LevelFinished(LevelClear levelClear, int salvage, int score)
    {
        salvageAmount = salvage;

        StartCoroutine(HideSlidingHUD());
        detonatePanel.SetActive(false);
        bombPanel.SetActive(false);
        mainMenuButton.SetActive(false);

        resetButton1.SetActive(false);
        resetButton2.SetActive(true);

        if (gameMaster.currentChapterLevels != null)
        {
            //If last level in chapter, show back to map button instead
            if (gameMaster.currentChapterLevels.Last().name == SceneManager.GetActiveScene().name)
            {
                nextLevelButton.SetActive(false);
                campaignMapButton.SetActive(true);
                if (levelClear == LevelClear.Failed)
                {
                    //nextLevelButton.SetActive(true);
                    campaignMapButton.GetComponent<Button>().interactable = false;
                    campaignMapButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0.2f);
                }
            }
            else
            {
                nextLevelButton.SetActive(true);
                campaignMapButton.SetActive(false);
                if (levelClear == LevelClear.Failed)
                {
                    //nextLevelButton.SetActive(true);
                    nextLevelButton.GetComponent<Button>().interactable = false;
                    nextLevelButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0.2f);
                }
            }
        }


        ShowEndScreen(levelClear, score);

    }

    private void ShowEndScreen(LevelClear levelClear, int levelScore)
    {
        if (endScreen != null)
        {
            var endScreenClone = Instantiate(endScreen);
            var endscreenScript = endScreenClone.GetComponent<EndScreen>();

            salvagePanel.SetActive(true);

            if (levelClear == LevelClear.Failed)
            {
                endscreenScript.Enable(0, levelScore);
                salvagePanel.SetActive(false);
                //salvagePanel.GetComponent<CanvasGroup>().alpha = 0;
                //salvagePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else if (levelClear == LevelClear.OnePentagram)
            {
                endscreenScript.Enable(1, levelScore);
            }
            else if (levelClear == LevelClear.TwoPentagram)
            {
                endscreenScript.Enable(2, levelScore);
            }
            else if (levelClear == LevelClear.ThreePentagram)
            {
                endscreenScript.Enable(3, levelScore);
            }
        }
    }

    public void ShowFinishPanel()
    {
        //var rectTransform = levelFinishPanel.GetComponent<RectTransform>();
        //rectTransform.
        levelFinishPanel.SetActive(true);
        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 1;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (salvagePanel.activeInHierarchy)
        {
            var salvageGain = GameObject.Find("SalvageGain");

            salvageGain.SetActive(true);
            salvageGain.GetComponent<TextMeshProUGUI>().text = salvageAmount.ToString();

        }

    }

    public IEnumerator UpdateBombCount(int bombCount)
    {
        var animator = bombCountText.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("NewValue", true);
        }
        bombCountText.text = bombCount.ToString();
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("NewValue", false);
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
        if (menuMusic != null)
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
            leftOffset += bombCardTransform.sizeDelta.x;
        }

        //Create new bomb card and set sprite, size and positioning
        var card = Instantiate(bombCardPrefab);
        card.transform.SetParent(bombPanel.transform);
        var cardTransform = card.GetComponent<RectTransform>();
        var icon = card.transform.Find("BombCardIcon");
        var cardImage = icon.GetComponent<Image>();
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

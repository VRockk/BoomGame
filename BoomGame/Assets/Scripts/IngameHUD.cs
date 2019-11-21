using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

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
    //public GameObject progressPanel;
    public GameObject progressingPanel;
    //public Slider slider;
    public Image OnePentagram;
    public Image Bar1;
    public Image TwoPentagrams;
    public Image Bar2;
    public Image ThreePentagrams;

    public GameObject endScreen;
    public AudioClip menuMusic;
    private GameMaster gameMaster;
    private CameraHandler cameraHandler;

    private TextMeshProUGUI bombCountText;

    private Canvas canvas;

    private int salvageAmount = 0;

    private bool hudVisible;


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

        detonatePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(230f, detonatePanel.GetComponent<RectTransform>().anchoredPosition.y);
        resetButton1.GetComponent<RectTransform>().anchoredPosition = new Vector2(230f, resetButton1.GetComponent<RectTransform>().anchoredPosition.y);
        mainMenuButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-230f, mainMenuButton.GetComponent<RectTransform>().anchoredPosition.y);
        bombPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500f, bombPanel.GetComponent<RectTransform>().anchoredPosition.y);

        //detonatePanel.GetComponent<RectTransform>().DOAnchorPosX(230f, 0.0f, true).SetEase(Ease.InBack).SetUpdate(true);
        //resetButton1.GetComponent<RectTransform>().DOAnchorPosX(230f, 0.0f, true).SetEase(Ease.InBack).SetUpdate(true);
        //mainMenuButton.GetComponent<RectTransform>().DOAnchorPosX(-230f, 0.0f, true).SetEase(Ease.InBack).SetUpdate(true);
        //bombPanel.GetComponent<RectTransform>().DOAnchorPosX(-500f, 0.0f, true).SetEase(Ease.InBack).SetUpdate(true);

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DetonateAllBombs()
    {
        if (gameController.Detonation())
        {
            HideSlidingHUD(0.5f);
            detonateButton.GetComponent<Image>().sprite = detonatorDown;
        }

    }

    private void HideSlidingHUD(float delay = 0.5f)
    {
        //scorePanel.GetComponent<RectTransform>().DOAnchorPosY(200f, Random.Range(0.45f, 0.65f), false).SetDelay(delay).SetEase(Ease.InBack).SetUpdate(true);
        detonatePanel.GetComponent<RectTransform>().DOAnchorPosX(230f, Random.Range(0.45f, 0.65f), false).SetDelay(delay).SetEase(Ease.InBack).SetUpdate(true);
        resetButton1.GetComponent<RectTransform>().DOAnchorPosX(230f, Random.Range(0.45f, 0.65f), false).SetDelay(delay).SetEase(Ease.InBack).SetUpdate(true);
        mainMenuButton.GetComponent<RectTransform>().DOAnchorPosX(-230f, Random.Range(0.45f, 0.65f), false).SetDelay(delay).SetEase(Ease.InBack).SetUpdate(true);
        bombPanel.GetComponent<RectTransform>().DOAnchorPosX(-500f, Random.Range(0.45f, 0.65f), false).SetDelay(delay).SetEase(Ease.InBack).SetUpdate(true);
    }

    private void ShowSlidingHUD(float delay = 0.5f)
    {
        //scorePanel.GetComponent<RectTransform>().DOAnchorPosY(-60f, Random.Range(0.45f, 0.65f), false).SetDelay(delay).SetEase(Ease.OutBack).SetUpdate(true);
        resetButton1.GetComponent<RectTransform>().DOAnchorPosX(-20f, Random.Range(0.45f, 0.65f), false).SetDelay(delay).SetEase(Ease.OutBack).SetUpdate(true);
        mainMenuButton.GetComponent<RectTransform>().DOAnchorPosX(20f, Random.Range(0.45f, 0.65f), false).SetDelay(delay).SetEase(Ease.OutBack).SetUpdate(true);
        detonatePanel.GetComponent<RectTransform>().DOAnchorPosX(0f, Random.Range(0.45f, 0.65f), false).SetDelay(delay).SetEase(Ease.OutBack).SetUpdate(true);
        bombPanel.GetComponent<RectTransform>().DOAnchorPosX(0f, Random.Range(0.45f, 0.65f), false).SetDelay(delay).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void ResetLevel()
    {
        //TODO: Show loading screens
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        //Application.LoadLevel();
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(gameController.nextLevelName, LoadSceneMode.Single);
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
        detonateButton.GetComponent<Image>().sprite = detonatorUp;
        levelFinishPanel.SetActive(false);

        StartCoroutine(NextRoundDelayed(roundNumber, delay));
    }

    private IEnumerator NextRoundDelayed(int roundNumber, float delay)
    {
        yield return new WaitForSeconds(0.1f);
        ShowSlidingHUD(0.8f);
        ShowRoundText(delay, roundNumber);

    }

    private Sequence scoreTween;

    public void UpdateScore(int score)
    {
        //score = score * 15;
        TextMeshProUGUI scoreText = scorePanel.GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;

        if (scoreText.text != score.ToString())
        {
            if (scoreTween != null)
                scoreTween.Kill();

            //Does a scale up and down animation on the text 
            scoreTween = DOTween.Sequence()
                .Append(scoreText.gameObject.transform.DOScale(1.15f, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true))
                            .Insert(0.2f, scoreText.gameObject.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true));
        }

        scoreText.text = score.ToString();


        float scorePer = (float)score / (float)gameController.maxScore;

        float onePentaLimit = (float)gameController.onePentaScore / 100;
        float twoPentaLimit = (float)gameController.twoPentaScore / 100;
        float threePentaLimit = (float)gameController.threePentaScore / 100;


        TweenerCore<float, float, FloatOptions> tween1 = null;
        TweenerCore<float, float, FloatOptions> tween2 = null;
        TweenerCore<float, float, FloatOptions> tween3 = null;
        TweenerCore<float, float, FloatOptions> tween4 = null;
        TweenerCore<float, float, FloatOptions> tween5 = null;

        if (scorePer > 0f)
        {
            float val = scorePer / onePentaLimit;
            //tween1 = DOTween.To(() => OnePentagram.fillAmount, x => OnePentagram.fillAmount = x, val, 0.5f).SetUpdate(true);
            OnePentagram.fillAmount = val;
        }

        if (scorePer > onePentaLimit)
        {
            float val = (scorePer - onePentaLimit) / (twoPentaLimit - 0.1f - onePentaLimit);
            //if (tween1 != null)
            //    tween1.Kill();
            //OnePentagram.fillAmount = 1;
            //tween2 = DOTween.To(() => Bar1.fillAmount, x => Bar1.fillAmount = x, val, 0.5f).SetUpdate(true);
            Bar1.fillAmount = val;
        }

        if (scorePer > (twoPentaLimit - 0.1f))
        {
            float val = (scorePer - (twoPentaLimit - 0.1f)) / (twoPentaLimit - (twoPentaLimit - 0.1f));
            //if (tween2 != null)
            //    tween2.Kill();
            //Bar1.fillAmount = 1;
            //tween3 = DOTween.To(() => TwoPentagrams.fillAmount, x => TwoPentagrams.fillAmount = x, val, 0.5f).SetUpdate(true);
            TwoPentagrams.fillAmount = val;
        }
        if (scorePer > twoPentaLimit)
        {
            float val = (scorePer - twoPentaLimit) / ((threePentaLimit - 0.1f) - twoPentaLimit);
            //if (tween3 != null)
            //    tween3.Kill();
            //TwoPentagrams.fillAmount = 1;
            //tween4 = DOTween.To(() => Bar2.fillAmount, x => Bar2.fillAmount = x, val, 0.5f).SetUpdate(true);
            Bar2.fillAmount = val;
        }
        if (scorePer > (threePentaLimit - 0.1f))
        {
            float val = (scorePer - (threePentaLimit - 0.1f)) / (threePentaLimit - (threePentaLimit - 0.1f));
            //if (tween4 != null)
            //    tween4.Kill();
            //Bar2.fillAmount = 1;
            //tween5 = DOTween.To(() => ThreePentagrams.fillAmount, x => ThreePentagrams.fillAmount = x, val, 0.5f).SetUpdate(true);
            ThreePentagrams.fillAmount = val;

        }

        //DOTween.To(() => OnePentagram.fillAmount, x => OnePentagram.fillAmount = x, (scorePer) / (0.25f), 0.5f).SetUpdate(true);
        //OnePentagram.fillAmount  = (scorePer) / (0.25f);

        //Bar1.fillAmount = (scorePer - 0.25f) / (0.4f - 0.25f);

        //TwoPentagrams.fillAmount = (scorePer - 0.4f) / (0.5f - 0.4f);

        //Bar2.fillAmount = (scorePer - 0.5f) / (0.65f - 0.5f);

        //ThreePentagrams.fillAmount = (scorePer - 0.65f) / (0.7f - 0.65f);
    }



    public void LevelFinished(LevelClear levelClear, int salvage, int score)
    {
        salvageAmount = salvage;

        HideSlidingHUD(0.5f);
        detonatePanel.SetActive(false);
        bombPanel.SetActive(false);
        mainMenuButton.SetActive(false);

        resetButton1.SetActive(false);
        resetButton2.SetActive(true);

        nextLevelButton.SetActive(true);

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

        var transform = text.gameObject.GetComponent<RectTransform>();

        transform.anchoredPosition = new Vector2(-1500f, 0);
        DOTween.Sequence()
                .Append(transform.DOAnchorPosX(0, 0.4f).SetEase(Ease.OutQuad).SetUpdate(true))
                            .Insert(0.8f, transform.DOAnchorPosX(1500f, 0.4f).SetEase(Ease.InQuad).SetUpdate(true));

        StartCoroutine(ActivateObjectWithDelay(hideDelay, roundPanel, false));
        StartCoroutine(ShowTutorial(hideDelay + 0.3f));
    }

    private IEnumerator ActivateObjectWithDelay(float delay, GameObject panel, bool active)
    {
        yield return new WaitForSeconds(delay);
        panel.SetActive(active);
    }

    private IEnumerator ShowTutorial(float delay)
    {
        yield return new WaitForSeconds(delay);

        var tutorial = GameObject.Find("TutorialUI");

        if (tutorial != null)
        {
            var tutorialScript = tutorial.GetComponent<Tutorial>();
            if (tutorialScript != null)
            {
                tutorialScript.StartTutorial();
            }
        }
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
        float leftOffset = -5f;
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
        //var cardAspectRatioFitter = card.GetComponent<AspectRatioFitter>();

        //cardAspectRatioFitter.aspectRatio = cardImage.sprite.rect.width / cardImage.sprite.rect.height;

        cardTransform.localPosition = new Vector3(leftOffset, -5f, 0);
        cardTransform.localScale = new Vector3(1f, 1f, 1f);

        var bombCardScript = card.GetComponent<BombCard>();
        bombCardScript.bombPrefab = bomb;
    }
}

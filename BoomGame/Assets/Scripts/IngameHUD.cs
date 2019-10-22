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
    public GameObject detonateButton;
    public GameObject resetButton;
    public GameObject nextLevelButton;
    public GameObject salvagePanel;

    public GameObject bombCardPrefab;

    public GameObject endScreen;

    private CameraHandler cameraHandler;

    private TextMeshProUGUI bombCountText;

    private Canvas canvas;

    private float salvageAmount = 0;
    private float bonusSalvage = 0;
    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        cameraHandler = GameObject.FindObjectOfType<CameraHandler>();

        if (gameController == null)
            Debug.LogError("GameController not found in the scene for the IngameHUD");

        if (cameraHandler == null)
            Debug.LogError("CameraHandler not found in the scene for the IngameHUD");


        detonatePanel.GetComponent<CanvasGroup>().alpha = 1;
        detonatePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        roundPanel.GetComponent<CanvasGroup>().alpha = 0;
        roundPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        bombPanel.GetComponent<CanvasGroup>().alpha = 1;
        bombPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 0;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        
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
        detonatePanel.GetComponent<CanvasGroup>().alpha = 1;
        detonatePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        detonateButton.GetComponent<Image>().sprite = detonatorUp;

        bombPanel.GetComponent<CanvasGroup>().alpha = 1;
        bombPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 0;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        ShowRoundText(delay, roundNumber);
    }

    public void LevelFinished(LevelClear levelClear, float salvage, float bonus)
    {
        salvageAmount = salvage;
        bonusSalvage = bonus;
        detonatePanel.GetComponent<CanvasGroup>().alpha = 0;
        detonatePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        bombPanel.GetComponent<CanvasGroup>().alpha = 0;
        bombPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        resetButton.SetActive(true);
        nextLevelButton.SetActive(true);

        if (levelClear == LevelClear.Failed)
        {
            nextLevelButton.SetActive(false);
        }

        ShowEndScreen(levelClear);

    }

    private void ShowEndScreen(LevelClear levelClear)
    {
        if (endScreen != null)
        {
            var endScreenClone = Instantiate(endScreen);
            var endscreenScript = endScreenClone.GetComponent<EndScreen>();

            if (levelClear == LevelClear.Failed)
            {
                endscreenScript.Enable(0);
                nextLevelButton.SetActive(false);
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
        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 1;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
 
    }

    public void UpdateBombCount(int bombCount)
    {
        bombCountText.text = bombCount.ToString();
    }

    private void ShowRoundText(float hideDelay, int roundNumber)
    {
        //var text = roundPanel.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI text = GameObject.Find("RoundText").GetComponent<TextMeshProUGUI>();
        text.text = "Round " + roundNumber;

        roundPanel.GetComponent<CanvasGroup>().alpha = 1;
        //roundPanel.transform.localPosition = new Vector3(0, 0 ,0);

        StartCoroutine(SetCanvasGroupAlpha(hideDelay, roundPanel.GetComponent<CanvasGroup>(), 0));
    }

    private IEnumerator SetCanvasGroupAlpha(float delay, CanvasGroup canvasGroup, float alpha)
    {
        yield return new WaitForSeconds(delay);
        canvasGroup.alpha = alpha;
    }

    public void OpenMainMenu()
    {
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

        cardTransform.localPosition = new Vector3(leftOffset, 5f, 0);


        var bombCardScript = card.GetComponent<BombCard>();
        bombCardScript.bombPrefab = bomb;
    }
}

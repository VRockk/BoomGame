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

    private GameObject detonatePanel;
    private GameObject roundPanel;
    private GameObject failedPanel;
    private GameObject levelFinishPanel;
    private GameObject bombPanel;
    private GameObject penta1Panel;
    private GameObject penta2Panel;
    private GameObject penta3Panel;
    private GameObject bomb1Icon;
    private GameObject detonateButton;
    private CameraHandler cameraHandler;

    private TextMeshProUGUI bombCountText;
    void Awake()
    {
        gameController = GameObject.FindObjectOfType<GameController>();

        if (gameController == null)
            Debug.LogError("GameController not found in the scene for the IngameHUD");

        cameraHandler = GameObject.FindObjectOfType<CameraHandler>();
        if (cameraHandler == null)
            Debug.LogError("CameraHandler not found in the scene for the IngameHUD");

        detonatePanel = GameObject.Find("DetonatePanel");
        roundPanel = GameObject.Find("RoundPanel");
        failedPanel = GameObject.Find("FailedPanel");
        levelFinishPanel = GameObject.Find("LevelFinishPanel");
        bombPanel = GameObject.Find("BombPanel");
        penta1Panel = GameObject.Find("Penta1");
        penta2Panel = GameObject.Find("Penta2");
        penta3Panel = GameObject.Find("Penta3");
        bomb1Icon = GameObject.Find("Bomb1Icon");
        detonateButton = GameObject.Find("DetonateButton"); 

        detonatePanel.GetComponent<CanvasGroup>().alpha = 1;
        detonatePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        roundPanel.GetComponent<CanvasGroup>().alpha = 0;
        roundPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        bombPanel.GetComponent<CanvasGroup>().alpha = 1;
        bombPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        failedPanel.GetComponent<CanvasGroup>().alpha = 0;
        failedPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 0;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        penta1Panel.GetComponent<CanvasGroup>().alpha = 0;
        penta1Panel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        penta2Panel.GetComponent<CanvasGroup>().alpha = 0;
        penta2Panel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        penta3Panel.GetComponent<CanvasGroup>().alpha = 0;
        penta3Panel.GetComponent<CanvasGroup>().blocksRaycasts = false;


        bombCountText = GameObject.Find("Bomb1Count").GetComponent<TextMeshProUGUI>();
        bombCountText.text = gameController.bombCount.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowRoundText(2f, 1);

        Bomb bomb = gameController.bomb.GetComponent<Bomb>();
        bomb1Icon.GetComponent<Image>().sprite = bomb.inventoryIcon;
        //bomb.inventoryIcon
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DetonateAllBombs()
    {
        detonateButton.GetComponent<Image>().sprite = detonatorDown;
        gameController.Detonation();
    }

    public void ResetLevel()
    {
        //TODO: Show loading screens
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        //Application.LoadLevel();
    }

    public void NextLevel()
    {
        //TODO: Show loading screens
        SceneManager.LoadSceneAsync(gameController.nextLevelName, LoadSceneMode.Single);
        //Application.LoadLevel();
    }

    public void NextRound(int roundNumber)
    {
        detonatePanel.GetComponent<CanvasGroup>().alpha = 1;
        detonatePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        detonateButton.GetComponent<Image>().sprite = detonatorUp;

        bombPanel.GetComponent<CanvasGroup>().alpha = 1;
        bombPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        failedPanel.GetComponent<CanvasGroup>().alpha = 0;
        failedPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 0;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        //TODO show "ROUND 2" text popup for a second or something like that

        ShowRoundText(2f, roundNumber);
    }

    public void LevelFinished(LevelClear levelClear)
    {
        detonatePanel.GetComponent<CanvasGroup>().alpha = 0;
        detonatePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        failedPanel.GetComponent<CanvasGroup>().alpha = 0;
        failedPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 1;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        bombPanel.GetComponent<CanvasGroup>().alpha = 0;
        bombPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;


        if (levelClear == LevelClear.OnePentagram)
        {
            penta1Panel.GetComponent<CanvasGroup>().alpha = 1;
            penta1Panel.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            penta2Panel.GetComponent<CanvasGroup>().alpha = 1;
            penta2Panel.GetComponent<Image>().color = new Color32(65, 65, 65, 255);

            penta3Panel.GetComponent<CanvasGroup>().alpha = 1;
            penta3Panel.GetComponent<Image>().color = new Color32(65, 65, 65, 255);
        }
        else if (levelClear == LevelClear.TwoPentagram)
        {
            penta1Panel.GetComponent<CanvasGroup>().alpha = 1;
            penta1Panel.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            penta2Panel.GetComponent<CanvasGroup>().alpha = 1;
            penta2Panel.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            penta3Panel.GetComponent<CanvasGroup>().alpha = 1;
            penta3Panel.GetComponent<Image>().color = new Color32(65, 65, 65, 255);

        }
        else if (levelClear == LevelClear.ThreePentagram)
        {
            penta1Panel.GetComponent<CanvasGroup>().alpha = 1;
            penta1Panel.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            penta2Panel.GetComponent<CanvasGroup>().alpha = 1;
            penta2Panel.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            penta3Panel.GetComponent<CanvasGroup>().alpha = 1;
            penta3Panel.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        }

    }

    public void LevelFailed()
    {
        //TODO show "Level failed" text popup or something like that
        detonatePanel.GetComponent<CanvasGroup>().alpha = 0;
        detonatePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        failedPanel.GetComponent<CanvasGroup>().alpha = 1;
        failedPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 0;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        bombPanel.GetComponent<CanvasGroup>().alpha = 0;
        bombPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;


        //Invoke("ResetLevel", 2f);
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
        roundPanel.transform.localPosition = new Vector3(0, 0 ,0);

        StartCoroutine(SetCanvasGroupAlpha(hideDelay, roundPanel.GetComponent<CanvasGroup>(), 0));
    }

    private IEnumerator SetCanvasGroupAlpha(float delay, CanvasGroup canvasGroup, float alpha)
    {
        //print("sup2");
        yield return new WaitForSeconds(delay);
        canvasGroup.alpha = alpha;
        roundPanel.transform.localPosition = new Vector3(0, -10000f, 0);
    }
}

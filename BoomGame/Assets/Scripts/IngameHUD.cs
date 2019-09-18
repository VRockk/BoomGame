using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameHUD : MonoBehaviour
{
    private GameController gameController;

    private GameObject detonatePanel;
    private GameObject roundPanel;
    private GameObject failedPanel;
    private GameObject levelFinishPanel;
    private GameObject bombPanel;
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


        bombCountText = GameObject.Find("Bomb1Count").GetComponent<TextMeshProUGUI>();
        bombCountText.text = gameController.bombCount.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowRoundText(2f, 1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DetonateAllBombs()
    {
        //No bombs. Dont allow
        if (GameObject.FindObjectsOfType<Bomb>().Length == 0)
            return;

        //if (!allowInput)
        //    return;

        //allowInput = false;
        //gameController.allowInput = false;

        cameraHandler.ZoomToSize(45f, new Vector3(0, 0f, 0));

        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject bombObject in bombs)
        {
            var bomb = bombObject.GetComponent<Bomb>();
            if (bomb != null)
                bomb.Detonate();
        }
        //gameController.WaitForNextRound();
        gameController.Invoke("WaitForNextRound", 2f);
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

        }
        else if (levelClear == LevelClear.TwoPentagram)
        {

        }
        else if (levelClear == LevelClear.ThreePentagram)
        {

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
        //TextMesh

        roundPanel.GetComponent<CanvasGroup>().alpha = 1;

        StartCoroutine(SetCanvasGroupAlpha(hideDelay, roundPanel.GetComponent<CanvasGroup>(), 0));


    }

    private IEnumerator SetCanvasGroupAlpha(float delay, CanvasGroup canvasGroup, float alpha)
    {
        print("sup2");
        yield return new WaitForSeconds(delay);
        canvasGroup.alpha = alpha;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameHUD : MonoBehaviour
{
    private GameController gameController;

    private GameObject detonatePanel;
    private GameObject failedPanel;
    private GameObject levelFinishPanel;
    private GameObject bombPanel;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (gameController == null)
            Debug.LogError("No GameController found in the scene for the IngameHUD");

        detonatePanel = GameObject.Find("DetonatePanel");
        failedPanel = GameObject.Find("FailedPanel");
        levelFinishPanel = GameObject.Find("LevelFinishPanel");
        bombPanel = GameObject.Find("BombPanel");

        detonatePanel.GetComponent<CanvasGroup>().alpha = 1;
        detonatePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        //TODO blocksRaycasts doesnt work on mobile
        bombPanel.GetComponent<CanvasGroup>().alpha = 1;
        bombPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        failedPanel.GetComponent<CanvasGroup>().alpha = 0;
        failedPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 0;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // Start is called before the first frame update
    void Start()
    {
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
}

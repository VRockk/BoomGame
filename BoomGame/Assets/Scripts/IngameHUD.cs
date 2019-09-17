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

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (gameController == null)
            Debug.LogError("No GameController found in the scene for the IngameHUD");

        detonatePanel = GameObject.Find("DetonatePanel");
        failedPanel = GameObject.Find("FailedPanel");
        levelFinishPanel = GameObject.Find("LevelFinishPanel");

        detonatePanel.GetComponent<CanvasGroup>().alpha = 1;
        detonatePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        failedPanel.GetComponent<CanvasGroup>().alpha = 0;
        failedPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 0;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DetonateAllBombs()
    {
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
        var asd = GameObject.Find("FailedPanel");
        if(asd.activeSelf)
        {

        }

        detonatePanel.GetComponent<CanvasGroup>().alpha = 1;
        failedPanel.GetComponent<CanvasGroup>().alpha = 0;
        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 0;

        detonatePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        failedPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        //TODO show "ROUND 2" text popup for a second or something like that
    }

    public void LevelFinished(LevelClear levelClear)
    {
        detonatePanel.GetComponent<CanvasGroup>().alpha = 0;
        failedPanel.GetComponent<CanvasGroup>().alpha = 0;
        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 1;

        detonatePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        failedPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void LevelFailed()
    {
        //TODO show "Level failed" text popup or something like that
        detonatePanel.GetComponent<CanvasGroup>().alpha = 0;
        failedPanel.GetComponent<CanvasGroup>().alpha = 1;
        levelFinishPanel.GetComponent<CanvasGroup>().alpha = 0;

        detonatePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        failedPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        levelFinishPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        //Invoke("ResetLevel", 2f);
    }
}

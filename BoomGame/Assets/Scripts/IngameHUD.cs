using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameHUD : MonoBehaviour
{
    public string nextLevelName;

    private GameController gameController;
    
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if(gameController == null)
            Debug.LogError("No GameController found in IngameHUD");
        //allowInput = true;
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
        SceneManager.LoadSceneAsync("Level_01", LoadSceneMode.Single);
        //Application.LoadLevel();
    }
    
    public void LoadNextLevel()
    {
        //TODO: Show loading screens
        SceneManager.LoadScene(nextLevelName, LoadSceneMode.Single);
    }
}

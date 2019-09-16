using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDLogic : MonoBehaviour
{
    public string nextLevelName;

    private GameController gameController;

    public bool allowInput = true;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
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

        Invoke("WaitForNextRound", 3f);
        //StartCoroutine(WaitForNextRound());
    }

    public void WaitForNextRound()
    {
        allowInput = true;
        gameController.allowInput = true;
        //gameController.NextRound();
    }

    public void ResetLevel()
    {
        //TODO: Show loading screens
        SceneManager.LoadSceneAsync("Level_01", LoadSceneMode.Single);
        allowInput = true;
        //Application.LoadLevel();
    }
    
    public void LoadNextLevel()
    {
        //TODO: Show loading screens
        SceneManager.LoadScene(nextLevelName, LoadSceneMode.Single);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDLogic : MonoBehaviour
{

    public string nextLevelName;
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
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject bombObject in bombs)
        {
            var bomb = bombObject.GetComponent<Bomb>();
            if (bomb != null)
                bomb.Detonate();
        }
    }

    public void ResetLevel()
    {
        //TODO: Show loading screens
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Application.LoadLevel();
    }


    public void LoadNextLevel()
    {
        //TODO: Show loading screens
        SceneManager.LoadScene(nextLevelName, LoadSceneMode.Single);
    }


}

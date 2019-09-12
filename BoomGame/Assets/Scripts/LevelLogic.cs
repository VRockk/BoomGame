using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLogic : MonoBehaviour
{

    public string nextLevel;


    private int shatteringObjectCount;

    // Start is called before the first frame update
    void Start()
    {
        shatteringObjectCount = GameObject.FindGameObjectsWithTag("ShatteringObject").Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel()
    {
        //TODO: Show loading screens
        SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Single);
    }


    
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    
    public void OpenPanel()
    {
        SceneManager.LoadScene("Shop");
    }
     public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

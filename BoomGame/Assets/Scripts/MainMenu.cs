﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public GameObject shopPanel;
    
    public void PlayGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    
    public void OpenPanel()
    {
        if(shopPanel != null)
        { 
            shopPanel.SetActive(true);
        }
    }

}

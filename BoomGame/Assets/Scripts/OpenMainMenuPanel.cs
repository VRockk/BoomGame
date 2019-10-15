using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMainMenuPanel : MonoBehaviour
{
    public GameObject MainMenuPanel;

    public void OpenPanel()
    {
        if(MainMenuPanel != null)
        { 
            MainMenuPanel.SetActive(true);
        }
    }
}

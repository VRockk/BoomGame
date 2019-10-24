using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignMap : MonoBehaviour
{
    public GameController gameController;
    public GameMaster gameMaster;
    public Button[] levelButtons;
    

    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
                levelButtons[i].interactable = false;
        }

        //PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + gameController.salvageValue);
        //if ()
        //{

        //}
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void OpenLevel(string name)
    {
        //TODO show "loading" screen.
        SceneManager.LoadScene(name);
        
    }
}

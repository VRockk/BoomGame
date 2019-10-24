using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CampaignMap : MonoBehaviour
{
    public GameMaster gameMaster;
    public Button[] levelButtons;


    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("LevelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
            {
                levelButtons[i].interactable = false;
                var buttonText = levelButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                    buttonText.color = new Color(1, 1, 1, 0.33f);
            }
        }

        //PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + gameController.salvageValue);
        //if ()
        //{

        //}
    }

    private void Awake()
    {
        //NO dont do this
        //DontDestroyOnLoad(this.gameObject);
    }

    public void OpenLevel(string name)
    {
        //TODO show "loading" screen.
        SceneManager.LoadScene(name);

    }
}

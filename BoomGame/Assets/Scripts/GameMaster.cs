using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public Text salvageText;
    public int currentSalvage;


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("CurrentSalvage"))
        {
            currentSalvage = PlayerPrefs.GetInt("CurrentSalvage");
        } else
        {
            currentSalvage = 0;
            PlayerPrefs.SetInt("CurrentSalvage", 0);
        }

        salvageText.text = "Salvage: " + currentSalvage;
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    public void AddSalvage (int salvageToAdd) {

        currentSalvage += salvageToAdd;
        PlayerPrefs.SetInt("CurrentSalvage", currentSalvage);
        salvageText.text = "Salvage: " + currentSalvage;

    }

}

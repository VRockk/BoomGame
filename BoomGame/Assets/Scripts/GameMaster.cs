using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    
    public static int currentSalvage;


    // Start is called before the first frame update
    void Start()
    {
        GameMaster[] gameMaster = FindObjectsOfType<GameMaster>();
        if (gameMaster.Length == 2)
        {
            Destroy(this.gameObject);
        }


        if (PlayerPrefs.HasKey("CurrentSalvage"))
        {
            currentSalvage = PlayerPrefs.GetInt("CurrentSalvage");
        } else
        {
            currentSalvage = 0;
            PlayerPrefs.SetInt("CurrentSalvage", 0);
        }

        
        
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    public void AddSalvage (int salvageToAdd) {

        currentSalvage += salvageToAdd;
        PlayerPrefs.SetInt("CurrentSalvage", currentSalvage);
        

    }

    void Awake()
    {

        DontDestroyOnLoad(this.gameObject);
        
    }

}

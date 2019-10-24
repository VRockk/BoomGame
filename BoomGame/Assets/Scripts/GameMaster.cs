using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public int currentSalvage;

    private AudioSource audioSource;


    public BombData regularBombData;
    public BombData acidBombData;




    private bool privacyPolicyAccepted;
    public bool PrivacyPolicyAccepted
    {
        get
        {
            return privacyPolicyAccepted;
        }
        set
        {
            if (value)
                PlayerPrefs.SetInt("PrivacyPolicyAccepted", 1);
            else
                PlayerPrefs.SetInt("PrivacyPolicyAccepted", 0);

            privacyPolicyAccepted = value;
        }
    }

    void Awake()
    {
        GetPlayerPrefValues();
        DontDestroyOnLoad(this.gameObject);
    }

    private void GetPlayerPrefValues()
    {
        //PrivacyPolicy
        if (PlayerPrefs.HasKey("PrivacyPolicyAccepted"))
        {
            int privacyAccepted = PlayerPrefs.GetInt("PrivacyPolicyAccepted");
            if (privacyAccepted == 1)
                privacyPolicyAccepted = true;
            else
                privacyPolicyAccepted = false;
        }
        else
        {
            PrivacyPolicyAccepted = false;
        }

        //Salvage
        if (PlayerPrefs.HasKey("CurrentSalvage"))
        {
            currentSalvage = PlayerPrefs.GetInt("CurrentSalvage");
        }
        else
        {
            currentSalvage = 0;
            PlayerPrefs.SetInt("CurrentSalvage", 0);
        }

        //Level Progression



        //Bomb data
        GetBombData();

    }

    private void GetBombData()
    {
        //Regular bomb is always unlocked
        int regularBombUnlocked = PlayerPrefs.GetInt("RegularBombUnlocked", 1);
        int regularBombUpgrade1 = PlayerPrefs.GetInt("RegularBombUpgrade1", 0);
        int regularBombUpgrade2 = PlayerPrefs.GetInt("RegularBombUpgrade2", 0);
        int regularBombUpgrade3 = PlayerPrefs.GetInt("RegularBombUpgrade3", 0);
        int regularBombUpgrade4 = PlayerPrefs.GetInt("RegularBombUpgrade4", 0);

        regularBombData = new BombData(BombType.Regular,
            new int[4] { regularBombUpgrade1, regularBombUpgrade2, regularBombUpgrade3, regularBombUpgrade4 },
            regularBombUnlocked == 1 ? true : false);


        int acidBombUnlocked = PlayerPrefs.GetInt("AcidBombUnlocked", 1);
        int acidBombUpgrade1 = PlayerPrefs.GetInt("RegularBombUpgrade1", 0);
        int acidBombUpgrade2 = PlayerPrefs.GetInt("RegularBombUpgrade2", 0);
        int acidBombUpgrade3 = PlayerPrefs.GetInt("RegularBombUpgrade3", 0);
        int acidBombUpgrade4 = PlayerPrefs.GetInt("RegularBombUpgrade4", 0);

        acidBombData = new BombData(BombType.Acid,
            new int[4] { acidBombUpgrade1, acidBombUpgrade2, acidBombUpgrade3, acidBombUpgrade4 },
            acidBombUnlocked == 1 ? true : false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //If we have a situation where we already have a GameMaster, destroy the new one.
        GameMaster[] gameMaster = FindObjectsOfType<GameMaster>();
        if (gameMaster.Length == 2)
        {
            Destroy(this.gameObject);
        }

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddSalvage(int salvageToAdd)
    {
        currentSalvage += salvageToAdd;
        PlayerPrefs.SetInt("CurrentSalvage", currentSalvage);
    }

    public void SetMusic(AudioClip clip)
    {
        //TODO Fade out/fade in
        audioSource.clip = clip;
    }

    public void PassLevel (int levelNumber)
    {   if (levelNumber > PlayerPrefs.GetInt("LevelReached, 1"))
        {
            PlayerPrefs.SetInt("levelReached", levelNumber);
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public int currentSalvage;

    private AudioSource audioSource;

    

  


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

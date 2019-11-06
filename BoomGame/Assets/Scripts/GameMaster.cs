using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    [HideInInspector]
    public int currentSalvage;
    [HideInInspector]
    public BombData regularBombData;
    [HideInInspector]
    public BombData acidBombData;



    private AudioSource audioSource;

    private AudioClip newMusicClip;
    private float musicFadeInTime = 2f;
    private float musicFadeOutTime = 0.5f;
    private float musicDefaultVolume = 1f;
    private float fadeStarted;
    private bool fadingMusic = false;
    private bool fadingOut = false;

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
        //If we have a situation where we already have a GameMaster, destroy the new one.
        GameMaster[] gameMaster = FindObjectsOfType<GameMaster>();
        if (gameMaster.Length == 2)
        {
            Destroy(this.gameObject);
        }
        else
        {

            //PlayerPrefs.DeleteAll();
            GetPlayerPrefValues();
            DontDestroyOnLoad(this.gameObject);
            //AddSalvage(1500);
        }
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
        int acidBombUpgrade1 = PlayerPrefs.GetInt("AcidBombUpgrade1", 0);
        int acidBombUpgrade2 = PlayerPrefs.GetInt("AcidBombUpgrade2", 0);
        int acidBombUpgrade3 = PlayerPrefs.GetInt("AcidBombUpgrade3", 0);
        int acidBombUpgrade4 = PlayerPrefs.GetInt("AcidBombUpgrade4", 0);

        acidBombData = new BombData(BombType.Acid,
            new int[4] { acidBombUpgrade1, acidBombUpgrade2, acidBombUpgrade3, acidBombUpgrade4 },
            acidBombUnlocked == 1 ? true : false);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fadingMusic)
        {
            float timeSinceStarted = Time.time - fadeStarted;
            if (fadingOut)
            {
                float percentage = timeSinceStarted / musicFadeOutTime;
                float volume = UtilityLibrary.Lerp(musicDefaultVolume, 0f, percentage, LerpMode.EaseIn);
                audioSource.volume = volume;
                //fade out previous music
                if (percentage >= 1f)
                {
                    audioSource.Stop();
                    audioSource.clip = newMusicClip;
                    audioSource.Play();
                    percentage = 0;
                    fadingOut = false;
                    fadeStarted = Time.time;
                }
            }
            else
            {
                float percentage = timeSinceStarted / musicFadeInTime;
                //fade in new music
                float volume = UtilityLibrary.Lerp(0f, musicDefaultVolume, percentage, LerpMode.EaseIn);
                audioSource.volume = volume;
                if (percentage >= 1f)
                {
                    percentage = 0;
                    fadingOut = false;
                    fadeStarted = Time.time;
                    fadingMusic = false;
                }

            }
        }
    }

    public void AddSalvage(int salvageToAdd)
    {
        currentSalvage += salvageToAdd;
        PlayerPrefs.SetInt("CurrentSalvage", currentSalvage);
    }

    public void SetMusic(AudioClip clip)
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && clip != null)
        {
            //Dont set the same clip again
            if (audioSource.clip != null && audioSource.clip.name == clip.name)
                return;

            musicDefaultVolume = audioSource.volume;
            newMusicClip = clip;
            fadeStarted = Time.time;
            fadingMusic = true;

            //Only fade out if we have music already playing
            if (audioSource.isPlaying)
                fadingOut = true;
            else
            {
                audioSource.clip = newMusicClip;
                audioSource.volume = 0f;
                audioSource.Play();
                fadingOut = false;
            }
        }
    }

    public void SetBombUpgradeLevel(BombType bombtype, int upgradePosition, int level)
    {
        string levelKeyName = "";
        if (bombtype == BombType.Regular)
        {
            levelKeyName = "RegularBombUpgrade";
        }
        else if (bombtype == BombType.Acid)
        {
            levelKeyName = "AcidBombUpgrade";
        }

        if (upgradePosition == 1)
        {
            levelKeyName += "1";
        }
        else if (upgradePosition == 2)
        {
            levelKeyName += "2";
        }
        else if (upgradePosition == 3)
        {
            levelKeyName += "3";
        }
        else if (upgradePosition == 4)
        {
            levelKeyName += "4";
        }
        PlayerPrefs.SetInt(levelKeyName, level);
        GetBombData();
    }

}

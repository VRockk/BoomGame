using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    [HideInInspector]
    public int currentSalvage;
    [HideInInspector]
    public List<Level> currentChapterLevels;


    private AudioSource audioSource;

    private AudioClip newMusicClip;
    private float musicFadeInTime = 2f;
    private float musicFadeOutTime = 0.5f;
    private float musicDefaultVolume = 1f;
    private float fadeStarted;
    private bool fadingMusic = false;
    private bool fadingOut = false;

    [HideInInspector]
    public List<BombData> bombData;

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

    private bool signIn;
    [HideInInspector]
    public bool loginShowed = false;

    public bool SignIn
    {
        get
        {
            return signIn;
        }
        set
        {
            if (value)
                PlayerPrefs.SetInt("SignIn", 1);
            else
                PlayerPrefs.SetInt("SignIn", 0);
            signIn = value;
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

#if UNITY_EDITOR
            PlayerPrefs.DeleteAll();
#endif
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

        //currentSalvage = 100000;
        //Bomb data
        GetBombData();

        //Sign In
        if (PlayerPrefs.HasKey("SignIn"))
        {
            int signed = PlayerPrefs.GetInt("SignIn");
            if (signed == 1)
                signIn = true;
            else
                signIn = false;
        }
        else
        {
            signIn = false;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //TODO use DOTween for music fading
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

    private void GetBombData()
    {
        bombData = new List<BombData>();

        //initialize all bomb types with saved level data
        bombData.Add(new BombData(BombType.Regular, PlayerPrefs.GetInt(BombType.Regular.ToString() + "Level", 1), false, "Normal Bomb", "normal"));
        bombData.Add(new BombData(BombType.Acid, PlayerPrefs.GetInt(BombType.Acid.ToString() + "Level", 1), false, "Acid Bomb", "acid"));
        bombData.Add(new BombData(BombType.Screamer, PlayerPrefs.GetInt(BombType.Screamer.ToString() + "Level", 1), true, "Screamer Bomb", "screamer"));
        bombData.Add(new BombData(BombType.HolyWater, PlayerPrefs.GetInt(BombType.HolyWater.ToString() + "Level", 1), true, "Holy Water Bomb", "holy"));
        bombData.Add(new BombData(BombType.Fire, PlayerPrefs.GetInt(BombType.Fire.ToString() + "Level", 1), true, "Fire Bomb", "fire"));
        bombData.Add(new BombData(BombType.Mega, PlayerPrefs.GetInt(BombType.Mega.ToString() + "Level", 1), true, "Mega Bomb", "mega"));
        bombData.Add(new BombData(BombType.Void, PlayerPrefs.GetInt(BombType.Void.ToString() + "Level", 1), true, "Void Bomb", "void"));
    }

}

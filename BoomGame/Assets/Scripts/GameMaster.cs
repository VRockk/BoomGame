using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{

    public int currentSalvage;

    private AudioSource audioSource;
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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

        if (PlayerPrefs.HasKey("CurrentSalvage"))
        {
            currentSalvage = PlayerPrefs.GetInt("CurrentSalvage");
        }
        else
        {
            currentSalvage = 0;
            PlayerPrefs.SetInt("CurrentSalvage", 0);
        }



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

}

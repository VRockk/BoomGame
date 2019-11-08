using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Chapter : MonoBehaviour
{
    public string chapterName = "";

    //How many pentagrams needs to be gained to open chapter
    public int pentaLimit = 0;

    public List<GameObject> levels;

    public Sprite clearedIcon;
    public Sprite unclearedIcon;
    public Sprite selectedIcon;
    public GameObject lockInfo;
    public GameObject pentaRequirement;

    [HideInInspector]
    public bool locked = true;

    public SpriteRenderer iconRenderer;
    private bool allLevelsCleared = true;
    private int playerPentagrams;

    // Start is called before the first frame update
    void Start()
    {
        //Get saved values for each level
        foreach (var level in levels)
        {
            var levelScript = level.GetComponent<Level>();
            if (levelScript != null)
            {
                levelScript.SetSavedValues();

                //If any level has zero pentagrams it means the chapter is not fully cleared
                if (levelScript.pentagrams == 0)
                    allLevelsCleared = false;
            }
        }
        playerPentagrams = PlayerPrefs.GetInt("PlayerPentagrams", 0);

        //Player needs to have more or equal amount of pentagrams to have this chapter open
        if (playerPentagrams >= pentaLimit)
        {
            locked = false;
        }
        else
        {
            locked = true;
        }
        SetStatus();
    }

    private void SetStatus()
    {
        if (iconRenderer != null)
        {
            if (allLevelsCleared)
                iconRenderer.sprite = clearedIcon;
            else
                iconRenderer.sprite = unclearedIcon;
        }

        if (locked)
        {
            if (lockInfo != null)
            {
                lockInfo.SetActive(true);
                if (pentaRequirement != null)
                {
                    var lockText = pentaRequirement.GetComponent<TextMeshPro>();
                    lockText.text = (pentaLimit - playerPentagrams).ToString();
                }
            }
        }
        else
            lockInfo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetSelected(bool selected)
    {
        SetStatus();
    }
}

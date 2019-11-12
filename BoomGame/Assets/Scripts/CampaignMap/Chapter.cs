using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Chapter : MonoBehaviour
{
    public string chapterName = "";

    //How many pentagrams needs to be gained to open chapter
    public int pentaLimit = 0;

    public List<string> levelNames;

    public Sprite clearedIcon;
    public Sprite unclearedIcon;
    public Sprite selectedIcon;
    public GameObject lockInfo;
    public GameObject pentaRequirement;
    public GameObject previousChapter;
    public SpriteRenderer iconRenderer;

    [HideInInspector]
    public bool locked = false;

    [HideInInspector]
    public List<Level> chapterLevels;

    private bool allLevelsCleared = true;
    private int playerPentagrams;

    private void Awake()
    {
        chapterLevels = new List<Level>();
        //Get saved values for each level
        foreach (var levelName in levelNames)
        {
            Level chapterLevel = new Level(levelName);
            //If any level has zero pentagrams it means the chapter is not fully cleared
            if (chapterLevel.pentagrams == 0)
                allLevelsCleared = false;

            chapterLevels.Add(chapterLevel);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        locked = false;
        playerPentagrams = PlayerPrefs.GetInt("PlayerPentagrams", 0);

        //All levels in previous chapter needs to be cleared for this to be open
        if (previousChapter != null)
        {
            var prevChapterScript = previousChapter.GetComponent<Chapter>();
            foreach (var level in prevChapterScript.chapterLevels)
            {
                if (level.pentagrams == 0)
                {
                    locked = true;
                    break;
                }
            }
        }

        //Player needs to have more or equal amount of pentagrams to have this chapter open
        if (playerPentagrams < pentaLimit)
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

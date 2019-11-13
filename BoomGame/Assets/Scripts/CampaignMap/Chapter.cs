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

    public SpriteRenderer chapterIconRenderer;
    public GameObject requirement;
    public GameObject requirementText;
    public GameObject previousChapter;
    public GameObject selectedIcon;

    [HideInInspector]
    public bool enoughPentagrams = false;

    [HideInInspector]
    public List<Level> chapterLevels;

    [HideInInspector]
    public bool previousLevelsCleared = true;
    private int playerPentagrams;

    private void Awake()
    {
        chapterLevels = new List<Level>();

        //Create Level objects
        foreach (var levelName in levelNames)
        {
            Level chapterLevel = new Level(levelName);
            chapterLevels.Add(chapterLevel);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enoughPentagrams = false;
        playerPentagrams = PlayerPrefs.GetInt("PlayerPentagrams", 0);

        //All levels in previous chapter needs to be cleared for this to be open
        if (previousChapter != null)
        {
            var prevChapterScript = previousChapter.GetComponent<Chapter>();
            foreach (var level in prevChapterScript.chapterLevels)
            {
                if (level.pentagrams == 0)
                {
                    previousLevelsCleared = false;
                    break;
                }
            }
        }


        //Player needs to have more or equal amount of pentagrams to have this chapter open
        enoughPentagrams = playerPentagrams >= pentaLimit;


        SetStatus();
    }

    private void SetStatus()
    {
        if (chapterIconRenderer != null)
        {
            if (previousLevelsCleared && enoughPentagrams)
            {
                chapterIconRenderer.color = new Color(1, 1, 1, 1);
            }
            else
            {
                chapterIconRenderer.color = new Color(0.33f, 0.33f, 0.33f, 1);
            }

            if (!enoughPentagrams && requirement != null)
            {
                int neededPentas = pentaLimit - playerPentagrams;
                if (neededPentas > 0)
                {
                    requirement.SetActive(true);
                    if (requirementText != null)
                    {
                        var lockText = requirementText.GetComponent<TextMeshPro>();
                        lockText.text = (pentaLimit - playerPentagrams).ToString();
                    }
                }
            }
            else
                requirement.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetSelected(bool selected)
    {
        //if (selectedIcon != null)
        //    selectedIcon.SetActive(selected);
    }
}

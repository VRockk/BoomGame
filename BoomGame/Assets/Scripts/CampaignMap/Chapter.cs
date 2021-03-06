﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;


public class Chapter : MonoBehaviour
{
    public string chapterName = "";

    //How many pentagrams needs to be gained to open chapter
    public int pentaLimit = 0;

    public List<string> levelNames;

    public List<GameObject> levelIcons;

    public GameObject chapterIcon;
    public GameObject requirement;
    public GameObject requirementText;
    public GameObject previousChapter;
    public GameObject selectedIcon;

    public Sprite levelBronzeSprite;
    public Sprite levelSilverSprite;
    public Sprite levelGoldSprite;

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
        for (int i = 0; i < levelNames.Count; i++)
        {
            Level chapterLevel = new Level(levelNames[i]);
            chapterLevels.Add(chapterLevel);
            if (levelIcons.Count - 1 >= i)
            {
                var levelIcon = levelIcons[i];
                if (levelIcon != null)
                {
                    var spriteRenderer = levelIcon.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        // Set the map level icons depending on how many pentagrams that level has
                        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                        if (chapterLevel.pentagrams == 0)
                        {
                            spriteRenderer.color = new Color(0.33f, 0.33f, 0.33f, 1f);
                        }
                        if (chapterLevel.pentagrams == 1)
                        {
                            spriteRenderer.sprite = levelBronzeSprite;
                        }
                        if (chapterLevel.pentagrams == 2)
                        {
                            spriteRenderer.sprite = levelSilverSprite;
                        }
                        if (chapterLevel.pentagrams == 3)
                        {
                            spriteRenderer.sprite = levelGoldSprite;
                        }
                    }
                }
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        enoughPentagrams = false;
        playerPentagrams = PlayerPrefs.GetInt("PlayerPentagrams", 0);

        //All levels in previous chapter needs to be cleared for this chapter to be open available
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

        //AND player needs to have more or equal amount of pentagrams to have this chapter open
        enoughPentagrams = playerPentagrams >= pentaLimit;

        if (!enoughPentagrams && !previousLevelsCleared)
        {
            if (levelIcons != null)
            {
                //hide the level icons if this chapter is not open yet
                foreach (var icon in levelIcons)
                {
                    icon.SetActive(false);
                }
            }
        }
        SetStatus();
    }

    private void SetStatus()
    {
        if (chapterIcon != null)
        {
            var spriteRenderer = chapterIcon.GetComponent<SpriteRenderer>();
            if (previousLevelsCleared && enoughPentagrams)
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);
            }
            else
            {
                spriteRenderer.color = new Color(0.33f, 0.33f, 0.33f, 1);
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


    Sequence selectedScaleSeq = null;
    public void SetSelected(bool selected)
    {
        if (chapterIcon != null)
        {
            chapterIcon.transform.localScale = new Vector3(1, 1, 1f);
            if (selected)
            {
                selectedScaleSeq = DOTween.Sequence().Append(chapterIcon.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.3f)).SetLoops(-1, LoopType.Yoyo);
                //chapterIcon.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
                //chapterIcon.transform.DOScale(new Vector3(1.35f, 1.35f, 1f), 0.3f).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                if (selectedScaleSeq != null)
                    selectedScaleSeq.Kill();
                //chapterIcon.transform.localScale = new Vector3(1f, 1f, 1f);
                chapterIcon.transform.DOScale(new Vector3(1, 1, 1), 0.3f);
            }
        }
    }
}

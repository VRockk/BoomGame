using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public Sprite lockedIcon;

    [HideInInspector]
    public bool locked = true;

    public SpriteRenderer iconRenderer;
    private bool allLevelsCleared = true;

    // Start is called before the first frame update
    void Start()
    {

        //Get saved values for each level
        foreach (var level in levels)
        {
            var levelScript = level.GetComponent<Level>();
            if (levelScript != null)
            {
                levelScript.GetSavedValues();

                //If any level has zero pentagrams it means the chapter is not fully cleared
                if (levelScript.pentagrams == 0)
                    allLevelsCleared = false;
            }
        }


        var playerPentagrams = PlayerPrefs.GetInt("PlayerPentagrams", 0);

        //Player needs to have more or equal amount of pentagrams to have this chapter open
        if (playerPentagrams >= pentaLimit)
        {
            locked = false;
        }
        else
        {
            locked = true;
        }

        if (iconRenderer != null)
        {
            if (locked)
                iconRenderer.sprite = lockedIcon;
            else
            {
                if (allLevelsCleared)
                    iconRenderer.sprite = clearedIcon;
                else
                    iconRenderer.sprite = unclearedIcon;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetSelected(bool selected)
    {
        iconRenderer = GetComponent<SpriteRenderer>();
        if (iconRenderer != null)
        {
            if (locked)
                iconRenderer.sprite = lockedIcon;
            else
            {
                if (selected)
                {
                    iconRenderer.sprite = selectedIcon;
                }
                else
                {
                    if (allLevelsCleared)
                        iconRenderer.sprite = clearedIcon;
                    else
                        iconRenderer.sprite = unclearedIcon;
                }
            }
        }
    }
}

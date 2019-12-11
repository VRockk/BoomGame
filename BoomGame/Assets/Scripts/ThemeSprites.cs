using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ThemeSprites : MonoBehaviour
{

    public Sprite fireSprite;
    public Sprite iceSprite;

    [HideInInspector]
    public bool skipStart = false;

    // Start is called before the first frame update
    void Start()
    {
        if (skipStart)
            return;
        Initialize();
    }

    public void Initialize()
    {
        var gameController = GameObject.FindObjectOfType<GameController>();

        //var theme = GameObject.
        //print(gameController.levelTheme);

        if (gameController == null)
            print("GameController not found : " + gameObject.name);

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            if (gameController.levelTheme == LevelTheme.Fire && fireSprite != null)
            {
                spriteRenderer.sprite = fireSprite;
            }
            else if (gameController.levelTheme == LevelTheme.Ice && iceSprite != null)
            {
                spriteRenderer.sprite = iceSprite;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

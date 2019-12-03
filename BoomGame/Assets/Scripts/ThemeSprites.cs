using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ThemeSprites : MonoBehaviour
{

    public Sprite fireSprite;
    public Sprite iceSprite;
    // Start is called before the first frame update
    void Start()
    {
        var gameController = GameObject.FindObjectOfType<GameController>();
        //var theme = GameObject.
        //print(gameController.levelTheme);
        if (gameController == null)
            print("GameController not found : " + gameObject.name);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            if (gameController.levelTheme == LevelTheme.Fire)
            {

            }
            else if (gameController.levelTheme == LevelTheme.Ice)
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

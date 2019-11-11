using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoPanel : MonoBehaviour
{
    public GameObject penta1Icon;
    public GameObject penta2Icon;
    public GameObject penta3Icon;
    public GameObject score;

    public GameObject number;

    private string levelName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLevelInfo(Level level, int count)
    {
        var penta1 = penta1Icon.GetComponent<Image>();
        var penta2 = penta2Icon.GetComponent<Image>();
        var penta3 = penta3Icon.GetComponent<Image>();
        var scoreText = score.GetComponent<TextMeshProUGUI>();
        var numberText = number.GetComponent<TextMeshProUGUI>();

        levelName = level.name;
        //set pentagram colors depending on the clear level
        if (level.pentagrams == 0)
        {
            penta1.color = new Color(0.1f, 0.1f, 0.1f, 1);
            penta2.color = new Color(0.1f, 0.1f, 0.1f, 1);
            penta3.color = new Color(0.1f, 0.1f, 0.1f, 1);
        }
        else if (level.pentagrams == 1)
        {
            penta1.color = new Color(1f, 1f, 1f, 1);
            penta2.color = new Color(0.1f, 0.1f, 0.1f, 1);
            penta3.color = new Color(0.1f, 0.1f, 0.1f, 1);
        }
        else if (level.pentagrams == 2)
        {
            penta1.color = new Color(1f, 1f, 1f, 1);
            penta2.color = new Color(1f, 1f, 1f, 1);
            penta3.color = new Color(0.1f, 0.1f, 0.1f, 1);
        }
        else if (level.pentagrams == 3)
        {
            penta1.color = new Color(1f, 1f, 1f, 1);
            penta2.color = new Color(1f, 1f, 1f, 1);
            penta3.color = new Color(1f, 1f, 1f, 1);
        }

        scoreText.text = level.score.ToString();
        numberText.text = count.ToString();
    }


    public void OpenLevel()
    {
        //TODO show "loading" screen.
        var chapter = this.transform.parent.parent.parent.gameObject.GetComponent<ChapterPanel>();
        chapter.OpenLevel(levelName);

    }
}

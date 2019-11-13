using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public string name;
    public int pentagrams = 0;
    public int savedBombs = 0;
    public int score = 0;

    public Level(string name)
    {
        this.name = name;
        pentagrams = PlayerPrefs.GetInt(name + "Pentagrams", 0);
        savedBombs = PlayerPrefs.GetInt(name + "SavedBombs", 0);
        score = PlayerPrefs.GetInt(name + "Score", 0);
    }
}

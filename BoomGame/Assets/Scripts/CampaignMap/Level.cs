using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Object scene;

    [HideInInspector]
    public int pentagrams = 0;
    [HideInInspector]
    public int savedBombs = 0;
    [HideInInspector]
    public int score = 0;

    public void SetSavedValues()
    {
        pentagrams = PlayerPrefs.GetInt(scene.name + "Pentagrams", 0);
        savedBombs = PlayerPrefs.GetInt(scene.name + "SavedBombs", 0);
        score = PlayerPrefs.GetInt(scene.name + "Score", 0);
    }
}

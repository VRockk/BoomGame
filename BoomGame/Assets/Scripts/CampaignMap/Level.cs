using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Level : MonoBehaviour
{
    public SceneAsset scene;

    [HideInInspector]
    public int pentagrams = 0;
    [HideInInspector]
    public int savedBombs = 0;
    [HideInInspector]
    public int score = 0;

    //public string[] maps;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetSavedValues()
    {
        if (scene == null)
            Debug.LogWarning("No scene file set for level object: " + gameObject.name);
        else
        {
            var name = scene.name;
            pentagrams = PlayerPrefs.GetInt(name + "Pentagrams", 0);
            savedBombs = PlayerPrefs.GetInt(name + "SavedBombs", 0);
            score = PlayerPrefs.GetInt(name + "Score", 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Randomizes the pitch and volume of AudioSource found from this object
/// </summary>
public class AudioRandomizer : MonoBehaviour
{
    void Awake()
    {

        var audio = GetComponent<AudioSource>();

        if(audio != null)
        {
            audio.pitch = Random.Range(0.95f, 1.05f);
            audio.volume = Random.Range(0.95f, 1.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

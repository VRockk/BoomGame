using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Randomizes the pitch and volume of AudioSource found from this object
/// </summary>
public class AudioRandomizer : MonoBehaviour
{
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;
    public float minVolume = 0.95f;
    public float maxVolume = 1.05f;
    void Awake()
    {

        var audio = GetComponent<AudioSource>();

        if( audio != null)
        {
            audio.pitch = Random.Range(minPitch, maxPitch);
            audio.volume = Random.Range(minVolume, maxVolume);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

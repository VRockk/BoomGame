using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
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

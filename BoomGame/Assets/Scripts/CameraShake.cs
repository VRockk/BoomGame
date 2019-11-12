using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void Shake(float duration, float strength, int vibrato, float randomness, bool fadeOut)
    {
        Camera.main.DOShakePosition(duration, strength, vibrato, randomness, fadeOut);
    }
}

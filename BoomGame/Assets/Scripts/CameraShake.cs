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

        StartCoroutine(SetCamPos(duration + 0.05f));
    }

    private IEnumerator SetCamPos(float delay)
    {
        yield return new WaitForSeconds(delay);
        var cameraHandler = FindObjectOfType<CameraHandler>();
        Camera.main.transform.localPosition = cameraHandler.cameraDefaultPos;

    }
}

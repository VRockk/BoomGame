using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollParallaxing : MonoBehaviour
{
    public Camera camera;
    private float referenceSize = 45;

    public float xParallax = 0.2f;
    public float yParallax = 0.2f;
    public float sizeParallax = 0.2f;

    private Vector3 defaultScale;
    private Vector3 defaultPosition;
    private void Start()
    {
        if (camera == null)
            camera = Camera.main;
        referenceSize = camera.orthographicSize;
        defaultScale = transform.localScale;
        defaultPosition = transform.localPosition;

    }
    void LateUpdate()
    {
        float sizeRatio = (camera.orthographicSize / referenceSize);
        float newSize = (sizeRatio - 1) * sizeParallax + 1;

        Vector3 tempScale = transform.localScale;
        tempScale.x = defaultScale.x * newSize;
        tempScale.y = defaultScale.y * newSize;
        transform.localScale = tempScale;

        Vector3 tempPos = transform.localPosition;

        tempPos.x = (camera.transform.position.x * (1 - newSize + newSize * xParallax)) + (defaultPosition.x * xParallax);
        tempPos.y = (camera.transform.position.y * (1 - newSize + newSize * yParallax)) + (defaultPosition.y * xParallax);

        transform.localPosition = tempPos;
    }
}

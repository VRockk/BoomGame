﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollParallaxing : MonoBehaviour
{
    public Camera mainCamera;
    private float referenceSize = 45;

    public float xParallax = 0.2f;
    public float yParallax = 0.2f;
    public float sizeParallax = 0.2f;

    private Vector3 defaultScale;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        referenceSize = mainCamera.orthographicSize;
        defaultScale = transform.localScale;

    }
    void LateUpdate()
    {

        float sizeRatio = (mainCamera.orthographicSize / referenceSize);
        float newSize = (sizeRatio - 1) * sizeParallax + 1;

        Vector3 tempScale = transform.localScale;
        tempScale.x = defaultScale.x * newSize;
        tempScale.y = defaultScale.y * newSize;
        transform.localScale = tempScale;

        Vector3 tempPos = transform.localPosition;
        var xRatio = (1 - newSize + newSize * xParallax);
        var yRatio = (1 - newSize + newSize * yParallax);
        tempPos.x = (mainCamera.transform.position.x * xRatio);
        tempPos.y = (mainCamera.transform.position.y * yRatio);

        transform.localPosition = tempPos;
    }
}

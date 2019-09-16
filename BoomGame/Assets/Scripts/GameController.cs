﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int maxRounds = 2;
    public GameObject bomb;
    public bool allowInput = true;

    private int roundCounter = 1;
    private int shatteringObjectCount;


    // Start is called before the first frame update
    void Start()
    {
        roundCounter = 1;
        shatteringObjectCount = GameObject.FindGameObjectsWithTag("ShatteringObject").Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
                Vector3 worldPos;
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    worldPos = hit.point;
                }
                else
                {
                    worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                }
                worldPos.z = 0f;
                Instantiate(bomb, worldPos, Quaternion.identity);
            }
        }
    }



    public void NextRound()
    {
        roundCounter++;

        if(roundCounter > maxRounds)
        {
            // Show Next level/Reset buttons
        }
    }


    
}

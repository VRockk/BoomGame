using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentPentagrams : MonoBehaviour
{
    public GameObject pentaText;
    // Start is called before the first frame update
    void Start()
    {
        if (pentaText != null)
        {
            var pentaTextComp = pentaText.GetComponent<TextMeshProUGUI>();
            var playerPentagrams = PlayerPrefs.GetInt("PlayerPentagrams", 0);
            pentaTextComp.text = playerPentagrams.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

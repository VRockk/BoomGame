using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Score : MonoBehaviour
{
    public static int scoreValue = 0;
    public TextMeshProUGUI ScoreText;

    // Start is called before the first frame update
    void Start()
    {
       
        
    } 

    // Update is called once per frame
    void Update()
    {
        ScoreText.GetComponent<TextMeshProUGUI>().text = "Score " + scoreValue;
    }
}

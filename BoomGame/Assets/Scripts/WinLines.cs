using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLines : MonoBehaviour
{

    public float onePentaLine = 0;
    public float twoPentaLine = -10f;
    public float threePentaLine = -20f;

    private GameObject redline;
    private GameObject blueline;
    private GameObject greenline;

    // Start is called before the first frame update
    void Start()
    {
        redline = GameObject.Find("Redline");
        blueline = GameObject.Find("Blueline");
        greenline = GameObject.Find("Greenline");

        if (redline == null)
        {
            Debug.LogError("No redline found");
        }
        if (blueline == null)
        {
            Debug.LogError("No blueline found");
        }
        if (greenline == null)
        {
            Debug.LogError("No greenline found");
        }

        SetLinePositions();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {

        redline = GameObject.Find("Redline");
        blueline = GameObject.Find("Blueline");
        greenline = GameObject.Find("Greenline");

        if(redline != null && blueline != null && greenline != null)
            SetLinePositions();

    }

    private void SetLinePositions()
    {
        var redlineRenderer = redline.GetComponent<LineRenderer>();
        var bluelineRenderer = blueline.GetComponent<LineRenderer>();
        var greenlineRenderer = greenline.GetComponent<LineRenderer>();

        redlineRenderer.SetPosition(0, new Vector3(-200, onePentaLine, 0));
        redlineRenderer.SetPosition(1, new Vector3(200, onePentaLine, 0));

        bluelineRenderer.SetPosition(0, new Vector3(-200, twoPentaLine, 0));
        bluelineRenderer.SetPosition(1, new Vector3(200, twoPentaLine, 0));

        greenlineRenderer.SetPosition(0, new Vector3(-200, threePentaLine, 0));
        greenlineRenderer.SetPosition(1, new Vector3(200, threePentaLine, 0));
    }
}

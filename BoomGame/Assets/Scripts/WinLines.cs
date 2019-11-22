using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLines : MonoBehaviour
{

    public float onePentaHeight = 0;
    public float twoPentaHeight = -10f;
    public float threePentaHeight = -20f;

    public GameObject oneScoreLineObject;
    public GameObject twoScoreLineObject;
    public GameObject threeScoreLineObject;

    private Scoreline oneScoreLine;
    private Scoreline twoScoreLine;
    private Scoreline threeScoreLine;
    // Start is called before the first frame update
    void Start()
    {
        //redline = GameObject.Find("Redline");
        //blueline = GameObject.Find("Blueline");
        //greenline = GameObject.Find("Greenline");

        if (oneScoreLineObject == null)
        {
            Debug.LogError("No redline found");
        }
        if (twoScoreLineObject == null)
        {
            Debug.LogError("No blueline found");
        }
        if (threeScoreLineObject == null)
        {
            Debug.LogError("No greenline found");
        }

        if (oneScoreLineObject != null && twoScoreLineObject != null && threeScoreLineObject != null)
        {
            SetLinePositions();
            oneScoreLine = oneScoreLineObject.GetComponent<Scoreline>();
            twoScoreLine = twoScoreLineObject.GetComponent<Scoreline>();
            threeScoreLine = threeScoreLineObject.GetComponent<Scoreline>();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnValidate()
    {

        //redline = GameObject.Find("Redline");
        //blueline = GameObject.Find("Blueline");
        //greenline = GameObject.Find("Greenline");

        if (oneScoreLineObject != null && twoScoreLineObject != null && threeScoreLineObject != null)
            SetLinePositions();

    }

    private void SetLinePositions()
    {
        oneScoreLineObject.transform.position = new Vector3(0, onePentaHeight, 0);
        twoScoreLineObject.transform.position = new Vector3(0, twoPentaHeight, 0);
        threeScoreLineObject.transform.position = new Vector3(0, threePentaHeight, 0);
        //var redlineRenderer = redline.GetComponent<LineRenderer>();
        //var bluelineRenderer = blueline.GetComponent<LineRenderer>();
        //var greenlineRenderer = greenline.GetComponent<LineRenderer>();

        //redlineRenderer.SetPosition(0, new Vector3(-200, onePentaLine, 0));
        //redlineRenderer.SetPosition(1, new Vector3(200, onePentaLine, 0));

        //bluelineRenderer.SetPosition(0, new Vector3(-200, twoPentaLine, 0));
        //bluelineRenderer.SetPosition(1, new Vector3(200, twoPentaLine, 0));

        //greenlineRenderer.SetPosition(0, new Vector3(-200, threePentaLine, 0));
        //greenlineRenderer.SetPosition(1, new Vector3(200, threePentaLine, 0));
    }

    public void UpdateLines(LevelClear levelClear)
    {
        if (levelClear == LevelClear.NotCleared || levelClear == LevelClear.Failed)
        {
            oneScoreLine.SetLineHot(false);
            twoScoreLine.SetLineHot(false);
            threeScoreLine.SetLineHot(false);
        }
        else if (levelClear == LevelClear.OnePentagram)
        {
            oneScoreLine.SetLineHot(true);
            twoScoreLine.SetLineHot(false);
            threeScoreLine.SetLineHot(false);
        }
        else if (levelClear == LevelClear.TwoPentagram)
        {
            oneScoreLine.SetLineHot(true);
            twoScoreLine.SetLineHot(true);
            threeScoreLine.SetLineHot(false);
        }
        else if (levelClear == LevelClear.ThreePentagram)
        {
            oneScoreLine.SetLineHot(true);
            twoScoreLine.SetLineHot(true);
            threeScoreLine.SetLineHot(true);
        }
    }
}

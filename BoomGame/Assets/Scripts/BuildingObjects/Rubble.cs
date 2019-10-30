using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubble : MonoBehaviour
{
    /// <summary>
    /// The time taken to move from the start to finish positions
    /// </summary>
    private float moveTime = 0.5f;


    //Whether we are currently interpolating or not
    private bool isLerping = false;

    //The start and finish positions for the interpolation
    private Vector3 startPosition;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 endPosition;

    private Vector3 startScale;
    private Vector3 endScale;

    //The Time.time value when we started the interpolation
    private float timeStartedLerping;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartMoving()
    {
        sprite = GetComponent<SpriteRenderer>();
        isLerping = true;
        timeStartedLerping = Time.time;

        //Move the rubble in random 
        startPosition = transform.position;
        endPosition = transform.position + new Vector3(UnityEngine.Random.Range(-3f, 3f), -transform.position.y - Random.Range(35f, 40f), Random.Range(0, 1f) > 0.2f ? 20f : -20f);
        Vector3 position33 = UtilityLibrary.Lerp(startPosition, endPosition, 0.33f, LerpMode.EaseOut);
        Vector3 position66 = UtilityLibrary.Lerp(startPosition, endPosition, 0.33f, LerpMode.EaseOut);
        p1 = startPosition + new Vector3(position33.x, startPosition.y + 20f, position33.z);
        p2 = startPosition + new Vector3(position66.x, startPosition.y + 10f, position66.z);
        startScale = transform.localScale;
        if (endPosition.z > 0f)
        {
            endScale = new Vector3(0.1f, 0.1f, 1f);
        }
        else
        {
            //Set sortinglayer to UI so the objects are in front of everything
            sprite.sortingLayerName = "Buildings";
            sprite.sortingOrder = 5;
            endScale = new Vector3(Random.Range(1, 1.5f), Random.Range(1, 1.5f), 1f);
        }

    }

    void FixedUpdate()
    {
        if (isLerping)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentage = timeSinceStarted / moveTime;
            float position = UtilityLibrary.Lerp(0, 1, percentage, LerpMode.EaseIn);
            //transform.position = UtilityLibrary.CalculateCubicBezierPoint(position, startPosition, p1, p2, endPosition);
            transform.position = UtilityLibrary.Lerp(startPosition, endPosition, percentage, LerpMode.EaseIn);
            transform.localScale = UtilityLibrary.Lerp(startScale, endScale, percentage, LerpMode.EaseIn);
            //print(percentage);
            //When we've completed the lerp, we set _isLerping to false
            if (percentage >= 1.0f)
            {
                isLerping = false;
                //Destroy(this.gameObject);
            }
        }
    }
}

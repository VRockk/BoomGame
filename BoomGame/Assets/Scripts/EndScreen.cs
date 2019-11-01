using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public Animator animator;
    public IngameHUD ingameHUD;
    public int testScore = -1;
    void Awake()
    {
        animator.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        ingameHUD = FindObjectOfType<IngameHUD>();
        //print(ingameHUD);
        if(testScore > -1)
        {
            Enable(testScore);
        }
        var camPos = Camera.main.transform.position;
        transform.position = new Vector3(camPos.x, camPos.y, 0);


        //Scale to correct size with different camera zoom values
        var percentage = (Camera.main.orthographicSize - 25f) / 20f;
        transform.localScale = UtilityLibrary.Lerp(new Vector3(4.9f, 4.9f, 1f), new Vector3(8.5f, 8.5f, 1f), percentage, LerpMode.Linear);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Start is called before the first frame update
    public void Enable(int score)
    {
        print(score);
        animator.SetInteger("Score", score);
        animator.enabled = true;
    }

    public void ShowLevelFinishedScreen()
    {
        ingameHUD.ShowFinishPanel();
        //Destroy(gameObject);
    }
}

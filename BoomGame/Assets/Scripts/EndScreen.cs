using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public Animator animator;
    public IngameHUD ingameHUD;
    void Awake()
    {
        animator.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        ingameHUD = FindObjectOfType<IngameHUD>();
        print(ingameHUD);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Start is called before the first frame update
    public void Enable(int score)
    {
        animator.SetInteger("Score", score);
        animator.enabled = true;
    }

    public void ShowLevelFinishedScreen()
    {
        ingameHUD.ShowFinishPanel();
    }
}

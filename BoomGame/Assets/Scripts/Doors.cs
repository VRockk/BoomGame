using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{

    public Animator animator;
    private GameMaster gameMaster;
    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster != null)
        {
            if (gameMaster.doorOpen)
                animator.SetBool("doorOpen", true);
            else
                animator.SetBool("doorOpen", false);
        }
        else
            animator.SetBool("doorOpen", true);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OpenDoor()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster != null)
        {
            gameMaster.doorOpen = true;
            animator = GetComponent<Animator>();
            animator.SetInteger("openState", 1);
        }
    }

    public void CloseDoor()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster != null)
        {
            gameMaster.doorOpen = false;
            animator = GetComponent<Animator>();
            animator.SetInteger("openState", 2);
        }
    }
}

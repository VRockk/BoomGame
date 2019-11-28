using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {


        //OpenDoor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenDoor()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("opening", true);
    }

    public void CloseDoor()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("opening", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        var audio = GetComponent<AudioSource>();
        //var animator = GetComponent<Animator>();
        float delay = 5f;
        if (audio != null && audio.clip != null)
        {
            //print(audio.gameObject.name);
            //destroy after audio clip ends
            delay = audio.clip.length + 0.1f;
            print(audio.clip.length);
        }
        //if(animator != null)
        //{
        //    animator.GetCurrentAnimatorStateInfo(0).length
        //}


        Invoke("Destroy", delay);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}

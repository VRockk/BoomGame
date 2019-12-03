using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEvents : MonoBehaviour
{
    public bool clickToSkip = false;
    private GameMaster gameMaster;
    private AudioSource audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null)
            Debug.LogWarning("No GameMaster found");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clickToSkip)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var animator = GetComponent<Animator>();
                if(animator != null)
                {
                    //print("Skip anim");
                    animator.SetFloat("AnimSpeed", 2f);



                    clickToSkip = false;
                }
            }
        }

    }

    public void PlaySound(AudioClip sound)
    {

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.volume = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(sound);
    }

    public void SetMusic(AudioClip music)
    {
        if (gameMaster == null)
            gameMaster = FindObjectOfType<GameMaster>();
        gameMaster.SetMusic(music);
    }

    public void DestroyParent()
    {
        if (transform.parent != null)
            Destroy(transform.parent.gameObject);
    }

    public void OpenLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}

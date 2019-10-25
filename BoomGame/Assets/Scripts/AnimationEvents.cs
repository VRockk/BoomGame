using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private GameMaster gameMaster;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null)
            Debug.LogError("No GameMaster found");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(AudioClip sound)
    {
        AudioSource.PlayClipAtPoint(sound, new Vector3(0, 0, 0));
    }

    public void SetMusic(AudioClip music)
    {
        gameMaster.SetMusic(music);
    }

    public void DestroyParent()
    {
        if (transform.parent != null)
            Destroy(transform.parent.gameObject);
    }
}

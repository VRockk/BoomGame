using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreline : MonoBehaviour
{

    public GameObject leftHot;
    public GameObject rightHot;
    public GameObject leftFlame;
    public GameObject rightFlame;
    private SpriteRenderer spriteLeft;
    private SpriteRenderer spriteRight;

    private float lerpTime = 0.5f;
    private bool lerping = false;
    private float lerpingStarted;

    [HideInInspector]
    public bool isHot = false;

    // Start is called before the first frame update
    void Start()
    {
        if (leftHot != null && rightHot != null)
        {
            spriteLeft = leftHot.GetComponent<SpriteRenderer>();
            spriteRight = rightHot.GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lerping)
        {
            float timeSinceStarted = Time.time - lerpingStarted;
            float percentage = timeSinceStarted / lerpTime;
            float alpha = UtilityLibrary.Lerp(0f, 1f, percentage, LerpMode.EaseIn);

            if (spriteLeft != null && spriteRight != null)
            {
                spriteLeft.color = new Color(1, 1, 1, alpha);
                spriteRight.color = new Color(1, 1, 1, alpha);
            }
            if (percentage >= 1)
            {
                percentage = 0;
                lerping = false;
                lerpingStarted = Time.time;
            }
        }
    }

    public void SetLineHot(bool isHot)
    {
        if (this.isHot != isHot)
        {
            if (spriteLeft != null && spriteRight != null)
            {
                lerping = true;
                lerpingStarted = Time.time;
                this.isHot = isHot;
            }
            if (isHot)
            {
                leftFlame.SetActive(true);
                rightFlame.SetActive(true);

                var particles = FindObjectsOfType<ParticleSystem>();
                foreach (var particle in particles)
                {
                    particle.Play();
                }
            }
            else
            {
                var particles = FindObjectsOfType<ParticleSystem>();
                foreach (var particle in particles)
                {
                    particle.Stop();
                }
                leftFlame.SetActive(false);
                rightFlame.SetActive(false);
            }
        }
    }
}

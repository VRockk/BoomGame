using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Metal beams bends from normal explosions. Acid "melts" them.
/// </summary>
public class Metal : BuildingObject
{
    public Sprite bendLeftSprite;
    public Sprite bendRightSprite;
    public bool meltVertical = false;

    private bool melting = false;
    private float meltSpeed = 0.5f;

   

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    protected override void Awake()
    {
        materialType = MaterialType.Metal;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
        if (melting)
        {
            float scale = 1f;
            if (meltVertical)
            {
                scale = transform.localScale.y - (Time.deltaTime * meltSpeed);
                //var xScale = transform.localScale.x + (Time.deltaTime * meltSpeed);
                transform.localScale = new Vector3(transform.localScale.x, scale, transform.localScale.z);
            }
            else
            {
                scale = transform.localScale.x - (Time.deltaTime * meltSpeed);
                //var xScale = transform.localScale.x + (Time.deltaTime * meltSpeed);
                transform.localScale = new Vector3(scale, transform.localScale.y, transform.localScale.z);

            }
            if (scale <= 0.0f)
            {
                melting = false;
                Destroy(gameObject);
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    public void Bend(Vector3 explosionPos)
    {
        //var spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = bendLeftSprite;
    }

    public void Melt()
    {
        var joints = GetComponents<FixedJoint2D>();
        foreach (var joint in joints)
        {
            Destroy(joint);
        }
        foreach (var attachedObjectName in attachedObjects)
        {
            //TODO should reference the object for this so we didnt have to use find
            var attachedObject = GameObject.Find(attachedObjectName);
            if (attachedObject != null)
            {
                joints = attachedObject.GetComponents<FixedJoint2D>();
                foreach (var joint in joints)
                {
                    if (joint.connectedBody != null && joint.connectedBody.gameObject != null)
                    {
                        if (joint.connectedBody.gameObject.name == name)
                        {
                            Destroy(joint);
                        }
                    }
                }
            }
        }

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.3f, 1, 0.3f, 1);
        }
        melting = true;
        gameController.AddToScore(scoreValue);
        //Destroy(gameObject);
    }

}

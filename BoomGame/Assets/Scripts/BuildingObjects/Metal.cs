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

    [HideInInspector]
    public bool allowDamage = true;

    private bool melting = false;
    private float meltSpeed = 1f;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    protected override void Awake()
    {
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
            var yScale = transform.localScale.y - (Time.deltaTime * meltSpeed);
            transform.localScale = new Vector3(1, yScale, 1);

            if (yScale <= 0.0f)
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
        foreach(var attachedObjectName in attachedObjects)
        {
            var attachedObject = GameObject.Find(attachedObjectName);
            if (attachedObject != null)
            {
                joints = attachedObject.GetComponents<FixedJoint2D>();
                foreach (var joint in joints)
                {
                    if (joint.connectedBody.gameObject.name == name)
                    {
                        Destroy(joint);
                    }
                }
            }
        }
        melting = true;
        //Destroy(gameObject);
    }

}

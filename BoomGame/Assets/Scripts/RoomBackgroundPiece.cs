﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBackgroundPiece : MonoBehaviour
{
    [SerializeField]
    private int hitpoints = 2;
    public int Hitpoints
    {
        get => hitpoints;
        set
        {
            if (value < 0)
                this.hitpoints = 0;
            else
                this.hitpoints = value;
        }
    }

    [Tooltip("The force when the object is damaged if hit. Lower == Easier to destroy/shatter")]
    public float damagedForceLimit = 30000.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyPart(2f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator DestroyPart(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Hitpoints <= 0)
            Destroy(gameObject);

        var contact = collision.GetContact(0);

        if (contact.rigidbody == null)
            return;

        // Force equals mass times acceleration
        var hitForce = contact.rigidbody.mass * contact.relativeVelocity.magnitude * contact.relativeVelocity.magnitude;

        //print(hitForce);
        //Check if the force is over the limit and apply damage
        //if (contact.relativeVelocity.magnitude > damagedVelocity && Hitpoints > 0)
        if (hitForce > damagedForceLimit)
        {
            hitpoints--;
            if (hitpoints <= 0)
                Destroy(gameObject);
        }
    }
}

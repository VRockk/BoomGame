using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bricks are building objects that can shatter to smaller pieces
/// </summary>
public class Brick : BuildingObject
{
    public Vector2[] pieceSpawnLocations;
    public GameObject[] pieceObjects;

    public int hitpoints = 2;

    [Tooltip("The force when the object is damaged if hit. Lower == Easier to destroy/shatter")]
    public float damagedForceLimit = 10000.0f;

    [HideInInspector]
    public bool allowDamage = true;

    private bool shattered = false;
    private float damageGateDelay = 0f;
    private const float damageDelayDefault = 0.05f;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        //Draw indicators on piece spawn locations
        if (pieceSpawnLocations.Length > 0)
        {
            foreach (var pieceLocation in pieceSpawnLocations)
            {
                Gizmos.color = Color.blue;
                Vector3 localPosition = (this.transform.right * pieceLocation.x * this.transform.localScale.x) + (this.transform.up * pieceLocation.y * this.transform.localScale.y);
                Gizmos.DrawSphere(this.transform.position + new Vector3(localPosition.x, localPosition.y, 0.0f), 0.2f);
            }
        }
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
        if (!allowDamage)
        {
            //Gate for damaging the building. Only allow damaging after a short delay after the previous damage
            damageGateDelay -= Time.deltaTime;
            if (damageGateDelay <= 0)
                allowDamage = true;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (allowDamage)
        {
            damageGateDelay = damageDelayDefault;
            allowDamage = false;
            if (hitpoints <= 0)
                return;

            var contact = collision.GetContact(0);

            if (contact.rigidbody == null)
                return;

            // Force equals mass times acceleration
            var hitForce = contact.rigidbody.mass * contact.relativeVelocity.magnitude * contact.relativeVelocity.magnitude;

            //Check if the force over the limit and apply damage. No more hitpoints -> Shatter
            //if (contact.relativeVelocity.magnitude > damagedVelocity)
            if (hitForce > damagedForceLimit)
            {
                contact.rigidbody.constraints = RigidbodyConstraints2D.None;
                //print(hitForce);
                hitpoints--;

                if (hitpoints == 0)
                    Shatter(this.transform.position, 200, 100);
            }
        }
    }
    
    /// <summary>
    /// Shatters the object into smaller pieces
    /// Spawn new game object instances and sends them flying away from the force
    /// </summary>
    /// <param name="explosionPos">Position of the explosion origin</param>
    /// <param name="power"> Power of the explosion force</param>
    /// <param name="upwardsForce">Additive power added upwards during explosion</param>
    public void Shatter(Vector3 explosionPos, float power, float upwardsForce)
    {
        if (shattered)
            return;

        shattered = true;

        //TODO optimization. We need create the new objects already in start because instantiating object can be laggy and set them invisible or something like that

        //Instantiate new objects
        List<GameObject> newObjects = new List<GameObject>();
        for (int e = 0; e < pieceObjects.Length; e++)
        {
            var newObject = Instantiate(pieceObjects[e]);

            //Set the position showed by the gizmos
            if (pieceSpawnLocations.Length >= e)
            {

                Vector3 localPosition = (this.transform.right * pieceSpawnLocations[e].x) + (this.transform.up * pieceSpawnLocations[e].y);
                newObject.transform.position = this.transform.position + new Vector3(localPosition.x, localPosition.y, 0.0f);
                newObject.transform.localRotation = this.transform.localRotation;
            }
            newObjects.Add(newObject);
        }

        Destroy(this.gameObject);

        //Add explosion force to new objects
        foreach (var newObject in newObjects)
        {
            Rigidbody2D rigidBody = newObject.GetComponent<Rigidbody2D>();
            var brick = newObject.GetComponent<Brick>();

            if (rigidBody != null)
            {
                //add force to spawned small pieces
                Vector2 force = UtilityLibrary.CalculateExplosionForce(explosionPos, newObject.transform.position, power, upwardsForce);
                rigidBody.AddForce(force, ForceMode2D.Impulse);
            }
            if (brick != null)
            {
                brick.createJoints = false;
                brick.allowDamage = false;
                brick.damageGateDelay = 0.05f;
            }
        }
    }

}

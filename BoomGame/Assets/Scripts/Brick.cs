using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public Vector2[] pieceSpawnLocations;
    public GameObject[] pieceObjects;

    public float gravityScale = 1.0f;
    public float mass = 100.0f;
    public int hitpoints = 5;

    [Tooltip("The velocity limit when the object is damaged if hit. Lower == Easier to destroy/shatter")]
    public float damagedVelocity = 30.0f;

    //Does the object have an object below initially. If it doesnt, we dont need to do groundcheck in the beginning
    private bool hasInitialObjectBelow = false;
    private bool hasInitialObjectLeft = false;
    private bool hasInitialObjectRight = false;
    private bool isGrounded = false;
    private float distanceGround;
    private float distanceSides;
    private bool shattered = false;
    private Rigidbody2D rigidBody;

    void OnDrawGizmos()
    {
        //Draw indicators on piece spawn locations
        if (pieceSpawnLocations.Length > 0)
        {
            foreach (var pieceLocation in pieceSpawnLocations)
            {
                Gizmos.color = Color.blue;
                Vector3 localPosition = this.transform.right * pieceLocation.x + (this.transform.up * pieceLocation.y);
                Gizmos.DrawSphere(this.transform.position + new Vector3(localPosition.x, localPosition.y, 0.0f), 0.2f);
            }
        }
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody == null)
            Debug.LogError("No Rigidbody found for object using Brick: " + this.gameObject.name);
        distanceGround = GetComponent<Collider2D>().bounds.extents.y;
        distanceSides = GetComponent<Collider2D>().bounds.extents.x;

        if (SurroundsCheck(-Vector2.up, distanceGround + 0.1f, false))
        {
            hasInitialObjectBelow = true;
        }
        else
        {
            hasInitialObjectBelow = false;
            if (SurroundsCheck(Vector2.left, distanceSides + 0.1f, false))
            {
                hasInitialObjectLeft = true;
            }

            if (SurroundsCheck(Vector2.right, distanceSides + 0.1f, false))
            {
                hasInitialObjectRight = true;
            }
        }

        //No attached parts. set to dynamic from the beginnings
        if (!hasInitialObjectBelow && !hasInitialObjectLeft && !hasInitialObjectRight)
        {
            SetObjectDynamic();
        }
    }

    void FixedUpdate()
    {
        //If not moving
        if (rigidBody.velocity.magnitude == 0)
        {
            //Do ground check only if there was an object below this object initially
            if (hasInitialObjectBelow)
            {
                if (SurroundsCheck(-Vector2.up, distanceGround + 0.1f, true))
                {
                    if (Mathf.Equals(rigidBody.velocity, Vector2.zero))
                    {
                        isGrounded = true;
                    }
                }
                else
                {
                    isGrounded = false;
                    SetObjectDynamic();
                }
            }
            else
            {
                //Check left and right if this object is "attached" to something
                if ((hasInitialObjectLeft && !SurroundsCheck(Vector2.left, distanceSides + 0.1f, true)) ||
                   (hasInitialObjectRight && !SurroundsCheck(Vector2.right, distanceSides + 0.1f, true)))
                {
                    SetObjectDynamic();
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var contact = collision.GetContact(0);
        Debug.DrawRay(contact.point, contact.normal, Color.white);

        //Check if the velocity is over the limit and apply damage. No more hitpoints -> Shatter
        if (contact.relativeVelocity.magnitude > damagedVelocity)
        {
            hitpoints--;

            if (hitpoints <= 0)
                Shatter(this.transform.position, 100, 100);
        }
    }


    #region PUBLIC

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

        //Instantiate new objects
        List<GameObject> newObjects = new List<GameObject>();
        for (int e = 0; e < pieceObjects.Length; e++)
        {
            var newObject = Instantiate(pieceObjects[e]);

            //Set the position showed by the gizmos
            if (pieceSpawnLocations.Length >= e)
            {
                Vector3 localPosition = this.transform.right * pieceSpawnLocations[e].x + (this.transform.up * pieceSpawnLocations[e].y);
                newObject.transform.position = this.transform.position +
                    new Vector3(
                        localPosition.x,
                        localPosition.y,
                        0.0f);

                //newObject.transform.localRotation = this.transform.localRotation;
                //this.transform.right 
            }
            newObjects.Add(newObject);
        }

        Destroy(this.gameObject);

        //Add explosion force to new objects
        foreach (var newObject in newObjects)
        {
            Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 force = UtilityLibrary.CalculateExplosionForce(explosionPos, newObject.transform.position, power, upwardsForce);

                rb.AddForce(force, ForceMode2D.Impulse);

            }
        }
    }

    #endregion


    #region PRIVATE

    private void SetObjectDynamic()
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.mass = mass;
        rigidBody.gravityScale = gravityScale;

    }

    private bool SurroundsCheck(Vector2 traceDirection, float distance, bool checkIfMoving)
    {
        bool value = false;

        //Need to disable this objects collider??  Issue here was that the raytrace would hit this objects collider first
        GetComponent<Collider2D>().enabled = false;

        //Do raycast to check if there are objects next to this one
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), traceDirection, distance);

        value = hit;

        if (hit && checkIfMoving)
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                // Return false if the collided object is moving
                value = !(hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > 0);
            }
        }

        GetComponent<Collider2D>().enabled = true;

        return value;
    }

    #endregion

}

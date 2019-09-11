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

    //Does the object have an object below initially. If it doesnt, we dont need to do groundcheck in the beginning
    private bool hasInitialObjectBelow = false;
    private bool hasInitialObjectLeft = false;
    private bool hasInitialObjectRight= false;
    private bool isGrounded = false;
    private float distanceGround;
    private float distanceSides;

    private Rigidbody2D rigidBody;

    void OnDrawGizmos()
    {
        //Draw indicators on piece spawn locations
        if (pieceSpawnLocations.Length > 0)
        {
            foreach (var pieceLocation in pieceSpawnLocations)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawSphere(this.transform.position + new Vector3(pieceLocation.x, pieceLocation.y, 0.0f), 0.2f);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        distanceGround = GetComponent<Collider2D>().bounds.extents.y;
        distanceSides = GetComponent<Collider2D>().bounds.extents.x;

        if (SurroundsCheck(-Vector2.up, distanceGround + 0.1f))
        {
            hasInitialObjectBelow = true;
        }
        else
        {
            hasInitialObjectBelow = false;
            if (SurroundsCheck(Vector2.left, distanceSides + 0.1f))
            {
                hasInitialObjectLeft = true;
            }

            if (SurroundsCheck(Vector2.right, distanceSides + 0.1f))
            {
                hasInitialObjectRight = true;
            }
        }
    }

    void FixedUpdate()
    {
        //Do ground check only if there was an object below this object initially
        if (hasInitialObjectBelow)
        {
            RaycastHit2D hit = SurroundsCheck(-Vector2.up, distanceGround + 0.1f);
            if (hit)
            {
                var rigidBody = GetComponent<Rigidbody2D>();
                if (Mathf.Equals(rigidBody.velocity, Vector2.zero))
                {
                    isGrounded = true;
                }
            }
            else
            {
                isGrounded = false;
                var rigidBody = GetComponent<Rigidbody2D>();

                rigidBody.bodyType = RigidbodyType2D.Dynamic;
                rigidBody.mass = mass;
                rigidBody.gravityScale = gravityScale;
            }
        }
        else
        {
            //Check left and right if this object is "attached" to something
            if((hasInitialObjectLeft && !SurroundsCheck(Vector2.left, distanceSides + 0.1f)) ||
               (hasInitialObjectRight && !SurroundsCheck(Vector2.right, distanceSides + 0.1f)))
            {

                //No attachments. Set to dynamic
                var rigidBody = GetComponent<Rigidbody2D>();
                rigidBody.bodyType = RigidbodyType2D.Dynamic;
                rigidBody.mass = mass;
                rigidBody.gravityScale = gravityScale;
            }
        }
    }

    private RaycastHit2D SurroundsCheck(Vector2 traceDirection, float distance)
    {
        //Need to disable this objects collider?? What would be better way?
        GetComponent<Collider2D>().enabled = false;
        //Do raycast under the object to check if it is grounded or not. 
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), traceDirection, distance);
        GetComponent<Collider2D>().enabled = true;

        return hit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        
    void OnCollisionEnter(Collision collision)
    {
        print("Pew");
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
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

        //Instantiate new objects
        List<GameObject> newObjects = new List<GameObject>();
        for (int e = 0; e < pieceObjects.Length; e++)
        {
            var newObject = Instantiate(pieceObjects[e]);

            //Set the position showed by the gizmos
            if (pieceSpawnLocations.Length >= e)
            {
                newObject.transform.position =
                    new Vector3(
                        this.transform.position.x + pieceSpawnLocations[e].x,
                        this.transform.position.y + pieceSpawnLocations[e].y,
                        transform.position.z);

                //newObject.transform.localRotation = this.transform.localRotation;
                //this.transform.right 
            }
            newObjects.Add(newObject);
        }

        Destroy(this.gameObject);

        foreach (var newObject in newObjects)
        {
            Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Calculating the direction from the bomb placement to the overlapping 
                Vector2 heading = newObject.transform.position - explosionPos;
                float distance = heading.magnitude;
                Vector2 direction = heading / distance;

                // Calculate force from the direction multiplied by the power. Force weaker by distance
                Vector2 force = direction * (power / distance);

                // Add additional upwards force
                force = force + new Vector2(0, upwardsForce);

                rb.AddForce(force, ForceMode2D.Impulse);

            }
        }
    }

}

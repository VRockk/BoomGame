using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteringObject : MonoBehaviour
{
    public Vector2[] pieceSpawnLocations;
    public GameObject[] pieceObjects;

    public int hitpoints = 2;
    public bool canShatter = true;
    public float jointBreakForce = 50000f;
    public float jointBreakTorque = 50000f;

    [Tooltip("The force when the object is damaged if hit. Lower == Easier to destroy/shatter")]
    public float damagedForceLimit = 10000.0f;


    [Tooltip("Is this object is ignored when checking for level clear?")]
    public bool ignoreForClear = false;

    [HideInInspector]
    public bool isGrounded = true;

    [HideInInspector]
    public bool createJoints = true;

    [HideInInspector]
    public Vector3 initialPosition;

    //Does the object have an object below initially. If it doesnt, we dont need to do groundcheck in the beginning
    private bool hasInitialObjectBelow = false;
    private bool hasInitialObjectLeft = false;
    private bool hasInitialObjectRight = false;
    private float distanceGround;
    private float distanceSides;
    private bool shattered = false;
    private Rigidbody2D rigidBody;
    private bool allowDamage = true;
    private float damageGateDelay = 0f;
    private const float damageDelayDefault = 0.05f;


    public List<string> attachedObjects = new List<string>();

    void OnDrawGizmos()
    {
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

    void Awake()
    {
    }

    void Start()
    {
        initialPosition = this.transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody == null)
            Debug.LogError("No Rigidbody found for object using Brick: " + this.gameObject.name);
        distanceGround = GetComponent<Collider2D>().bounds.extents.y;
        distanceSides = GetComponent<Collider2D>().bounds.extents.x;

        CreateJoints();

        //var attachedTop
        //check if any object below
        //if (SurroundsCheck(-Vector2.up, distanceGround + 0.1f, false))
        //{

        //    //hasInitialObjectBelow = true;
        //}

        //if (SurroundsCheck(-Vector2.up, distanceGround + 0.1f, false))
        //{
        //    hasInitialObjectBelow = true;
        //}
        //else
        //{
        //    hasInitialObjectBelow = false;
        //    if (SurroundsCheck(Vector2.left, distanceSides + 0.1f, false))
        //    {
        //        hasInitialObjectLeft = true;
        //    }

        //    if (SurroundsCheck(Vector2.right, distanceSides + 0.1f, false))
        //    {
        //        hasInitialObjectRight = true;
        //    }
        //}

        ////No attached parts. set to dynamic from the beginnings
        //if (!hasInitialObjectBelow && !hasInitialObjectLeft && !hasInitialObjectRight)
        //{
        //    SetObjectDynamic();
        //}
    }

    void FixedUpdate()
    {
        //If not moving

        //if (rigidBody.velocity.magnitude == 0)
        //{
        //    //Do ground check only if there was an object below this object initially
        //    if (hasInitialObjectBelow)
        //    {
        //        if (SurroundsCheck(-Vector2.up, distanceGround + 0.1f, true))
        //        {
        //            if (Mathf.Equals(rigidBody.velocity, Vector2.zero))
        //            {
        //                isGrounded = true;
        //            }
        //        }
        //        else
        //        {
        //            isGrounded = false;
        //            SetObjectDynamic();
        //        }
        //    }
        //    else
        //    {
        //        //Check left and right if this object is "attached" to something
        //        if ((hasInitialObjectLeft && !SurroundsCheck(Vector2.left, distanceSides + 0.1f, true)) ||
        //           (hasInitialObjectRight && !SurroundsCheck(Vector2.right, distanceSides + 0.1f, true)))
        //        {
        //            SetObjectDynamic();
        //        }
        //    }
        //}
    }

    void Update()
    {
        if (!allowDamage)
        {
            //Gate for damaging the building. Only allow damaging after a short delay after the previous damage
            damageGateDelay -= Time.deltaTime;
            if (damageGateDelay <= 0)
                allowDamage = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
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
        if (shattered || !canShatter)
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
                newObject.transform.position = this.transform.position +
                    new Vector3(
                        localPosition.x,
                        localPosition.y,
                        0.0f);
                //if(newObject.name.ToLower().Contains("wood"))
                //print(newObject.transform.position + "   " + localPosition);


                //newObject.transform.localRotation = this.transform.localRotation;
                //this.transform.right 
            }
            newObjects.Add(newObject);
        }

        Destroy(this.gameObject);

        //Add explosion force to new objects
        foreach (var newObject in newObjects)
        {
            Rigidbody2D rigidBody = newObject.GetComponent<Rigidbody2D>();
            ShatteringObject shatteringObject = newObject.GetComponent<ShatteringObject>();

            if (rigidBody != null)
            {
                //print(explosionPos);

                //if (newObject.name.ToLower().Contains("wood"))
                //    print(explosionPos + "   " + newObject.transform.position);
                Vector2 force = UtilityLibrary.CalculateExplosionForce(explosionPos, newObject.transform.position, power, upwardsForce);

                rigidBody.AddForce(force, ForceMode2D.Impulse);
            }
            if (shatteringObject != null)
                shatteringObject.createJoints = false;
        }
    }

    /// <summary>
    /// Checks the direction if there are is another object in that direction.
    /// </summary>
    /// <param name="traceDirection"></param>
    /// <param name="distance"></param>
    /// <param name="checkIfMoving"></param>
    /// <param name="hitObject">Object that was hit</param>
    /// <returns></returns>
    private bool SurroundsCheck(Vector2 traceDirection, float distance, out GameObject hitObject)
    {
        bool value = false;
        hitObject = null;
        //gameObject = null;
        //Need to disable this objects collider??  Issue here was that the raytrace would hit this objects collider first
        GetComponent<Collider2D>().enabled = false;

        //Do raycast to check if there are objects next to this one
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), traceDirection, distance);

        value = hit;

        if (hit)
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                //print(hit.collider.gameObject.name);
                hitObject = hit.collider.gameObject;
            }
        }

        GetComponent<Collider2D>().enabled = true;

        return value;
    }
    
    /// <summary>
    /// Creates and attaches joints to objects next to this object
    /// </summary>
    private void CreateJoints()
    {
        if (!createJoints)
            return;

        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            GameObject upObject, downObject, leftObject, rightObject;

            List<FixedJoint2D> joints = new List<FixedJoint2D>();
            //Check if there are object next to this object and attach them to the joints.

            //Check above
            if (SurroundsCheck(Vector2.up, distanceGround + 0.1f, out upObject))
            {
                if (upObject != null)
                {
                    var shatteringObject = upObject.GetComponent<ShatteringObject>();
                    //Check that we only have one joint between two objects
                    if (shatteringObject != null && !shatteringObject.attachedObjects.Contains(this.gameObject.name))
                    {
                        //Create a new joint and attach object to it
                        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                        attachedObjects.Add(upObject.name);
                        joint.connectedBody = upObject.GetComponent<Rigidbody2D>();
                        // Set anchor points to the edges of the object
                        //joint.anchor = new Vector2(0, collider.bounds.extents.y);
                        joints.Add(joint);
                    }
                }
            }

            //Check below
            if (SurroundsCheck(-Vector2.up, distanceGround + 0.1f, out downObject))
            {
                if (downObject != null)
                {
                    var shatteringObject = downObject.GetComponent<ShatteringObject>();
                    //if ((shatteringObject != null && !shatteringObject.attachedObjects.Contains(this.gameObject.name)) || shatteringObject == null)

                    if (shatteringObject != null && !shatteringObject.attachedObjects.Contains(this.gameObject.name))
                    {
                        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                        attachedObjects.Add(downObject.name);
                        joint.connectedBody = downObject.GetComponent<Rigidbody2D>();
                        //joint.anchor = new Vector2(0, -collider.bounds.extents.y);
                        joints.Add(joint);
                    }
                }
            }

            //Check right
            if (SurroundsCheck(Vector2.right, distanceSides + 0.1f, out rightObject))
            {
                if (rightObject != null)
                {
                    var shatteringObject = rightObject.GetComponent<ShatteringObject>();
                    if (shatteringObject != null && !shatteringObject.attachedObjects.Contains(this.gameObject.name))
                    {
                        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                        attachedObjects.Add(rightObject.name);
                        joint.connectedBody = rightObject.GetComponent<Rigidbody2D>();
                        //joint.anchor = new Vector2(collider.bounds.extents.x, 0);
                        joints.Add(joint);
                    }
                }
            }

            //Check left
            if (SurroundsCheck(-Vector2.right, distanceSides + 0.1f, out leftObject))
            {
                if (leftObject != null)
                {
                    var shatteringObject = leftObject.GetComponent<ShatteringObject>();
                    if (shatteringObject != null && !shatteringObject.attachedObjects.Contains(this.gameObject.name))
                    {
                        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                        attachedObjects.Add(leftObject.name);
                        joint.connectedBody = leftObject.GetComponent<Rigidbody2D>();
                        //joint.anchor = new Vector2(-collider.bounds.extents.x, 0);
                        joints.Add(joint);
                    }
                }
            }

            //Set default values for all the new joints 
            foreach(var joint in joints)
            {
                joint.enableCollision = true;
                joint.autoConfigureConnectedAnchor = false;
                joint.breakForce = jointBreakForce;
                joint.breakTorque = jointBreakTorque;
                //var limits = new JointAngleLimits2D();
                //limits.min = 0;
                //limits.max = 0;
                //joint.limits = limits;
                //joint.useMotor = true;
                //var motor = new JointMotor2D();
                //motor.maxMotorTorque = 1000000;
                //motor.motorSpeed = 10000;
            }
        }
    }
}

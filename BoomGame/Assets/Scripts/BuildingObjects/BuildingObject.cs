using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    public float jointBreakForce = 50000f;
    public float jointBreakTorque = 50000f;
    public bool createJoints = true;
    public bool createManualJoints = false;
    public bool createJointToGround = false;

    public List<GameObject> ignoredJoints = new List<GameObject>();

    public List<Vector2> manualJointLocations = new List<Vector2>();

    [HideInInspector]
    public MaterialType materialType = MaterialType.None;

    [HideInInspector]
    public List<string> attachedObjects = new List<string>();

    [HideInInspector]
    public bool allowDamage = true;

    [HideInInspector]
    public bool deactivateDelay = false;

    [HideInInspector]
    public bool checkedInLevelClear = true;

    protected virtual void OnDrawGizmos()
    {
        foreach (var jointLocation in manualJointLocations)
        {

            Gizmos.color = Color.green;
            Vector3 localPosition = (this.transform.right * jointLocation.x * this.transform.localScale.x) + (this.transform.up * jointLocation.y * this.transform.localScale.y);
            //Vector3 localPosition = new Vector3(jointLocation.x, jointLocation.y, 0f);
            Gizmos.DrawSphere(this.transform.position + localPosition, 0.2f);

        }
    }
    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        //Dont allow creating joints to parent objects. 
        if (this.transform.parent != null)
            ignoredJoints.Add(this.transform.parent.gameObject);

        CreateJoints();
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void Update()
    {
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
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
        Debug.DrawRay(transform.position, traceDirection * distance, Color.red, 5f);

        value = hit;

        if (hit)
        {
            if (!ignoredJoints.Contains(hit.collider.gameObject))
            {
                if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                {
                    //print(hit.collider.gameObject.name);
                    hitObject = hit.collider.gameObject;
                }
                else if (createJointToGround && hit.collider.gameObject.tag == "Ground")
                {
                    hitObject = hit.collider.gameObject;
                }
            }
        }

        GetComponent<Collider2D>().enabled = true;

        return value;
    }

    /// <summary>
    /// Creates and attaches joints to objects next to this object
    /// </summary>
    protected void CreateJoints()
    {
        if (createManualJoints)
        {
            foreach (var jointLocation in manualJointLocations)
            {
                Vector3 position = this.transform.position + (this.transform.right * jointLocation.x * this.transform.localScale.x) + (this.transform.up * jointLocation.y * this.transform.localScale.y);
                Vector3 traceDirection = Vector3.Normalize(position - this.transform.position);
                //traceDirection = traceDirection);
                var traceDistance = Vector3.Distance(this.transform.position, position);

                GameObject otherObject;
                if (SurroundsCheck(traceDirection, traceDistance, out otherObject))
                {
                    CreateNewJoint(otherObject);
                }

            }
        }

        if (!createJoints)
            return;

        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            float distanceTop = collider.bounds.extents.y;
            float distanceSides = collider.bounds.extents.x;
            GameObject upObject, downObject, leftObject, rightObject;


            //Check if there are object next to this object and attach them to the joints.

            //Check above
            if (SurroundsCheck(Vector2.up, distanceTop + 0.1f, out upObject))
            {
                CreateNewJoint(upObject);
            }

            //Check below
            if (SurroundsCheck(-Vector2.up, distanceTop + 0.1f, out downObject))
            {
                CreateNewJoint(downObject);
            }

            //Check right
            if (SurroundsCheck(Vector2.right, distanceSides + 0.1f, out rightObject))
            {
                CreateNewJoint(rightObject);
            }

            //Check left
            if (SurroundsCheck(-Vector2.right, distanceSides + 0.1f, out leftObject))
            {
                CreateNewJoint(leftObject);
            }
        }
    }

    private void CreateNewJoint(GameObject otherObject)
    {
        if (otherObject != null)
        {
            var otherShatteringObject = otherObject.GetComponent<BuildingObject>();

            //if these objects are not already connected add a joint
            if (!ignoredJoints.Contains(otherObject) &&
                (createJointToGround && otherObject.tag == "Ground") || (otherShatteringObject != null && !otherShatteringObject.ignoredJoints.Contains(gameObject) && !otherShatteringObject.attachedObjects.Contains(this.gameObject.name)))
            {
                var joint = gameObject.AddComponent<FixedJoint2D>();
                attachedObjects.Add(otherObject.name);
                joint.connectedBody = otherObject.GetComponent<Rigidbody2D>();
                joint.enableCollision = true;
                joint.autoConfigureConnectedAnchor = false;
                joint.breakForce = jointBreakForce;
                joint.breakTorque = jointBreakTorque;
                joint.dampingRatio = 0f;
                if (otherShatteringObject != null)
                {
                    otherShatteringObject.attachedObjects.Add(name);
                }
            }
        }
    }
}

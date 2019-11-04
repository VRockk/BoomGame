using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingProp : MonoBehaviour
{
    public float damagedForceLimit = 3000f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var contact = collision.GetContact(0);

        if (contact.rigidbody == null)
            return;

        if (contact.rigidbody.gameObject.tag == "Rubble")
            return;

        // Force equals mass times acceleration
        var hitForce = contact.rigidbody.mass * contact.relativeVelocity.magnitude * contact.relativeVelocity.magnitude;

        //print(hitForce);
        //print(hitForce);
        //Check if the force is over the limit and apply damage
        //if (contact.relativeVelocity.magnitude > damagedVelocity && Hitpoints > 0)
        if (hitForce > damagedForceLimit)
        {
            Destroy(gameObject);
        }
    }
}

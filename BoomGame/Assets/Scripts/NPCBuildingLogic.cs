using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBuildingLogic : MonoBehaviour
{
    [Tooltip("How many hits the building can take before it is destroyed.")]
    public int hitpoints = 3;
    
    [Tooltip("The buildings sprites corresponding to hitpoints. If building has 3 hitpoints it NEEDS to have 4 sprites. Last being the non-damaged one, first one being the destroyed one.")]
    public Sprite[] buildingSprites;

    [Tooltip("The force when the object is damaged if hit. Lower == Easier to destroy/shatter")]
    public float damagedForceLimit = 50000.0f;

    private SpriteRenderer spriteRenderer;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitpoints <= 0)
            return;

        var contact = collision.GetContact(0);

        if (contact.rigidbody == null)
            return;

        // Force equals mass times acceleration
        var hitForce = contact.rigidbody.mass * contact.relativeVelocity.magnitude * contact.relativeVelocity.magnitude;

        //print(hitForce);
        //Check if the force is over the limit and apply damage
        //if (contact.relativeVelocity.magnitude > damagedVelocity && hitpoints > 0)
        if (hitForce > damagedForceLimit && hitpoints > 0)
        {
            hitpoints--;

            DamageBuilding();
        }
    }

    public void DamageBuilding()
    {
        //TODO Error Checking
        //switch to next building sprite
        spriteRenderer.sprite = buildingSprites[hitpoints];
        if (hitpoints == 0)
        {
            //TODO Show smoke cloud effect when building is destroyed

            //Disable collisions for now when building is destroyed.
            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer == null)
        {
            Debug.LogError("No spriterendered found from object implementing NPCBuildingLogic. " + this.gameObject.name);
        }
        else
        {
            // TODO Error Checking
            spriteRenderer.sprite = buildingSprites[hitpoints];
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

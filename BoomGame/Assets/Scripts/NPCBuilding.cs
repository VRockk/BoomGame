using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBuilding : MonoBehaviour
{
    [Tooltip("How many hits the building can take before it is destroyed.")]
    [SerializeField]
    private int hitpoints = 3;
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

    [Tooltip("The buildings sprites corresponding to Hitpoints. If building has 3 Hitpoints it NEEDS to have 4 sprites. Last being the non-damaged one, first one being the destroyed one.")]
    public Sprite[] buildingSprites;

    [Tooltip("The force when the object is damaged if hit. Lower == Easier to destroy/shatter")]
    public float damagedForceLimit = 70000.0f;

    [HideInInspector]
    public int maxHitpoints;

    public GameObject smokeAnimation;

    private SpriteRenderer spriteRenderer;
    private bool allowDamage = true;
    private float damageGateDelay = 0f;
    private const float damageDelayDefault = 0.25f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Hitpoints <= 0)
            return;

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
            DamageBuilding(1, true);
        }
    }

    void Awake()
    {
        maxHitpoints = hitpoints;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No spriterendered found from object implementing NPCBuildingLogic. " + this.gameObject.name);
        }
        else
        {
            // TODO Error Checking
            spriteRenderer.sprite = buildingSprites[Hitpoints];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!allowDamage)
        {
            //print(damageGateDelay);
            //Gate for damaging the building. Only allow damaging after a short delay after the previous damage
            damageGateDelay -= Time.deltaTime;
            if (damageGateDelay <= 0)
                allowDamage = true;
        }
    }



    public void DamageBuilding(int damageAmount, bool addDamageDelay)
    {
        if (Hitpoints <= 0)
        {
            return;
        }

        if (allowDamage)
        {
            Hitpoints -= damageAmount;

            if (addDamageDelay)
            {
                damageGateDelay = damageDelayDefault;
                allowDamage = false;
            }

            //switch to next building sprite
            //print(Hitpoints);
            spriteRenderer.sprite = buildingSprites[Hitpoints];
            if (Hitpoints == 0)
            {
                print("Building destroyed: " + this.gameObject.name);
                //TODO Show smoke cloud effect when building is destroyed

                Instantiate(smokeAnimation, transform.position, Quaternion.identity);
                //Disable collisions for now when building is destroyed.
                var collider = GetComponent<Collider2D>();
                if (collider != null)
                    collider.enabled = false;

            }
        }
    }
}

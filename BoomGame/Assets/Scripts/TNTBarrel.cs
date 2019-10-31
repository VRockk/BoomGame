using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTBarrel : MonoBehaviour
{

    public float power = 10000f;
    public float upwardsForce = 100f;
    public float damagedForceLimit = 7000f;

    public GameObject explosion;

    public GameObject explosionParticles;

    public float radius = 6f;
    public float damageRadius = 10f;

    public bool showExplosionGizmo = false;

    // Start is called before the first frame update
    void Start()
    {

    }    

    protected virtual void OnDrawGizmos()
    {
        if (showExplosionGizmo)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawSphere(this.transform.position, damageRadius);

            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(this.transform.position, radius);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
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
            Detonate(0.1f);
        }
    }

    public void Detonate(float delay)
    {
        StartCoroutine(Explode(delay));
    }

    protected IEnumerator Explode(float delay)
    {
        yield return new WaitForSeconds(delay);

        //Spawn explosion animation
        if (explosion != null)
        {
            var exp = Instantiate(explosion, this.transform.position, Quaternion.identity);
            exp.transform.localScale = new Vector3(5f, 5f, 1f);
        }

        if (explosionParticles != null)
            Instantiate(explosionParticles, this.transform.position, Quaternion.identity);

        ExplosionEffect();
        Destroy(this.gameObject);

    }


    protected virtual void ExplosionEffect()
    {

        if (radius > 0f)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, radius);

            //Objects inside main explosive radius. Shatter, destroy, add force
            foreach (Collider2D hit in colliders)
            {
                ExplosionForces(hit);
            }
        }
        if (damageRadius > 0f)
        {
            //Remove hitpoints on these
            Collider2D[] damageColliders = Physics2D.OverlapCircleAll(this.transform.position, damageRadius);

            foreach (Collider2D hit in damageColliders)
            {
                if (hit.gameObject.tag.Contains("BuildingObject"))
                {
                    var brick = hit.gameObject.GetComponent<Brick>();
                    if (brick != null && brick.allowDamage)
                    {
                        //Remove hitpoints
                        brick.hitpoints--;
                        brick.hitpoints--;

                        //No more hitpoints, shatter
                        if (brick.hitpoints <= 0)
                        {
                            brick.Shatter(this.transform.position, 100, 100);
                        }
                        else
                        {
                            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                            if (rb != null && hit.gameObject.tag != "Ground")
                            {
                                Vector2 force = UtilityLibrary.CalculateExplosionForceWithDistance(this.transform.position, hit.transform.position, power, upwardsForce);

                                rb.AddForce(force, ForceMode2D.Impulse);
                            }
                        }
                    }
                }
                else
                {
                    Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                    if (rb != null && hit.gameObject.tag != "Ground")
                    {
                        Vector2 force = UtilityLibrary.CalculateExplosionForceWithDistance(this.transform.position, hit.transform.position, power, upwardsForce);

                        rb.AddForce(force, ForceMode2D.Impulse);
                    }
                }
            }
        }

    }

    private void ExplosionForces(Collider2D hit)
    {
        if (hit.gameObject.tag.Contains("BuildingObject"))
        {
            var buildingObject = hit.gameObject.GetComponent<BuildingObject>();
            if (buildingObject != null)
                print(hit);
            if (buildingObject.materialType == MaterialType.Brick)
            {
                var brick = hit.gameObject.GetComponent<Brick>();
                if (brick != null && brick.allowDamage)
                    brick.Shatter(this.transform.position, power, upwardsForce);
            }
            else if (buildingObject.materialType == MaterialType.Wood)
            {
                Destroy(hit.transform.gameObject);
            }
            else if (buildingObject.materialType == MaterialType.Metal)
            {
                var metal = hit.gameObject.GetComponent<Metal>();
                if (metal != null)
                {
                    metal.Bend(transform.position);
                }
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    Vector2 force = UtilityLibrary.CalculateExplosionForce(this.transform.position, hit.transform.position, power, upwardsForce);

                    rb.AddForce(force, ForceMode2D.Impulse);
                }
            }

        }
        else if (hit.gameObject.tag.Contains("NPCBuilding"))
        {
            var npcHouse = hit.gameObject.GetComponent<NPCBuilding>();

            if (npcHouse != null)
            {
                //Destroy NPC buildings if they are hit by the blast
                npcHouse.DamageBuilding(10000, false);
            }
        }
        else
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null && hit.gameObject.tag != "Ground")
            {
                print(hit.gameObject);
                Vector2 force = UtilityLibrary.CalculateExplosionForceWithDistance(this.transform.position, hit.transform.position, power, upwardsForce);

                rb.AddForce(force, ForceMode2D.Impulse);
            }

            var barrel = hit.gameObject.GetComponent<TNTBarrel>();
            if (barrel != null)
            {
                barrel.Detonate(0.1f);
            }
        }
    }
}

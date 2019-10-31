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
                UtilityLibrary.ExplosionForces(hit, this.transform, power, upwardsForce);
            }
        }
        if (damageRadius > 0f)
        {
            //Remove hitpoints on these
            Collider2D[] damageColliders = Physics2D.OverlapCircleAll(this.transform.position, damageRadius);

            foreach (Collider2D hit in damageColliders)
            {
                UtilityLibrary.ExplosionDamage(hit, this.transform, power, upwardsForce);
            }
        }
    }
}

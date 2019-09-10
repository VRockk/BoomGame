using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    //Delay in seconds before exploding
    public float delay = 1.0f;

    public float destroyRadius = 1.0f;
    public float radius = 1.0f;
    public float damageRadius = 1.0f;
    public float power = 100.0f;
    public float upwardsForce = 100.0f;
    public bool showPlacementGizmo = true;
    public bool showExplosionGizmo = false;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        if (showExplosionGizmo)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawSphere(this.transform.position, damageRadius);

            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(this.transform.position, radius);

            Gizmos.color = Color.red;

            Gizmos.DrawSphere(this.transform.position, destroyRadius);
        }
        if (showPlacementGizmo)
        {
            Gizmos.color = Color.white;

            Gizmos.DrawSphere(this.transform.position, 0.5f);

        }
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(delay);
        Vector3 explosionPos = transform.position;

        Collider2D[] destroyColliders = Physics2D.OverlapCircleAll(explosionPos, destroyRadius);
        foreach (Collider2D hit in destroyColliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Destroy(hit.transform.gameObject);
            }
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);

        //Objects inside main explosive radius
        foreach (Collider2D hit in colliders)
        {
            if (hit.gameObject.tag.Contains("ShatteringObject"))
            {

                hit.gameObject.GetComponent<Brick>().Shatter(explosionPos, power, upwardsForce);
            }
            else

            {
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    // Calculating the direction from the bomb placement to the overlapping 
                    Vector2 heading = hit.transform.position - explosionPos;
                    float distance = heading.magnitude;
                    Vector2 direction = heading / distance;

                    //Calculate force from the direction multiplied by the power. Force weaker by distance
                    Vector2 force = direction * (power / distance);

                    // Add additional upwards force
                    force += new Vector2(0, upwardsForce);

                    rb.AddForce(force, ForceMode2D.Impulse);
                }
            }
        }
        Destroy(this.gameObject);
    }
}

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

    public void Detonate()
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
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

        //Remove hitpoints on these
        Collider2D[] damageColliders = Physics2D.OverlapCircleAll(explosionPos, damageRadius);

        foreach (Collider2D hit in damageColliders)
        {
            if (hit.gameObject.tag.Contains("ShatteringObject"))
            {
                //Remove hitpoints
                hit.gameObject.GetComponent<Brick>().hitpoints--;

                if (hit.gameObject.GetComponent<Brick>().hitpoints <= 0)
                    hit.gameObject.GetComponent<Brick>().Shatter(hit.transform.position, 100, 100);
            }
            else if (hit.gameObject.tag.Contains("NPCBuilding"))
            {
                //Remove hitpoints
                hit.gameObject.GetComponent<NPCBuildingLogic>().hitpoints--;
                hit.gameObject.GetComponent<NPCBuildingLogic>().DamageBuilding();
            }
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);

        //Objects inside main explosive radius. Shatter, destroy, add force
        foreach (Collider2D hit in colliders)
        {
            if (hit.gameObject.tag.Contains("ShatteringObject"))
            {

                hit.gameObject.GetComponent<Brick>().Shatter(explosionPos, power, upwardsForce);
            }
            else if (hit.gameObject.tag.Contains("NPCBuilding"))
            {
                //Destroy NPC buildings if they are hit by the blast
                hit.gameObject.GetComponent<NPCBuildingLogic>().hitpoints = 0;
                hit.gameObject.GetComponent<NPCBuildingLogic>().DamageBuilding();
            }
            else
            {
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    Vector2 force = UtilityLibrary.CalculateExplosionForce(explosionPos, hit.transform.position, power, upwardsForce);

                    rb.AddForce(force, ForceMode2D.Impulse);
                }
            }
        }
        Destroy(this.gameObject);
    }

}

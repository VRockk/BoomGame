using System;
using System.Collections;
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
    public bool showExplosionGizmo = false;
    public AudioClip[] exposionScreamSounds;
    public GameObject explosion;

    public Sprite inventoryIcon;


    public GameObject explosionParticles;

    protected virtual void Awake()
    {
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
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

            Gizmos.color = Color.red;

            Gizmos.DrawSphere(this.transform.position, destroyRadius);
        }
    }

    public void Detonate()
    {
        PlayBombScreamSound();
        StartCoroutine(Explode());
    }

    private void PlayBombScreamSound()
    {
        //Get random scream sound and play it.
        //TODO play only one scream, now we play multiple sounds if we have multiple bombs
        if (exposionScreamSounds.Length > 0)
        {
            var screamSound = exposionScreamSounds[UnityEngine.Random.Range(0, exposionScreamSounds.Length)];
            AudioSource.PlayClipAtPoint(screamSound, Camera.main.transform.position);
        }
    }

    protected IEnumerator Explode()
    {
        yield return new WaitForSeconds(delay);


        //Spawn explosion animation
        if (explosion != null)
            Instantiate(explosion, this.transform.position, Quaternion.identity);

        if(explosionParticles != null)
            Instantiate(explosionParticles, this.transform.position, Quaternion.identity);

        ExplosionEffect();
        Destroy(this.gameObject);
    }

    protected virtual void ExplosionEffect()
    {
        if (destroyRadius > 0f)
        {
            //Destroy bricks in destroy radius
            Collider2D[] destroyColliders = Physics2D.OverlapCircleAll(this.transform.position, destroyRadius);
            foreach (Collider2D hit in destroyColliders)
            {
                var buildingObject = hit.gameObject.GetComponent<BuildingObject>();
                if (buildingObject != null)
                {
                    if (buildingObject.materialType == MaterialType.Brick || buildingObject.materialType == MaterialType.Wood)
                    {
                        if (buildingObject.allowDamage)
                            Destroy(hit.transform.gameObject);
                    }
                }
            }
        }

        if (radius > 0f)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, radius);

            //Objects inside main explosive radius. Shatter, destroy, add force
            foreach (Collider2D hit in colliders)
            {
                if (hit.gameObject.tag.Contains("BuildingObject"))
                {
                    var buildingObject = hit.gameObject.GetComponent<BuildingObject>();

                    if (buildingObject.materialType == MaterialType.Brick)
                    {
                        var brick = hit.gameObject.GetComponent<Brick>();
                        if (brick != null && brick.allowDamage)
                            brick.Shatter(this.transform.position, power, upwardsForce);
                    }
                    else if(buildingObject.materialType == MaterialType.Wood)
                    {
                        Destroy(hit.transform.gameObject);
                    }
                    else if(buildingObject.materialType == MaterialType.Metal)
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
                        Vector2 force = UtilityLibrary.CalculateExplosionForce(this.transform.position, hit.transform.position, power, upwardsForce);

                        rb.AddForce(force, ForceMode2D.Impulse);
                    }
                }
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
}

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
    public bool showPlacementGizmo = true;
    public bool showExplosionGizmo = false;
    public AudioClip[] exposionScreamSounds;
    public GameObject explosion;

    public Sprite inventoryIcon;

    private AudioSource audioSource;
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("AudioSource not found in the scene for the Bomb");
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

        //Get random scream sound and play it.
        //TODO play only one scream, now we play multiple sounds if we have multiple bombs
        if (exposionScreamSounds.Length > 0)
        {
            var screamSound = exposionScreamSounds[Random.Range(0, exposionScreamSounds.Length)];
            if (audioSource != null)
                audioSource.PlayOneShot(screamSound);
        }

        yield return new WaitForSeconds(delay);

        Vector3 explosionPos = this.transform.position;

        //Spawn explosion animation
        //Probably not the right way to do this 
        if (explosion != null)
            Instantiate(explosion, explosionPos, Quaternion.identity);


        //Get objects in destroy radius and destory them. Smallest radius
        Collider2D[] destroyColliders = Physics2D.OverlapCircleAll(explosionPos, destroyRadius);
        foreach (Collider2D hit in destroyColliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null && hit.gameObject.tag != "Ground")
            {
                Destroy(hit.transform.gameObject);
            }
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);

        //Objects inside main explosive radius. Shatter, destroy, add force
        foreach (Collider2D hit in colliders)
        {
            if (hit.gameObject.tag.Contains("ShatteringObject"))
            {
                var shatteringObject = hit.gameObject.GetComponent<ShatteringObject>();
                if (shatteringObject != null)
                    shatteringObject.Shatter(explosionPos, power, upwardsForce);
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
                    Vector2 force = UtilityLibrary.CalculateExplosionForce(explosionPos, hit.transform.position, power, upwardsForce);

                    rb.AddForce(force, ForceMode2D.Impulse);
                }
            }
        }

        //Remove hitpoints on these
        Collider2D[] damageColliders = Physics2D.OverlapCircleAll(explosionPos, damageRadius);

        foreach (Collider2D hit in damageColliders)
        {
            if (hit.gameObject.tag.Contains("ShatteringObject"))
            {
                var shatteringObject = hit.gameObject.GetComponent<ShatteringObject>();
                if (shatteringObject != null)
                {
                    //Remove hitpoints
                    shatteringObject.hitpoints--;

                    //No more hitpoints, shatter
                    if (shatteringObject.hitpoints <= 0)
                    {
                        shatteringObject.Shatter(explosionPos, 100, 100);
                    }
                    else
                    {

                        Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                        if (rb != null && hit.gameObject.tag != "Ground")
                        {
                            Vector2 force = UtilityLibrary.CalculateExplosionForce(explosionPos, hit.transform.position, power, upwardsForce);

                            rb.AddForce(force, ForceMode2D.Impulse);
                        }
                    }
                }
            }
        }
        Destroy(this.gameObject);
    }

}

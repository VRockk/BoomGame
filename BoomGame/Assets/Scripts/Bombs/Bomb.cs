﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //Delay in seconds before exploding
    public float explosionDelay = 1.0f;

    public float destroyRadius = 1.0f;
    public float radius = 1.0f;
    public float damageRadius = 1.0f;
    public float power = 100.0f;
    public float upwardsForce = 100.0f;
    public bool showExplosionGizmo = false;
    public AudioClip exposionScreamSound;
    public GameObject explosion;

    public GameObject bombAreaIndicator;
    public Sprite inventoryIcon;


    public GameObject explosionParticles;

    public BombType bombType;

    private GameMaster gameMaster;

    private CameraShake camShake;

    private PhoneVibration phoVibration;

    protected virtual void Awake()
    {
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        this.gameObject.name = this.gameObject.name + Guid.NewGuid().ToString();
        gameMaster = FindObjectOfType<GameMaster>();

        camShake = Camera.main.GetComponent<CameraShake>();

        phoVibration = Camera.main.GetComponent<PhoneVibration>();

        if (gameMaster == null)
            Debug.LogError("No GameMaster found in bomb upgrade panel");

        var bombData = gameMaster.bombData.First(x => x.BombType == bombType);

        //Calculate bomb radius from the bomb upgrade level
        radius = radius * (1 + (0.05f * ((float)bombData.Level -1 )));
        damageRadius = damageRadius * (1 + (0.05f * ((float)bombData.Level -1 )));

        var indicatorRadius = radius / 5;
        bombAreaIndicator.transform.localScale = new Vector3(indicatorRadius, indicatorRadius, 1);
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

    public void Detonate(float extraDelay)
    {
        StartCoroutine(Explode(extraDelay));
    }

    protected IEnumerator Explode(float extraDelay)
    {
        yield return new WaitForSeconds(explosionDelay + extraDelay);


        //Spawn explosion animation
        if (explosion != null)
            Instantiate(explosion, this.transform.position, Quaternion.identity);

        if (explosionParticles != null)
            Instantiate(explosionParticles, this.transform.position, Quaternion.identity);

        ExplosionEffect();

        //For some weird reason this doesnt work after another level is loaded!??!?

        if (camShake != null)
            camShake.Shake(0.5f, 1f, 60, 30, true);

        if (phoVibration != null)
            phoVibration.Vibrate();

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
                //if(hit.gameObject.name.ToLower().Contains("concrete"))
                //print(this.name + ":  " + hit.gameObject.name);
                UtilityLibrary.ExplosionForces(hit, this.transform, power, upwardsForce);
            }
        }

        if (damageRadius > 0f)
        {
            //Remove hitpoints on these
            Collider2D[] damageColliders = Physics2D.OverlapCircleAll(this.transform.position, damageRadius);

            foreach (Collider2D hit in damageColliders)
            {
                //if (hit.gameObject.name.ToLower().Contains("concrete"))
                //    print(this.name + ":  " + hit.gameObject.name);
                UtilityLibrary.ExplosionDamage(hit, this.transform, power, upwardsForce);
            }
        }

        if (radius > 0f)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, radius*1.5f);

            //Objects inside main explosive radius. Shatter, destroy, add force
            foreach (Collider2D hit in colliders)
            {
                //if (hit.gameObject.name.ToLower().Contains("concrete"))
                //    print(this.name + ":  " + hit.gameObject.name);
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    Vector2 force = UtilityLibrary.CalculateExplosionForceWithDistance(transform.position, hit.transform.position, power/2, upwardsForce);

                    rb.AddForce(force, ForceMode2D.Impulse);
                    var torque = (UnityEngine.Random.Range(0f, 1f) > 0.5f ? -10f : 10f) * force.magnitude;
                    rb.AddTorque(torque);
                }
            }
        }

    }

}

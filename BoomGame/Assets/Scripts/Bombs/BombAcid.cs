using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAcid : Bomb
{

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    protected override void ExplosionEffect()
    {
        if (destroyRadius > 0f)
        {
            Collider2D[] destroyColliders = Physics2D.OverlapCircleAll(this.transform.position, destroyRadius);
            foreach (Collider2D hit in destroyColliders)
            {
            }
        }

        if (radius > 0f)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, radius);
            foreach (Collider2D hit in colliders)
            {
                if (hit.gameObject.tag.Contains("BuildingObject"))
                {
                    var metal = hit.gameObject.GetComponent<Metal>();
                    if (metal != null)
                    {
                        metal.Melt();
                    }
                }
            }
        }

        if (damageRadius > 0f)
        {
            Collider2D[] damageColliders = Physics2D.OverlapCircleAll(this.transform.position, damageRadius);
            foreach (Collider2D hit in damageColliders)
            {
            }
        }
    }

}

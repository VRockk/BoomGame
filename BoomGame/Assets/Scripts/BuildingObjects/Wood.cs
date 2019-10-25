using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wood burns from fire 
/// </summary>
public class Wood : BuildingObject
{


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    protected override void Awake()
    {
        materialType = MaterialType.Wood;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
       
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    public void Burn()
    {
        
    }

}

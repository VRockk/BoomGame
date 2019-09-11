using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityLibrary
{
   

    public static Vector3 CalculateExplosionForce(Vector3 explosionPos, Vector3 hitObjectPosition, float power, float upwardsForce)
    {
        Vector2 force;
        // Calculating the direction from the bomb placement to the overlapping 
        Vector2 heading = hitObjectPosition - explosionPos;
        float distance = heading.magnitude;
        Vector2 direction = heading / distance;

        //Calculate force from the direction multiplied by the power. Force weaker by distance
        force = direction * (power / distance);

        // Add additional upwards force
        force += new Vector2(0, upwardsForce);
        return force;
    }

}

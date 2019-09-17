using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum LevelClear
{
    Failed = -1, // Any building destroyed
    NotCleared = 0, // Some blocks not moved
    OnePentagram = 1, // Level cleared on second round, some damage to buildings
    TwoPentagram = 2, // Either level cleared on second round and no damage to buildings, OR Level cleared on first round and some damage to buildings
    ThreePentagram = 3 // Level cleared on first round and no damage to buildings
}

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

    public static bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

}

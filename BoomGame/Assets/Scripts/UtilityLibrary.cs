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

public enum MaterialType
{
    None = 0,
    Brick = 1,
    Metal = 2,
    Wood = 3
}

public class UtilityLibrary
{
    public static Vector3 CalculateExplosionForceWithDistance(Vector3 explosionPos, Vector3 hitObjectPosition, float power, float upwardsForce)
    {
        Vector2 force;
        // Calculating the direction from the bomb placement to the overlapping 
        Vector2 heading = hitObjectPosition - explosionPos;
        float distance = heading.magnitude;
        if (distance == 0)
            distance = 1;
        Vector2 direction = heading / distance;

        //Calculate force from the direction multiplied by the power. Force weaker by distance
        force = direction * (power / distance);

        // Add additional upwards force
        force += new Vector2(0, upwardsForce);
        return force;
    }

    public static Vector3 CalculateExplosionForce(Vector3 explosionPos, Vector3 hitObjectPosition, float power, float upwardsForce)
    {
        Vector2 force;
        // Calculating the direction from the bomb placement to the overlapping 
        Vector2 heading = hitObjectPosition - explosionPos;
        float distance = heading.magnitude;
        if (distance == 0)
            distance = 1;
        Vector2 direction = heading / distance;

        //Calculate force from the direction multiplied by the power. Force the same no matter the distance
        force = direction * power;

        // Add additional upwards force
        force += new Vector2(0, upwardsForce);
        return force;
    }

    public static bool IsMouseOverUI()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;

        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        //check touch
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }

        return false;
    }


    /// <summary>
    /// Get the current mouse position
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetCurrentMousePosition()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;
        return mousePos;
    }

}

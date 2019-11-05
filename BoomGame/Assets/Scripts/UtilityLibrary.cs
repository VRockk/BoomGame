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


public enum LerpMode
{
    Linear = 0,
    EaseOut = 1,
    EaseIn = 2,
    Smoothstep = 3,
    SuperSmoothstep = 4,
    Exponential = 5
}


public class UtilityLibrary : MonoBehaviour
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

    public static Vector3 Lerp(Vector3 start, Vector3 finish, float percentage, LerpMode lerpMode = LerpMode.Linear)
    {
        //Make sure percentage is in the range [0.0, 1.0]
        percentage = Mathf.Clamp01(percentage);
        percentage = Smoothing(percentage, lerpMode);
        return (1 - percentage) * start + percentage * finish;
    }

    public static float Lerp(float start, float finish, float percentage, LerpMode lerpMode)
    {
        //Make sure percentage is in the range [0.0, 1.0]
        percentage = Mathf.Clamp01(percentage);
        percentage = Smoothing(percentage, lerpMode);

        return (1 - percentage) * start + percentage * finish;
    }

    private static float Smoothing(float percentage, LerpMode lerpMode)
    {
        float val = percentage;

        switch (lerpMode)
        {
            case LerpMode.Linear:
                val = percentage;
                break;
            case LerpMode.EaseOut:
                val = Mathf.Sin(percentage * Mathf.PI * 0.5f);
                break;
            case LerpMode.EaseIn:
                val = 1f - Mathf.Cos(percentage * Mathf.PI * 0.5f);
                break;
            case LerpMode.Smoothstep:
                val = percentage * percentage * (3f - 2f * percentage);
                break;
            case LerpMode.SuperSmoothstep:
                val = percentage * percentage * percentage * (percentage * (6f * percentage - 15f) + 10f);
                break;
            case LerpMode.Exponential:
                val = val * val;
                break;
        }
        return val;
    }

    public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }


    public static void ExplosionForces(Collider2D hit, Transform transform, float power, float upwardsForce)
    {
        if (hit.gameObject.tag.Contains("BuildingObject"))
        {
            var buildingObject = hit.gameObject.GetComponent<BuildingObject>();
            if (buildingObject != null)
            {
                if (buildingObject.materialType == MaterialType.Brick)
                {
                    var brick = hit.gameObject.GetComponent<Brick>();
                    if (brick != null && brick.allowDamage)
                        brick.Shatter(transform.position, power, upwardsForce);
                }
                else if (buildingObject.materialType == MaterialType.Wood)
                {
                    Destroy(hit.transform.gameObject);
                }
                else if (buildingObject.materialType == MaterialType.Metal)
                {
                    var metal = hit.gameObject.GetComponent<Metal>();
                    if (metal != null)
                    {
                        metal.Bend(transform.position);
                    }
                }
            }
            if (hit.gameObject != null)
            {
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    Vector2 force = UtilityLibrary.CalculateExplosionForce(transform.position, hit.transform.position, power, upwardsForce);

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
                //print(hit.gameObject);
                Vector2 force = UtilityLibrary.CalculateExplosionForceWithDistance(transform.position, hit.transform.position, power, upwardsForce);

                rb.AddForce(force, ForceMode2D.Impulse);
            }
            var barrel = hit.gameObject.GetComponent<TNTBarrel>();
            if (barrel != null)
            {
                barrel.Detonate(0.1f);
            }
            var prop = hit.gameObject.GetComponent<BuildingProp>();
            if (prop != null)
            {
                Destroy(prop.gameObject);
            }
        }
    }

    public static void ExplosionDamage(Collider2D hit, Transform transform, float power, float upwardsForce)
    {
        if (hit.gameObject.tag.Contains("BuildingObject"))
        {
            var brick = hit.gameObject.GetComponent<Brick>();
            if (brick != null && brick.allowDamage)
            {
                //Remove hitpoints
                brick.hitpoints--;
                brick.hitpoints--;

                //No more hitpoints, shatter
                if (brick.hitpoints <= 0)
                {
                    brick.Shatter(transform.position, 100, 100);
                }
                else
                {
                    Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                    if (rb != null && hit.gameObject.tag != "Ground")
                    {
                        Vector2 force = UtilityLibrary.CalculateExplosionForceWithDistance(transform.position, hit.transform.position, power, upwardsForce);

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
                Vector2 force = UtilityLibrary.CalculateExplosionForceWithDistance(transform.position, hit.transform.position, power, upwardsForce);

                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}

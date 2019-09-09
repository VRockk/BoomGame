using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickLogic : MonoBehaviour
{
    public Vector2[] pieceSpawnLocations;
    public GameObject[] pieceObjects;


    void OnDrawGizmos()
    {
        if (pieceSpawnLocations.Length > 0)
        {
            foreach (var pieceLocation in pieceSpawnLocations)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawSphere(this.transform.position + new Vector3(pieceLocation.x, pieceLocation.y, 0.0f), 0.2f);
            }
        }
    }

    /// <summary>
    /// Shatters the brick into smaller pieces
    /// Spawn new game object instances and sends them flying away from the force
    /// </summary>
    /// <param name="explosionPos"></param>
    /// <param name="power"></param>
    public void Shatter(Vector3 explosionPos, float power)
    {
        List<GameObject> newObjects = new List<GameObject>();
        for (int e = 0; e < pieceObjects.Length; e++)
        {
            var newObject = Instantiate(pieceObjects[e]);

            //Set the position showed by the gizmos
            if (pieceSpawnLocations.Length >= e)
            {
                newObject.transform.position = new Vector3(this.transform.position.x + pieceSpawnLocations[e].x, this.transform.position.y + pieceSpawnLocations[e].y, transform.position.z);
            }
            newObjects.Add(newObject);

        }


        Destroy(this.gameObject);


        foreach (var newObject in newObjects)
        {
            Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Calculating the direction from the bomb placement to the overlapping 
                Vector2 heading = newObject.transform.position - explosionPos;
                float distance = heading.magnitude;
                Vector2 direction = heading / distance;

                //Calculate force from the direction multiplied by the power. Force weaker by distance
                Vector2 force = direction * (power / distance);
                rb.AddForce(force, ForceMode2D.Impulse);

                //rb.AddForce.AddExplosionForce(power, explosionPos, radius, 3.0F, ForceMode.Impulse);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

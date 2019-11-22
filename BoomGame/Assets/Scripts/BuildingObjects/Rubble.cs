using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubble : MonoBehaviour
{
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        InvokeRepeating("CheckForMovement", 1f, 1f);
    }

    void CheckForMovement()
    {
        if (body != null)
        {
            //Sets to static if not moving to reduce lag
            if (body.velocity.magnitude < 0.1f)
            {
                body.bodyType = RigidbodyType2D.Static;
                body.simulated = false;
                CancelInvoke("CheckForMovement");
            }
        }
    }
}

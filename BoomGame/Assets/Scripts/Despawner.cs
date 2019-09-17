using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        var contact = collision.GetContact(0);

        if (contact.rigidbody == null)
            return;

        //print(contact.collider.gameObject); 
        Destroy(contact.collider.gameObject);
    }

}

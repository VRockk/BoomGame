using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingSprite : MonoBehaviour
{

    public Vector2 amount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
        transform.position = new Vector3(transform.position.x + amount.x, transform.position.y + amount.y, transform.position.z);
    }
}

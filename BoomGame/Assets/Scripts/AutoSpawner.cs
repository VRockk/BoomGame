using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawner : MonoBehaviour
{

    public GameObject objectToSpawn;
    // Start is called before the first frame update
    void Awake()
    {
        if(objectToSpawn != null)
        {
            Instantiate(objectToSpawn, this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

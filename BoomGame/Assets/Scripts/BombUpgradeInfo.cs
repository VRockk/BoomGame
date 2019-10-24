using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombUpgradeInfo : MonoBehaviour
{
    [HideInInspector]
    public int level;

    [HideInInspector]
    public int upgradePosition;

    [HideInInspector]
    public int maxLevel = 3;
    public string upgradeName;
    public string description;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

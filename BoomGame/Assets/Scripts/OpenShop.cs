using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShopPanel : MonoBehaviour
{
    public GameObject shopPanel;
    
    public void OpenPanel()
    {
        if(shopPanel != null)
        { 
            shopPanel.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenScene : MonoBehaviour
{
    public float WaitTime = 7f;
   
    void Start()
    {
        StartCoroutine(WaitForSplash());
        
    }

    IEnumerator WaitForSplash() {
        yield return new WaitForSeconds(WaitTime);
        SceneManager.LoadScene("MainMenuScene");
    }
  
}

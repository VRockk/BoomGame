using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLevelChange : MonoBehaviour
{

    public string levelName = "MainMenu";
    public float delay = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ChangeLevel", delay);

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ChangeLevel()
    {
        //print("sup2");
        //yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }
}

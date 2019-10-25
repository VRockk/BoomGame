using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject slidePanel1;
    public GameObject slidePanel2;
    public GameObject slidePanel3;
    public GameObject slidePanel4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenSlide2()
    {
        slidePanel1.SetActive(false);
        slidePanel2.SetActive(true);

    }
    public void OpenSlide3()
    {
        slidePanel2.SetActive(false);
        slidePanel3.SetActive(true);

    }
    public void OpenSlide4()
    {
        slidePanel3.SetActive(false);
        slidePanel4.SetActive(true);

    }
    public void OpenLevel(string name)
    {
        //TODO show "loading" screen.
        SceneManager.LoadScene(name);

    }
}

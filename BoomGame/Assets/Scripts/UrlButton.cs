using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlButton : MonoBehaviour
{
    private bool urlOpened = false;

    public string URL = "";


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenURL()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        var button = GameObject.Find(name);
        if (button != null)
        {
            var urlButton = button.GetComponent<UrlButton>();
            if (urlButton != null)
            {
                urlOpened = true;

#if UNITY_WEBGL && !UNITY_EDITOR
            Application.ExternalEval("window.open(\"" + URL + "\",\"_blank\")");
#else
                Application.OpenURL(URL);
#endif
            }
        }
    }


    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && urlOpened)
        {
            urlOpened = false;
        }
    }
}

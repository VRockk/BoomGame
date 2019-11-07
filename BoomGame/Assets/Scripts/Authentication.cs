using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Authentication : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private RawImage userImage;
    private bool mWaitingForAuth = false;

    // Start is called before the first frame update
    void Start()
    {
        // Select the Google Play Games platform as our social platform implementation
        GooglePlayGames.PlayGamesPlatform.Activate();

        playButton.interactable = false;
        if (!Social.localUser.authenticated)
        {
            loginButton.GetComponentInChildren<Text>().text = "Sign in";
        }
    }


    public void OnLoginButtonClick()
    {
        if (!Social.localUser.authenticated)
        {
            // Authenticate
            mWaitingForAuth = true;
            statusText.text = "Authenticating...";            

            Social.localUser.Authenticate((bool success) =>
            {
                mWaitingForAuth = false;
                if (success)
                {
                    statusText.text = "Welcome " + Social.localUser.userName;
                    playButton.interactable = true;
                    StartCoroutine("LoadImage");
                    loginButton.GetComponentInChildren<Text>().text = "Sign out";
                }
                else
                {
                    statusText.text = "Authentication failed.";
                }
            });
        }
        else
        {
            statusText.text = "";
            ((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
            playButton.interactable = false;  
            userImage.texture = null;
            loginButton.GetComponentInChildren<Text>().text = "Sign in";
        }
    }

    public void OnPlayButtonClick()
    {
            SceneManager.LoadScene("MainMenuScene");
    }

        private IEnumerator LoadImage()
    {
        while (Social.localUser.image == null)
        {
            //statusText.text = "Loading image...";
            yield return null;
        }
        //statusText.text = "Image found";
        userImage.texture = Social.localUser.image;
    }

}

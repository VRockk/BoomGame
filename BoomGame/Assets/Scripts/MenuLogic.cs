using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    public void GotoLiScene()
    {
        SceneManager.LoadScene("Li Scene");
    }

    public void GotoNewScene()
    {
        SceneManager.LoadScene("New Scene");
    }
}

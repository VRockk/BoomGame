using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public int timeSeconds;
    public string title;
    public string text;

    void Start()
    {
        PushNotification notification = new PushNotification("GenericNotification", timeSeconds, title, text);
    }
}

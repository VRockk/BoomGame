using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public int time;
    public string title;
    public string text;
    
    // Start is called before the first frame update
    void Start()
    {
        CreateNotificationChannel();
        SendNotification();
    }

    public void CreateNotificationChannel()
    {
        var c = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
    }

    public void SendNotification()
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddSeconds(time);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");

        print("Hello world");

    }
}

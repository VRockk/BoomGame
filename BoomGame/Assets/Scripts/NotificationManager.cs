using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public int timeSeconds;
    public string title;
    public string text;

    private AndroidNotification notification;
    private int notificationId;

    // Start is called before the first frame update
    void Start()
    {
        CreateNotificationChannel();

        DontDestroyOnLoad(this.gameObject);
    }

    public void CreateNotificationChannel()
    {
        var c = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications"
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
    }

    public void CreateNotification()
    {
        notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddSeconds(timeSeconds);
        notification.LargeIcon = "icon_large";
        //notification.SmallIcon = "default";

        notificationId = AndroidNotificationCenter.SendNotification(notification, "channel_id");


    }

    void OnApplicationPause(bool pauseStatus)
    {
        //Create notification when we soft close the app. Cancel when we come back
        if(!pauseStatus)
        {
            AndroidNotificationCenter.CancelNotification(notificationId);
        }
        else
        {
            CreateNotification();
        }
    }

}

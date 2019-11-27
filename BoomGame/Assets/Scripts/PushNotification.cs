using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine;

public class PushNotification
{
    int timeSeconds;
    string savedID;
    string title;
    string text;

    public PushNotification(string savedID, int timeSeconds, string title, string text)
    {
        CreateNotificationChannel();
        var existingID = PlayerPrefs.GetInt(savedID, 0);

        //Cancel old notification if exists
        if (existingID != 0)
            AndroidNotificationCenter.CancelNotification(existingID);
        this.savedID = savedID;
        this.timeSeconds = timeSeconds;
        this.title = title;
        this.text = text;

        CreateNotification();
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

        AndroidNotification notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddSeconds(timeSeconds);
        notification.LargeIcon = "icon_large";
        //notification.SmallIcon = "default";

        int notificationId = AndroidNotificationCenter.SendNotification(notification, "channel_id");
        PlayerPrefs.SetInt(savedID, notificationId);
    }
}

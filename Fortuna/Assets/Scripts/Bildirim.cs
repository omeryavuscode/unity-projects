using System;
using System.Collections;
using UnityEngine.Android;
using Unity.Notifications.Android;
//using Unity.Notifications.iOS;
using UnityEngine;

public class Bildirim : MonoBehaviour
{
    private void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Bildirim").Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }

        AndroidNotificationCenter.CancelAllDisplayedNotifications();
        AndroidNotificationCenter.CancelAllScheduledNotifications();

#elif UNITY_IOS
        StartCoroutine(IOSBildirimIzni());

        iOSNotificationCenter.RemoveAllDeliveredNotifications();
        iOSNotificationCenter.RemoveAllScheduledNotifications();

#else
        Debug.Log("Yanlýþ Cihaz");

#endif

    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
#if UNITY_ANDROID
            AndroidBildirim();

#elif UNITY_IOS
            IOSBildirim();

#else
            Debug.Log("Yanlýþ Cihaz");

#endif
        }
        else
        {
#if UNITY_ANDROID
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
            AndroidNotificationCenter.CancelAllScheduledNotifications();

#elif UNITY_IOS
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();

#else
            
.Log("Yanlýþ Cihaz");

#endif
        }
    }

    void AndroidBildirim()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "Fortuna",
            Name = "Fortuna",
            Importance = Importance.Default,
            Description = "Notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        for (int i = 0; i < 50; i++)
        {
            var notification = new AndroidNotification();
            notification.Title = $"Hey! {PlayerPrefs.GetString("kullaniciAdi")} Come Back!";
            notification.Text = "Return to earn money.";
            notification.SmallIcon = "icon_0";
            notification.FireTime = System.DateTime.Now.AddMinutes((i * 60 * 12) + 60);

            AndroidNotificationCenter.SendNotification(notification, "Fortuna");
        }
    }

    //void IOSBildirim()
    //{
    //    var timeTrigger = new iOSNotificationTimeIntervalTrigger()
    //    {
    //        TimeInterval = new TimeSpan(0, 30, 0),
    //        Repeats = true
    //    };

    //    var notification = new iOSNotification()
    //    {
    //        Identifier = "Fortuna",
    //        Title = "Fortuna",
    //        Body = "Hey! Come Back!",
    //        Subtitle = "Return to earn money.",
    //        ShowInForeground = true,
    //        ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
    //        CategoryIdentifier = "category_default",
    //        ThreadIdentifier = "thread1",
    //        Trigger = timeTrigger,
    //    };

    //    iOSNotificationCenter.ScheduleNotification(notification);
    //}

    //IEnumerator IOSBildirimIzni()
    //{
    //    var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
    //    using (var req = new AuthorizationRequest(authorizationOption, true))
    //    {
    //        while (!req.IsFinished)
    //        {
    //            yield return null;
    //        };
    //    }
    //}
}

#undef UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

class LocalCrossNotification
{
    /// <summary>
    /// Inexact uses `set` method
    /// Exact uses `setExact` method
    /// ExactAndAllowWhileIdle uses `setAndAllowWhileIdle` method
    /// Documentation: https://developer.android.com/intl/ru/reference/android/app/AlarmManager.html
    /// </summary>
    public enum NotificationExecuteMode
    {
        Inexact = 0,
        Exact = 1,
        ExactAndAllowWhileIdle = 2
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private static string fullClassName = "net.agasper.unitynotification.UnityNotificationManager";
    private static string mainActivityClassName = "com.unity3d.player.UnityPlayerNativeActivity";
#elif UNITY_IOS && !UNITY_EDITOR
  private static Dictionary <int, UnityEngine.iOS.LocalNotification> iNotiMap = new Dictionary<int, UnityEngine.iOS.LocalNotification>();
#endif

  public static void SendNotification(int id, TimeSpan delay, string title, string message)
    {
        SendNotification(id, (int)delay.TotalSeconds, title, message, Color.white);
    }
    
    public static void SendNotification(int id, long delay, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "", NotificationExecuteMode executeMode = NotificationExecuteMode.Inexact)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetNotification", id, delay * 1000L, title, message, message, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small", bgColor.r * 65536 + bgColor.g * 256 + bgColor.b, (int)executeMode, mainActivityClassName);
        }
#elif UNITY_IOS && !UNITY_EDITOR
    UnityEngine.iOS.LocalNotification iNoti = new UnityEngine.iOS.LocalNotification();
    iNotiMap.Add(id, iNoti);
    iNoti.fireDate = DateTime.Now.AddSeconds ((double) delay);
    iNoti.alertAction = title;
    iNoti.alertBody = message;
    iNoti.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
    iNoti.hasAction = false;    
    UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(iNoti);
#endif
  }

  public static void SendRepeatingNotification(int id, long delay, long timeout, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "")
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetRepeatingNotification", id, delay * 1000L, title, message, message, timeout * 1000, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small", bgColor.r * 65536 + bgColor.g * 256 + bgColor.b, mainActivityClassName);
        }
#elif UNITY_IOS && !UNITY_EDITOR
#endif
  }

  public static void CancelNotification(int id)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null) {
            pluginClass.CallStatic("CancelNotification", id);
        }
#elif UNITY_IOS && !UNITY_EDITOR
    UnityEngine.iOS.NotificationServices.CancelLocalNotification(iNotiMap[id]);
#endif
  }

  public static void CancelAllNotifications()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null)
            pluginClass.CallStatic("CancelAll");
#elif UNITY_IOS && !UNITY_EDITOR
    UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
#endif
  }
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

public class NotificationManager : MonoBehaviour {
  public static NotificationManager nm;
  [SerializeField, EnumMaskField(typeof(NotificationType))]
  private   NotificationType  m_notificationType;

  void Start() {
    nm = this;
    NPBinding.NotificationService.RegisterNotificationTypes(m_notificationType);
  }

  void OnEnable() {
    // NotificationService.DidLaunchWithLocalNotificationEvent     += DidLaunchWithLocalNotificationEvent;
    // NotificationService.DidReceiveLocalNotificationEvent      += DidReceiveLocalNotificationEvent;
  }

  void OnDisable() {
    // NotificationService.DidLaunchWithLocalNotificationEvent     -= DidLaunchWithLocalNotificationEvent;
    // NotificationService.DidReceiveLocalNotificationEvent      -= DidReceiveLocalNotificationEvent;
  }

  public void notifyAfter(int minutes) {
    ScheduleLocalNotification(CreateNotification(minutes * 60, eNotificationRepeatInterval.NONE));
  }

  private string ScheduleLocalNotification (CrossPlatformNotification _notification) {
    return NPBinding.NotificationService.ScheduleLocalNotification(_notification);
  }

  private void CancelLocalNotification (string _notificationID) {
    NPBinding.NotificationService.CancelLocalNotification(_notificationID);
  }

  private void CancelAllLocalNotifications () {
    NPBinding.NotificationService.CancelAllLocalNotification();
  }

  private void ClearNotifications () {
    NPBinding.NotificationService.ClearNotifications();
  }

  private CrossPlatformNotification CreateNotification (long _fireAfterSec, eNotificationRepeatInterval _repeatInterval) {
    // User info
    IDictionary _userInfo     = new Dictionary<string, string>();
    _userInfo["data"]       = "custom data";

    CrossPlatformNotification.iOSSpecificProperties _iosProperties      = new CrossPlatformNotification.iOSSpecificProperties();
    _iosProperties.HasAction    = true;
    _iosProperties.AlertAction    = "alert action";

    CrossPlatformNotification.AndroidSpecificProperties _androidProperties  = new CrossPlatformNotification.AndroidSpecificProperties();
    _androidProperties.ContentTitle = "Smashy Toys";
    _androidProperties.TickerText = "Receive Free Gift!";
    _androidProperties.LargeIcon  = "Logo.png"; //Keep the files in Assets/PluginResources/Android or Common folder.

    CrossPlatformNotification _notification = new CrossPlatformNotification();
    _notification.AlertBody     = "Receive Free Gift!"; //On Android, this is considered as ContentText
    _notification.FireDate      = System.DateTime.Now.AddSeconds(_fireAfterSec);
    _notification.RepeatInterval  = _repeatInterval;
    _notification.SoundName     = "Notification.mp3"; //Keep the files in Assets/PluginResources/Android or iOS or Common folder.
    _notification.UserInfo      = _userInfo;
    _notification.iOSProperties   = _iosProperties;
    _notification.AndroidProperties = _androidProperties;

    return _notification;
  }
}
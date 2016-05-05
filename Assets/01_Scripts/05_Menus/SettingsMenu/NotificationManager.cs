using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SmartLocalization;

public class NotificationManager : MonoBehaviour {
  public static NotificationManager nm;
  private string notifyMsg;
  private HashSet<int> notiIdSet;

  void Start() {
    if (nm != null && nm != this) {
      Destroy(gameObject);
      return;
    }
    notiIdSet = new HashSet<int>();
    Random.seed = System.DateTime.Now.Millisecond;
    DontDestroyOnLoad(gameObject);
    nm = this;
    LanguageManager languageManager = LanguageManager.Instance;
    languageManager.OnChangeLanguage += OnChangeLanguage;
    OnChangeLanguage(languageManager);
  }

  void OnEnable() {
    // NotificationService.DidLaunchWithLocalNotificationEvent     += DidLaunchWithLocalNotificationEvent;
    // NotificationService.DidReceiveLocalNotificationEvent      += DidReceiveLocalNotificationEvent;
  }

  void OnDisable() {
    // NotificationService.DidLaunchWithLocalNotificationEvent     -= DidLaunchWithLocalNotificationEvent;
    // NotificationService.DidReceiveLocalNotificationEvent      -= DidReceiveLocalNotificationEvent;
    if(LanguageManager.HasInstance) {
      LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
    }
  }

  void OnChangeLanguage(LanguageManager languageManager) {
    notifyMsg = LanguageManager.Instance.GetTextValue("Notification_FreeGift");
  }

  public void notifyAfter(int minutes) {
    CancelAllLocalNotifications();
    int id = (int)Random.Range(60000, 900000);
    notiIdSet.Add(id);
    LocalNotification.SendNotification(id, minutes * 60, "Smashy Toys", notifyMsg, new Color32(0xff, 0x44, 0x44, 255));
  }

  private void CancelLocalNotification (int _notificationID) {
    LocalNotification.CancelNotification(_notificationID);
    notiIdSet.Remove(_notificationID);
  }

  public void CancelAllLocalNotifications () {
    foreach (int id in notiIdSet) {
      CancelLocalNotification(id);
    }
    notiIdSet.Clear();
  }
  /*
  private NotificationDefinition CreateNotification (long _fireAfterSec, long _repeatInterval) {
    // User info
    IDictionary _userInfo     = new Dictionary<string, string>();
    _userInfo["data"]       = "custom data";

    NotificationDefinition def = new NotificationDefinition(_fireAfterSec, _repeatInterval);

    CrossPlatformNotification.iOSSpecificProperties _iosProperties      = new CrossPlatformNotification.iOSSpecificProperties();
    _iosProperties.HasAction    = true;
    _iosProperties.AlertAction    = "alert action";

    CrossPlatformNotification.AndroidSpecificProperties _androidProperties  = new CrossPlatformNotification.AndroidSpecificProperties();
    _androidProperties.ContentTitle = "Smashy Toys";
    _androidProperties.TickerText = notifyMsg;
    _androidProperties.LargeIcon  = "Logo.png"; //Keep the files in Assets/PluginResources/Android or Common folder.

    CrossPlatformNotification _notification = new CrossPlatformNotification();
    _notification.AlertBody     = notifyMsg; //On Android, this is considered as ContentText
    _notification.FireDate      = System.DateTime.Now.AddSeconds(_fireAfterSec);
    _notification.RepeatInterval  = _repeatInterval;
    _notification.SoundName     = "Notification.mp3"; //Keep the files in Assets/PluginResources/Android or iOS or Common folder.
    _notification.UserInfo      = _userInfo;
    _notification.iOSProperties   = _iosProperties;
    _notification.AndroidProperties = _androidProperties;

    return _notification;
  }
  */
  
}

using UnityEngine;
using System.Collections;

public class NotificationButton : OnOffButton {

	override public void activateSelf() {
    base.activateSelf();

    if (clicked) {
      NotificationManager.nm.CancelAllLocalNotifications();
    }
  }
}

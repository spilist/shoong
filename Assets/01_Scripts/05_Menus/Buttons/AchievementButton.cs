using UnityEngine;
using System.Collections;

public class AchievementButton : MenusBehavior {
  public override void activateSelf ()
  {
    SocialPlatformManager.spm.authenticate((bool _success) => {
      if (_success == true) {
        SocialPlatformManager.spm.showAchievementUI();
      }
    });
  }
}

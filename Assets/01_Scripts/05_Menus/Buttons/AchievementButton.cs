using UnityEngine;
using System.Collections;

public class AchievementButton : MenusBehavior {
  public override void activateSelf ()
  {
    DataManager.spm.authenticate((bool _success) => {
      if (_success == true) {
        DataManager.spm.showAchievementUI();
      }
    });
  }
}

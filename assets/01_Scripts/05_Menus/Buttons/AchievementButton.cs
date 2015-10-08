using UnityEngine;
using System.Collections;

public class AchievementButton : MenusBehavior {
  public override void activateSelf ()
  {
    Social.ShowAchievementsUI();
  }
}

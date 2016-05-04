using UnityEngine;
using System.Collections;

public class RankingButton : MenusBehavior {
  public override void activateSelf () {
    SocialPlatformManager.spm.showLeaderboardUI();
  }
}

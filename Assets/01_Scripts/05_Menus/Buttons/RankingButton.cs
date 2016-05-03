using UnityEngine;
using System.Collections;

public class RankingButton : MenusBehavior {
  public override void activateSelf ()
  {
    SocialPlatformManager.spm.authenticate((bool _success) => {
      if (_success == true) {
        SocialPlatformManager.spm.showLeaderboardUI();
      }
    });
  }
}

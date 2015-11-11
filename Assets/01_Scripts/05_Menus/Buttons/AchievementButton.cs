using UnityEngine;
using System.Collections;

public class AchievementButton : MenusBehavior {
  public override void activateSelf ()
  {
    DataManager.npbManager.authenticate((bool _success, string _error)=>{
      if (_success == true) {
        DataManager.npbManager.showAchievementUI((string _error2)=>{
          if(_error != null) {
          }
          else {
            Debug.Log("Error = " + _error);
          }
        });
      }
    });
  }
}

using UnityEngine;
using System.Collections;

public class GoogleAuthButton : OnOffButton {
  override public void activateSelf() {    
    if (clicked) {
      DataManager.npbManager.authenticate((bool _success, string _error)=>{      
        if (_success)
        {
          base.activateSelf();
        }
        else
        {
        }
      });
    } else {
      NPBinding.GameServices.LocalUser.SignOut((bool _success, string _error)=>{        
        if (_success)
        {
          base.activateSelf();
        }
        else
        {
        }
      });
    }
  }
}

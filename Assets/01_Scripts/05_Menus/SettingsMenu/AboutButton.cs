using UnityEngine;
using System.Collections;

public class AboutButton : InsideMenusBehavior {
  public CharactersMenu cm;
  public bool cheat;

  override public void afterActivate() {
    if (cheat) cm.allCharacters();
    if (cheat) DataManager.dm.setInt("TotalGoldenCubes", 5000);
    Heyzap.HeyzapAds.ShowMediationTestSuite();
  }
}

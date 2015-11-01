using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RateUsBannerButton : BannerButton {
  public int showAfterGames = 5;

  override public void activateSelf() {
    Application.OpenURL("http://unity3d.com/");

    GetComponent<MeshRenderer>().enabled = false;
    GetComponent<Collider>().enabled = false;

    transform.parent.GetComponent<Text>().text = "            THANKS FOR RATING";
  }

  override public bool available() {
    if (DataManager.dm.getInt("TotalNumPlays") == showAfterGames) return true;
    else return false;
  }
}

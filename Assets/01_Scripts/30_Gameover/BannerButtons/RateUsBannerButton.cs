using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RateUsBannerButton : BannerButton {
  public int showAfterGames = 5;

  override public void activateSelf() {
    Application.OpenURL("market://details?id=com.morogoro.smashytoysspace/");

    GetComponent<MeshRenderer>().enabled = false;
    GetComponent<Collider>().enabled = false;

    transform.parent.GetComponent<Text>().text = secondDescription;
  }

  override public bool available() {
    if (DataManager.dm.getInt("TotalNumPlays") == showAfterGames) return true;
    else return false;
  }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RateUsBannerButton : BannerButton {
  public int showAfterGames = 5;

  override public void activateSelf() {
#if UNITY_ANDROID
    Application.OpenURL("market://details?id=com.morogoro.smashytoysspace");
#elif UNITY_IOS
    Application.OpenURL ("https://itunes.apple.com/app/id1113368167");
#endif

    GetComponent<MeshRenderer>().enabled = false;
    GetComponent<Collider>().enabled = false;

    transform.parent.GetComponent<Text>().text = secondDescription;
  }

  override public bool available() {
		/*
    if (DataManager.dm.getInt("TotalNumPlays") % showAfterGames == 0 && 
			DataManager.dm.getInt("TotalNumPlays") / showAfterGames < 5) return true;
			*/
	if (DataManager.dm.getInt ("TotalNumPlays") == showAfterGames)
		return true;
    else return false;
  }
}

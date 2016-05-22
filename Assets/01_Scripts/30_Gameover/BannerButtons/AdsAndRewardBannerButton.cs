using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
// using UnityEngine.Advertisements;
using Heyzap;

public class AdsAndRewardBannerButton : BannerButton {
  public Text goldenCubeText;
  public GameObject icon;

  public int goldenCubePerAds = 30;
  public int showAdsPerGame = 10;
  public int showAdsPerMinutes = 10;
  public int dailyLimit = 5;
  private bool active = true;

	override public void activateSelf() {
    if (!active) return;

    if (HZIncentivizedAd.IsAvailable()) {
      HZIncentivizedAd.Show();
      gold.change(goldenCubePerAds);

      stopBlink();

      GetComponent<MeshRenderer>().enabled = false;
      GetComponent<Collider>().enabled = false;
      icon.SetActive(false);

      active = false;
      playTouchSound = false;

      transform.parent.GetComponent<Text>().text = secondDescription.Replace("_REWARD_", goldenCubePerAds.ToString());
    }

    // if(Advertisement.IsReady("rewardedVideoZone")){ Advertisement.Show("rewardedVideoZone", new ShowOptions {
    //     resultCallback = result => {
    //       Debug.Log(result.ToString());
    //       if (result == ShowResult.Finished) {
    //         // after ads end, show goldencube get effect
    //         gold.change(goldenCubePerAds);

    //         stopBlink();

    //         GetComponent<MeshRenderer>().enabled = false;
    //         GetComponent<Collider>().enabled = false;
    //         icon.SetActive(false);

    //         active = false;
    //         playTouchSound = false;

    //         transform.parent.GetComponent<Text>().text = secondDescription.Replace("_REWARD_", goldenCubePerAds.ToString());
    //       }
    //     }
    //   });
    // }
  }

  override public bool available() {
    return true;
  }
}

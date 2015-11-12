using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Advertisements;

public class AdsAndRewardBannerButton : BannerButton {
  public Text goldenCubeText;

  public int goldenCubePerAds = 30;
  public int showAdsPerGame = 10;
  public int showAdsPerMinutes = 10;
  public int dailyLimit = 5;
  private bool active = true;

	override public void activateSelf() {
    if (!active) return;

    if(Advertisement.IsReady("rewardedVideoZone")){ Advertisement.Show("rewardedVideoZone", new ShowOptions {
        resultCallback = result => {
          Debug.Log(result.ToString());
          if (result == ShowResult.Finished) {
            // after ads end, show goldencube get effect
            gold.change(goldenCubePerAds);

            DataManager.dm.increment("CurrentGoldenCubes", goldenCubePerAds);
            DataManager.dm.increment("TotalGoldenCubes", goldenCubePerAds);

      			DataManager.dm.save();

            stopBlink();

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            transform.Find("CubeIcon").gameObject.SetActive(false);

            active = false;
            playTouchSound = false;

            transform.parent.GetComponent<Text>().text = secondDescription.Replace("_REWARD_", goldenCubePerAds.ToString());
          }
        }
      });
    }
  }

  override public bool available() {
    return true;
  }
}

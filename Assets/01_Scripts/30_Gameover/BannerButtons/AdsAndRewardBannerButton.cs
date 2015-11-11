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

    Debug.Log("Show ADs");

    if(Advertisement.isReady("rewardedVideoZone")){ Advertisement.Show("rewardedVideoZone", new ShowOptions {
        resultCallback = result => {
          Debug.Log(result.ToString());
          if (result == ShowResult.Finished) {
            // after ads end, show goldencube get effect
            gold.change(goldenCubePerAds);

            DataManager.dm.increment("CurrentGoldenCubes", goldenCubePerAds);
            DataManager.dm.increment("TotalGoldenCubes", goldenCubePerAds);
            
            //goldenCubeText.text = (int.Parse(goldenCubeText.text) + goldenCubePerAds).ToString();
            
            DataManager.dm.increment("DailyAdsCount");
            DataManager.dm.setInt("LastNumPlayAdsSeen", DataManager.dm.getInt("TotalNumPlays"));
            DataManager.dm.setDateTime("LastDateTimeAdsSeen");
            
			DataManager.dm.save();
            stopBlink();
            filter.sharedMesh = inactiveMesh;
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

    // if (DataManager.dm.isAnotherDay("LastDateTimeAdsSeen")) {
    //   DataManager.dm.setInt("DailyAdsCount", 0);
    // }

    // if (DataManager.dm.getInt("DailyAdsCount") > dailyLimit) return false;

    // int gamesPassed = DataManager.dm.getInt("TotalNumPlays") - DataManager.dm.getInt("LastNumPlayAdsSeen");
    // int minutesPassed = DateTime.Now.Minute - DataManager.dm.getDateTime("LastDateTimeAdsSeen").Minute;

    // if (gamesPassed >= showAdsPerGame || minutesPassed >= showAdsPerMinutes) {
    //   return true;
    // }
    // return false;
  }
}

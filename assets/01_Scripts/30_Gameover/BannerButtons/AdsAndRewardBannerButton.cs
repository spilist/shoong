using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class AdsAndRewardBannerButton : BannerButton {
  public Text goldenCubeText;

  public int goldenCubePerAds = 30;
  public int showAdsPerGame = 10;
  public int showAdsPerMinutes = 10;
  public int dailyLimit = 5;

	override public void activateSelf() {
    Debug.Log("Show ADs");

    // after ads end, show goldencube get effect

    DataManager.dm.increment("CurrentGoldenCubes", goldenCubePerAds);
    DataManager.dm.increment("TotalGoldenCubes", goldenCubePerAds);

    goldenCubeText.text = (int.Parse(goldenCubeText.text) + goldenCubePerAds).ToString();

    DataManager.dm.increment("DailyAdsCount");
    DataManager.dm.setInt("LastNumPlayAdsSeen", DataManager.dm.getInt("TotalNumPlays"));
    DataManager.dm.setDateTime("LastDateTimeAdsSeen");

    stopBlink();
    filter.sharedMesh = inactiveMesh;
  }

  override public bool available(int spaceLeft) {
    if (DataManager.dm.isAnotherDay("LastDateTimeAdsSeen")) {
      DataManager.dm.setInt("DailyAdsCount", 0);
    }

    if (DataManager.dm.getInt("DailyAdsCount") > dailyLimit) return false;

    int gamesPassed = DataManager.dm.getInt("TotalNumPlays") - DataManager.dm.getInt("LastNumPlayAdsSeen");
    int minutesPassed = DateTime.Now.Minute - DataManager.dm.getDateTime("LastDateTimeAdsSeen").Minute;

    if (gamesPassed >= showAdsPerGame || minutesPassed >= showAdsPerMinutes) {
      return true;
    }
    return false;
  }
}

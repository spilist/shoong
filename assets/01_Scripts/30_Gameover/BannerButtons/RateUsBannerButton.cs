using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RateUsBannerButton : BannerButton {
  public Text goldenCubeText;
  public int showPerGame = 3;
  public int dailyLimit = 3;
  public int goldenCubesGet = 30;

  override public void activateSelf() {
    DataManager.dm.setBool("IsGameRated", true);
    Application.OpenURL("http://unity3d.com/");

    // show effect
    DataManager.dm.increment("CurrentGoldenCubes", goldenCubesGet);
    DataManager.dm.increment("TotalGoldenCubes", goldenCubesGet);

    goldenCubeText.text = (int.Parse(goldenCubeText.text) + goldenCubesGet).ToString();

    DataManager.dm.setDateTime("LastDateTimeRateSeen");
  }

  override public bool available(int spaceLeft) {
    if (DataManager.dm.getBool("IsGameRated")) return false;

    if (DataManager.dm.isAnotherDay("LastDateTimeRateSeen")) {
      DataManager.dm.setInt("DailyRateCount", 0);
    }

    if (DataManager.dm.getInt("DailyRateCount") > dailyLimit) return false;

    int dividened = DataManager.dm.getInt("TotalNumPlays") / showPerGame;
    int mod = DataManager.dm.getInt("TotalNumPlays") % showPerGame;

    if (dividened > 0 && mod == 0) {
      DataManager.dm.increment("DailyRateCount");
      return true;
    }
    return false;
  }
}

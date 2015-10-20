using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FreeRewardBannerButton : BannerButton {
  public Text goldenCubeText;
  public int minReward = 10;
  public int maxReward = 50;

  public float[] nextRewardMinutes;

  private bool active = true;

  override public void activateSelf() {
    if (!active) return;

    // 골드큐브 연출
    int reward = UnityEngine.Random.Range(minReward, maxReward + 1);

    goldenCubeText.text = (int.Parse(goldenCubeText.text) + reward).ToString();

    DataManager.dm.increment("CurrentGoldenCubes", reward);
    DataManager.dm.increment("TotalGoldenCubes", reward);
    DataManager.dm.increment("FreeRewardCount");
    DataManager.dm.setDateTime("LastFreeRewardTime");

    // stopBlink();
    // filter.sharedMesh = inactiveMesh;
    active = false;
    playTouchSound = false;
  }

  override public bool available(int spaceLeft) {
    float minPassed = (float) (DateTime.Now - DataManager.dm.getDateTime("LastFreeRewardTime")).TotalMinutes;

    int rewardCount = DataManager.dm.getInt("FreeRewardCount") >= nextRewardMinutes.Length ? (nextRewardMinutes.Length - 1) : DataManager.dm.getInt("FreeRewardCount");

    return minPassed >= nextRewardMinutes[rewardCount];
  }
}

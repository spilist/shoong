using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FreeRewardBannerButton : BannerButton {
  public FreeGift freeGift;
  public int minReward = 70;
  public int maxReward = 140;
  private int reward;

  public int[] nextRewardMinutes;

  override public void activateSelf() {
    gameOverUI.SetActive(false);
    reward = UnityEngine.Random.Range(minReward, maxReward + 1);

    freeGift.run(this, reward);

    stopBlink();

    GetComponent<MeshRenderer>().enabled = false;
    GetComponent<Collider>().enabled = false;
  }

  public void endFreeGift() {
    gameOverUI.SetActive(true);
    gold.change(reward);
    DataManager.dm.increment("FreeRewardCount");
    DataManager.dm.setDateTime("LastFreeRewardTime");
    transform.parent.GetComponent<Text>().text = secondDescription.Replace("_REWARD_", reward.ToString());

    int rewardCount = DataManager.dm.getInt("FreeRewardCount") >= nextRewardMinutes.Length ? (nextRewardMinutes.Length - 1) : DataManager.dm.getInt("FreeRewardCount");

    if (DataManager.dm.getBool("NotificationSetting")) {
      NotificationManager.nm.notifyAfter(nextRewardMinutes[rewardCount]);
    }

    DataManager.dm.save();
  }

  override public bool available() {
    float minPassed = (float) (DateTime.Now - DataManager.dm.getDateTime("LastFreeRewardTime")).TotalMinutes;

    int rewardCount = DataManager.dm.getInt("FreeRewardCount") >= nextRewardMinutes.Length ? (nextRewardMinutes.Length - 1) : DataManager.dm.getInt("FreeRewardCount");

    return minPassed >= nextRewardMinutes[rewardCount];
  }
}

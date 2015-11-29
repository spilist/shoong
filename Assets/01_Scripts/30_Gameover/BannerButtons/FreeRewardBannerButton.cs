using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FreeRewardBannerButton : BannerButton {
  public FreeGift freeGift;
  public int minReward = 70;
  public int maxReward = 140;
  public GameObject timeIcon;
  private int reward;

  public int[] nextRewardMinutes;
  public bool first;

  override public void activateSelf() {
    gameOverUI.SetActive(false);
    reward = UnityEngine.Random.Range(minReward, maxReward + 1);

    freeGift.run(this, reward);
    stopBlink();

    // GetComponent<MeshRenderer>().enabled = false;
    filter.sharedMesh = inactiveMesh;
    GetComponent<Collider>().enabled = false;

    timeIcon.SetActive(true);
  }

  public void endFreeGift() {
    gameOverUI.SetActive(true);
    if (first) {
      FacebookManager.fb.firstPlayLog("6_FirstGift");
    } else {
      DataManager.dm.increment("FreeRewardCount");
    }

    int frCount = DataManager.dm.getInt("FreeRewardCount");
    int rewardCount = frCount >= nextRewardMinutes.Length ? (nextRewardMinutes.Length - 1) : frCount;
    int nextRewardMinute = nextRewardMinutes[rewardCount];
    int nextRewardHour = nextRewardMinute / 60;
    nextRewardMinute %= 60;
    string nextReward = nextRewardHour.ToString("0") + ":" + nextRewardMinute.ToString("00");

    DataManager.dm.setDateTime("LastFreeRewardTime");
    transform.parent.GetComponent<Text>().text = nextReward;
    // transform.parent.GetComponent<Text>().text = secondDescription.Replace("_REWARD_", reward.ToString());

    gold.change(reward);


    if (!DataManager.dm.getBool("NotificationSetting")) {
      NotificationManager.nm.notifyAfter();
    }
  }

  override public bool available() {
    float minPassed = (float) (DateTime.Now - DataManager.dm.getDateTime("LastFreeRewardTime")).TotalMinutes;

    int rewardCount = DataManager.dm.getInt("FreeRewardCount") >= nextRewardMinutes.Length ? (nextRewardMinutes.Length - 1) : DataManager.dm.getInt("FreeRewardCount");

    return minPassed >= nextRewardMinutes[rewardCount];
  }
}

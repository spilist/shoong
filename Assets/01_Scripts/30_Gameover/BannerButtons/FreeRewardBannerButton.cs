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
  private DateTime nextRewardTime;
  private DateTime lastRewardTime;
  private bool indicatingNextTime;

  override public void activateSelf() {
    gameOverUI.SetActive(false);
    reward = UnityEngine.Random.Range(minReward, maxReward + 1);

    freeGift.run(this, reward);
    stopBlink();

    // GetComponent<MeshRenderer>().enabled = false;
    filter.sharedMesh = inactiveMesh;
    GetComponent<Collider>().enabled = false;

    if (!first) {
      timeIcon.SetActive(true);
    }
  }

  public void endFreeGift() {
    gameOverUI.SetActive(true);
    if (first) {
      TrackingManager.tm.firstPlayLog("6_FirstGift");
      gold.change(reward);
      DataManager.dm.setBool("FirstGiftReceived", true);
    } else {
      int frCount = DataManager.dm.getInt("FreeRewardCount");
      DataManager.dm.increment("FreeRewardCount");
      int rewardCount = frCount >= nextRewardMinutes.Length ? (nextRewardMinutes.Length - 1) : frCount;
      int nextRewardMinute = nextRewardMinutes[rewardCount];
      nextRewardTime = DateTime.Now.AddMinutes(nextRewardMinute);

      DataManager.dm.setDateTime("LastFreeRewardTime");
      indicatingNextTime = true;

      gold.change(reward);


      if (!DataManager.dm.getBool("NotificationSetting") && !(rewardCount == 0)) {
        NotificationManager.nm.notifyAfter(nextRewardMinute);
      }
    }    
  }

  override protected void Update() {
    base.Update();
    if (indicatingNextTime) {
      transform.parent.GetComponent<Text>().text = "    " + timeUntilAvailable();
    }
  }

  string timeUntilAvailable() {
    TimeSpan interval = nextRewardTime - DateTime.Now;
    return interval.Hours.ToString("00") + ":" + interval.Minutes.ToString("00") + ":" + interval.Seconds.ToString("00");
  }

  override public bool available() {
    if (!DataManager.dm.getBool("FirstGiftReceived")) {
      if (first) return true;
      else return false;
    } else if (first) {
      return false;
    }
    float minPassed = (float) (DateTime.Now - DataManager.dm.getDateTime("LastFreeRewardTime")).TotalMinutes;

    int rewardCount = DataManager.dm.getInt("FreeRewardCount") >= nextRewardMinutes.Length ? (nextRewardMinutes.Length - 1) : DataManager.dm.getInt("FreeRewardCount");

    return minPassed >= nextRewardMinutes[rewardCount];
  }
}

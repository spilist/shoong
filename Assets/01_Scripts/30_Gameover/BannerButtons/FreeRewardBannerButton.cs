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
    nextRewardTime = DateTime.Now.AddMinutes(nextRewardMinute);

    DataManager.dm.setDateTime("LastFreeRewardTime");
    indicatingNextTime = true;

    gold.change(reward);


    if (!DataManager.dm.getBool("NotificationSetting")) {
      NotificationManager.nm.notifyAfter();
    }
  }

  override protected void Update() {
    base.Update();
    if (indicatingNextTime) {
      transform.parent.GetComponent<Text>().text = "   " + timeUntilAvailable();
    }
  }

  string timeUntilAvailable() {
    TimeSpan interval = nextRewardTime - DateTime.Now;
    return interval.Hours.ToString("00") + ":" + interval.Minutes.ToString("00") + ":" + interval.Seconds.ToString("00");
  }

  override public bool available() {
    float minPassed = (float) (DateTime.Now - DataManager.dm.getDateTime("LastFreeRewardTime")).TotalMinutes;

    int rewardCount = DataManager.dm.getInt("FreeRewardCount") >= nextRewardMinutes.Length ? (nextRewardMinutes.Length - 1) : DataManager.dm.getInt("FreeRewardCount");

    return minPassed >= nextRewardMinutes[rewardCount];
  }
}

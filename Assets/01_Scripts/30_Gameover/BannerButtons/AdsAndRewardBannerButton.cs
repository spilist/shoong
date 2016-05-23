using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
// using UnityEngine.Advertisements;
using Heyzap;

public class AdsAndRewardBannerButton : BannerButton {
  public Text goldenCubeText;
  public GameObject icon;
  public GameObject timeIcon;

  public int goldenCubePerAds = 30;
  public int dailyLimit = 5;
  private bool active = true;
  private bool indicatingNextTime = false;
  bool buttonDisabled = false;
  bool receivedReward = false;


  override protected void Update() {
    base.Update();
    if (gameObject.activeInHierarchy) {
      if (AdsManager.am.rewardAvailable()) {
        if (buttonDisabled && !receivedReward) {
          activateButton(true);
          timeIcon.SetActive(false);
          if (transform.parent.GetComponent<Text>() != null)
            transform.parent.GetComponent<Text>().text = description;
        }
      } else {
        if (!buttonDisabled) {
          activateButton(false);
        }
        if (!receivedReward) {
          TimeSpan interval = AdsManager.am.getRewardTimeLeft();
          string timeUntilAvailable = interval.Hours.ToString("00") + ":" + interval.Minutes.ToString("00") + ":" + interval.Seconds.ToString("00");
          timeIcon.SetActive(true);
          if (transform.parent.GetComponent<Text>() != null)
            transform.parent.GetComponent<Text>().text = "    " + timeUntilAvailable;
        }
      }
    }
  }

  void activateButton(bool activeVal) {
    if (activeVal) startBlink();
    else stopBlink();
    active = activeVal;
    buttonDisabled = !activeVal;
    playTouchSound = activeVal;
    GetComponent<Collider>().enabled = activeVal;
    filter.sharedMesh = (activeVal) ? activeMesh : inactiveMesh;

  }

  override public void activateSelf() {
    if (!active) return;
#if UNITY_EDITOR
		AdsManager.am.showedRewardAd();
		receivedReward = true;
		gold.change(goldenCubePerAds);
		return;
#endif
    if (HZIncentivizedAd.IsAvailable()) {
      HZIncentivizedAd.Show();

      HZIncentivizedAd.AdDisplayListener listener = delegate(string adState, string adTag){
        if ( adState.Equals("incentivized_result_complete") || adState.Equals("click") ) {
          // The user has watched the entire video and should be given a reward.
          AdsManager.am.showedRewardAd();
          receivedReward = true;
          gold.change(goldenCubePerAds);
          
          transform.parent.GetComponent<Text>().text = secondDescription.Replace("_REWARD_", goldenCubePerAds.ToString());
        }
      };

      HZIncentivizedAd.SetDisplayListener(listener);
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

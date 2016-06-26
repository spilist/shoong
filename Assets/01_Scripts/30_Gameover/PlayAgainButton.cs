using UnityEngine;
using System.Collections;

public class PlayAgainButton : MenusBehavior {
  public BeforeIdle beforeIdle;
  public BackButton backButton;
  public Transform stageGiftList;
  public GameObject touchBlocker;
  public bool first;

  bool isOpeningBox = false;
  bool resetBonus = true;

	override public void activateSelf() {
    if (isOpeningBox)
      return;
    if (DataManager.dm.isFirstPlay()) {
      TrackingManager.tm.firstPlayLog("8_FirstPlayAgainButtonClick");
    }
    if (!this.gameObject.activeInHierarchy) {
      backButton.activateSelf();
    }

    if (resetBonus) DataManager.dm.isBonusStage = false;

    StartCoroutine(openGiftAndPlayAgain());
    /*
    beforeIdle.playAgain();
    AudioManager.am.changeVolume("Main", "Min");
    */


    if (DataManager.dm.getBool("FirstPlayFinished")) {
      DataManager.dm.setBool("FirstPlay", false);
    }
  }

  public void stopResetBonus() {
    resetBonus = false;
  }

  IEnumerator openGiftAndPlayAgain() {
    isOpeningBox = true;
    touchBlocker.SetActive(true);
    bool openedBox = false;
    StageGift[] stageGifts = stageGiftList.GetChild(PhaseManager.pm.phase() / 3).GetComponentsInChildren<StageGift>();
    foreach (StageGift gift in stageGifts) {
      if (gift.isOpened) {
        continue;
      } else {
        gift.GetComponentInChildren<StageGiftBox>().activateSelf();
        openedBox = true;
        yield return new WaitForSeconds(0.5f);
      }
    }
    if (openedBox) {
      yield return new WaitForSeconds(1.0f);
    }
    touchBlocker.SetActive(false);
    beforeIdle.playAgain();
    AudioManager.am.changeVolume("Main", "Min");
  }
}

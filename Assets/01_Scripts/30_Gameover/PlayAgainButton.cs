using UnityEngine;
using System.Collections;

public class PlayAgainButton : MenusBehavior {
  public BeforeIdle beforeIdle;
  public BackButton backButton;
  public Transform stageGiftList;
  public GameObject touchBlocker;
  public bool first;
  bool isOpeningBox = false;

	override public void activateSelf() {
    if (isOpeningBox)
      return;
    if (DataManager.dm.isFirstPlay) {
      int firstPlayAgainCount = DataManager.dm.getInt("FirstPlayAgainCount") + 1;
      if (firstPlayAgainCount < 10) {
        TrackingManager.tm.firstPlayLog("8_PlayAgain_" + firstPlayAgainCount);
        DataManager.dm.increment("FirstPlayAgainCount");
      }
    }
    if (!this.gameObject.activeInHierarchy) {
      backButton.activateSelf();
    }
    StartCoroutine(openGiftAndPlayAgain());
    /*
    beforeIdle.playAgain();
    AudioManager.am.changeVolume("Main", "Min");
    */
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

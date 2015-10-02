using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverBanner : MonoBehaviour {
  private ScoreManager scoreManager;
  public int expandTo = 75;
  public float expandDuration = 0.2f;
  public float moveDuration = 0.2f;
  public int positionXEnd = -165;

  private Transform overlay;
  private GameObject contents;

  private float scaleY = 1;
  private float positionX;
  private float distance;
  private int status = 0;
  private bool waiting = false;
  private GameOverBanner waitingTarget;
  private bool isLast = true;

  void Update() {
    if (waiting) {
      if (waitingTarget.moveDone()) {
        waiting = false;
        status++;
      }
    } else {
      if (status == 1) {
        scaleY = Mathf.MoveTowards(scaleY, expandTo, Time.deltaTime * expandTo / expandDuration);
        overlay.localScale = new Vector3(overlay.localScale.x, scaleY, 1);
        if (scaleY == expandTo) {
          status++;
        }
      } else if (status == 2) {
        positionX = Mathf.MoveTowards(positionX, positionXEnd, Time.deltaTime / moveDuration * distance);
        contents.GetComponent<RectTransform>().anchoredPosition = new Vector2(positionX, contents.GetComponent<RectTransform>().anchoredPosition.y);

        if (positionX == positionXEnd) {
          status++;
          if (isLast) scoreManager.bannerEnd();
        }
      }
    }

  }

  public void show(BannerButton bannerButton, GameObject another = null) {

    overlay = transform.Find("Overlay");
    contents = transform.Find("Contents").gameObject;
    contents.GetComponent<Text>().text = bannerButton.description;
    positionX = contents.GetComponent<RectTransform>().anchoredPosition.x;
    distance = Mathf.Abs(positionX - positionXEnd);

    if (another == null) {
      status++;
      // isLast = false;
      isLast = true;
    } else {
      waiting = true;
      waitingTarget = another.GetComponent<GameOverBanner>();
    }

    bannerButton.transform.SetParent(contents.transform, false);
    scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
  }

  public bool moveDone() {
    return status == 3;
  }
}

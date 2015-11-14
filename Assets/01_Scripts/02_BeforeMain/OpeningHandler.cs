using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpeningHandler : MonoBehaviour {
  public GameObject idleUI;
  public GameObject dreamingText;
  public GameObject tutorialHandler;
  public float cameraMoveTo = 200;
  public float movingIn = 0.4f;
  public float stayBeforeMove = 1;
  private float stayCount;
  private bool movingDown = false;
  private float cameraZ;

  public Text copyright;
  private Color copyrightColor;
  public float copyrightShowAfter = 0.3f;
  public float copyrightAlphaChangeDuration = 0.2f;
  private float copyrightHideCount = 0;

  public GameObject title;
  public float movingDuration = 0.6f;
  private bool titleMoving = false;
  private float titlePosX;
  private float distance;

	void Start () {
    cameraZ = 0;

    titlePosX = title.GetComponent<RectTransform>().anchoredPosition.x;
    copyrightColor = new Color(1, 1, 1, 0);
    distance = Mathf.Abs(titlePosX);
	}

  public void moveDown() {
    if (!movingDown) movingDown = true;
  }

  void Update() {
    if (movingDown) {
      if (stayCount < stayBeforeMove) stayCount += Time.deltaTime;
      else {
        dreamingText.SetActive(false);
        cameraZ = Mathf.MoveTowards(cameraZ, cameraMoveTo, Time.deltaTime * cameraMoveTo / movingIn);
        Player.pl.transform.position = new Vector3(Player.pl.transform.position.x, Player.pl.transform.position.y, cameraZ);
        if (cameraZ == cameraMoveTo) {
          movingDown = false;
          titleMoving = true;
        }
      }
    }

    if (titleMoving) {
      titlePosX = Mathf.MoveTowards(titlePosX, 0, Time.deltaTime / movingDuration * distance);
      title.GetComponent<RectTransform>().anchoredPosition = new Vector2(titlePosX, title.GetComponent<RectTransform>().anchoredPosition.y);

      if (copyrightHideCount < copyrightShowAfter) {
        copyrightHideCount += Time.deltaTime;
      } else {
        copyrightColor.a = Mathf.MoveTowards(copyrightColor.a, 1, Time.deltaTime / copyrightAlphaChangeDuration);
        copyright.color = copyrightColor;
      }

      if (titlePosX == 0) {
        titleMoving = false;
        idleUI.SetActive(true);
        tutorialHandler.SetActive(true);
        gameObject.SetActive(false);
      }
    }
  }
}

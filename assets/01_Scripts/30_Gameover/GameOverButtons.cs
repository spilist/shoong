using UnityEngine;
using System.Collections;

public class GameOverButtons : MonoBehaviour {
  public ScoreManager scoreManager;
  public float moveDuration = 0.2f;
  private int status = 0;
  private float positionY;
  private float distance;

	void OnEnable() {
    positionY = GetComponent<RectTransform>().anchoredPosition.y;
    distance = Mathf.Abs(positionY);
    status++;
  }

	void Update () {
    if (status == 1) {
      positionY = Mathf.MoveTowards(positionY, 0, Time.deltaTime / moveDuration * distance);
      GetComponent<RectTransform>().anchoredPosition = new Vector2(0, positionY);

      if (positionY == 0) {
        status++;
        scoreManager.setButtonsAvailable();
      }
    }
	}
}

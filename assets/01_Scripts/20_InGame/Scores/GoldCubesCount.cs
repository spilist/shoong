using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldCubesCount : MonoBehaviour {
  public ParticleSystem getEffect;
  public Text effectAmountText;

  public float moveTo;
  public float moveDuration = 0.5f;
  public float showDuration = 2;
  public int offset = 30;

  private float moveBackTo;
  private Text cubes;
  private int count;
  private RectTransform tr;
  private float positionX;
  private float showCount = 0;
  private int moveStatus = 0;

	void OnEnable() {
    cubes = GetComponent<Text>();
    count = DataManager.dm.getInt("CurrentGoldenCubes");
    cubes.text = count.ToString();

    tr = GetComponent<RectTransform>();
    positionX = cubes.preferredWidth + offset;
    tr.anchoredPosition += new Vector2(positionX, 0);
  }

  public void add(int amount = 1, bool withEffect = true) {
    count += amount;
    cubes.text = count.ToString();

    DataManager.dm.increment("CurrentGoldenCubes", amount);
    DataManager.dm.increment("TotalGoldenCubes", amount);

    moveBackTo = cubes.preferredWidth + offset;
    showCount = 0;
    moveStatus = 1;
    GetComponent<AudioSource>().Play();

    Player.pl.showEffect("GoldenCube");
    if (withEffect) getEffect.Play();
    effectAmountText.text = amount.ToString();
  }

  public int getCount() {
    return count;
  }

  void Update() {
    if (moveStatus == 1) {
      positionX = Mathf.MoveTowards(positionX, moveTo, Time.deltaTime / moveDuration * (moveBackTo - moveTo));
      tr.anchoredPosition = new Vector2(positionX, tr.anchoredPosition.y);
      if (positionX == moveTo) moveStatus = 2;
    } else if (moveStatus == 2) {
      if (showCount < showDuration) {
        showCount += Time.deltaTime;
      } else {
        moveStatus = 3;
      }
    } else if (moveStatus == 3) {
      positionX = Mathf.MoveTowards(positionX, moveBackTo, Time.deltaTime / moveDuration * (moveBackTo - moveTo));
      tr.anchoredPosition = new Vector2(positionX, tr.anchoredPosition.y);
      if (positionX == moveBackTo) moveStatus = 4;
    }
  }
}

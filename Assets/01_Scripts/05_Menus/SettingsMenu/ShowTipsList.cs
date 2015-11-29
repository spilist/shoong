using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowTipsList : VerticalScrollingMenu {
  public Transform tipsTransform;
  public GameObject tipPrefab;

  public int spaceBetween = 15;

	override protected void initRest () {
    int count = 0;
    float accumulatedHeight = 0;
    foreach (Transform tr in tipsTransform) {
      GameObject tip = (GameObject) Instantiate(tipPrefab);
      tip.transform.SetParent(scrollTarget.transform, false);

      tip.GetComponent<Text>().text = (count + 1).ToString() + ". " + tr.GetComponent<Tip>().description;
      tip.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, 300);

      float preferredHeight = tip.GetComponent<Text>().preferredHeight;
      tip.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, - accumulatedHeight);

      accumulatedHeight += preferredHeight + spaceBetween;
      count++;
    }

    scrollTarget.sizeDelta = new Vector2(newWidth, accumulatedHeight);
	}
}

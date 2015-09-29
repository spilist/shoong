using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowTipsList : MonoBehaviour {
  public Transform tipsTransform;
  public GameObject tipPrefab;
  public float widthOffset;
  public float heightOffset;

  public int spaceBetween = 15;

	void Start () {
    float screenWidth = transform.parent.GetComponent<RectTransform>().rect.width;
    float screenHeight = transform.parent.GetComponent<RectTransform>().rect.height;

    float newWidth = screenWidth - 2 * widthOffset;
    float newHeight = screenHeight - 2 * heightOffset;

    GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, newHeight);
    GetComponent<RectTransform>().anchoredPosition = new Vector2(widthOffset, - heightOffset);

    Transform tipsList = transform.Find("TipsList");
    int count = 0;
    float accumulatedHeight = 0;
    foreach (Transform tr in tipsTransform) {
      GameObject tip = (GameObject) Instantiate(tipPrefab);
      tip.transform.SetParent(tipsList, false);

      tip.GetComponent<Text>().text = (count + 1).ToString() + ". " + tr.GetComponent<Tip>().description;
      tip.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, 300);

      float preferredHeight = tip.GetComponent<Text>().preferredHeight;
      tip.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, - (accumulatedHeight + count * spaceBetween));

      accumulatedHeight += preferredHeight;
      count++;
    }

    tipsList.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, accumulatedHeight + count * spaceBetween);
	}
}

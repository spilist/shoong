using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyButtonsCubeIconPosition : MonoBehaviour {
  public float startPosX = 0;
  public float offset = 1;
  public int sign = -1;
  public float scale = 5;

  public void adjust(Text price) {
    float priceWidth = price.preferredWidth;
    float posX = sign * (priceWidth / 2 / scale + offset);
    GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosX + posX, GetComponent<RectTransform>().anchoredPosition.y);
  }
}

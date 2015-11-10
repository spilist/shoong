using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VerticalScrollingMenu : MonoBehaviour {
  public float widthOffset = 110;
  public float heightOffset = 110;
  public RectTransform scrollTarget;

  protected float newWidth;
  protected float newHeight;

	void Start () {
    float screenWidth = transform.parent.GetComponent<RectTransform>().rect.width;
    float screenHeight = transform.parent.GetComponent<RectTransform>().rect.height;

    newWidth = screenWidth - 2 * widthOffset;
    newHeight = screenHeight - 2 * heightOffset;

    GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, newHeight);
    GetComponent<RectTransform>().anchoredPosition = new Vector2(widthOffset, - heightOffset);

    initRest();
	}

	virtual protected void initRest() {}
}

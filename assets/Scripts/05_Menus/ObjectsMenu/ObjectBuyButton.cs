using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectBuyButton : MenusBehavior {
  public GoldenCubesYouHave goldenCubes;
  public Material notAffordableCubeMat;
  public Material originalMat;
  public Color notAffordableTextColor;
  public Renderer goldenCubeIconRenderer;
  public Text priceText;

  private UIObjects selectedObj;
  private string objectName;
  private int price;
  private bool affordable = true;

	void Start () {
    playTouchSound = false;
	}

	public void setObject(UIObjects obj) {
    gameObject.SetActive(true);
    selectedObj = obj;
    objectName = obj.transform.parent.name;
    priceText.text = obj.price.ToString("N0");

    if (goldenCubes.youHave() < obj.price) {
      affordable = false;
      goldenCubeIconRenderer.sharedMaterial = notAffordableCubeMat;
      priceText.color = notAffordableTextColor;
    } else {
      affordable = true;
      goldenCubeIconRenderer.sharedMaterial = originalMat;
      priceText.color = new Color(255, 255, 255);
    }
  }

  override public void activateSelf() {
    if (!affordable) return;

    goldenCubes.buy(selectedObj.price);
    GameController.control.objects[objectName] = true;
    selectedObj.buy();
  }
}

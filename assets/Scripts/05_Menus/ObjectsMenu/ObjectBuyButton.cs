using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectBuyButton : MenusBehavior {
  public string which;
  public CubesYouHave cubes;
  public Material notAffordableCubeMat;
  public Material originalMat;
  public Color notAffordableTextColor;
  public Renderer goldenCubeIconRenderer;
  public Text priceText;

  private UIObjects selectedObj;
  private int price;
  private string objectName;
  private bool affordable = true;

	void Start () {
    playTouchSound = false;
	}

	public void setObject(UIObjects obj) {
    gameObject.SetActive(true);
    selectedObj = obj;
    objectName = obj.transform.parent.name;

    price = obj.getPrice(which);
    priceText.text = price.ToString("N0");

    if (cubes.youHave() < price) {
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

    cubes.buy(price);
    GameController.control.objects[objectName] = true;
    selectedObj.buy();
  }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectBuyButton : MenusBehavior {
  public string which;
  public CubesYouHave cubes;
  public Color notAffordableTextColor;
  public Text priceText;
  public Mesh originalMesh;
  public Mesh blinkingMesh;
  public float blinkingSeconds = 0.4f;
  public BuyButtonsCubeIconPosition icon;

  private UIObjects selectedObj;
  private int price;
  private string objectName;
  private bool affordable = true;

	void Start () {
    playTouchSound = false;
	}

  IEnumerator blinkButton() {
    while(true) {
      GetComponent<MeshFilter>().sharedMesh = originalMesh;

      yield return new WaitForSeconds(blinkingSeconds);

      GetComponent<MeshFilter>().sharedMesh = blinkingMesh;

      yield return new WaitForSeconds(blinkingSeconds);
    }
  }

	public void setObject(UIObjects obj) {
    gameObject.SetActive(true);
    selectedObj = obj;
    objectName = obj.transform.parent.name;

    price = obj.getPrice(which);
    priceText.text = price.ToString("N0");
    icon.adjust(priceText);

    if (cubes.youHave() < price) {
      affordable = false;
      priceText.color = notAffordableTextColor;
      StopCoroutine("blinkButton");
      if (which == "normal") {
        GetComponent<MeshFilter>().sharedMesh = originalMesh;
      }
    } else {
      affordable = true;
      priceText.color = new Color(255, 255, 255);
      if (which == "normal") {
        StopCoroutine("blinkButton");
        StartCoroutine("blinkButton");
      }
    }
  }

  override public void activateSelf() {
    if (!affordable) return;

    if (DataManager.dm.getInt(objectName + "Level") == 0) {
      DataManager.dm.setBool(objectName, true);
      string tutorialsNotDone = DataManager.dm.getString("ObjectTutorialsNotDone");
      DataManager.dm.setString("ObjectTutorialsNotDone", (tutorialsNotDone + " " + objectName).Trim());
    }
    DataManager.dm.increment(objectName + "Level");

    cubes.buy(price);
    selectedObj.buy();
  }
}

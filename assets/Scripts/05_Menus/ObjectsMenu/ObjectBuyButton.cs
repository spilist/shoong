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

    cubes.buy(price);
    GameController.control.objects[objectName] = true;
    selectedObj.buy();

    string tutorialsNotDone = PlayerPrefs.GetString("ObjTutorialsNotDone");
    PlayerPrefs.SetString("ObjTutorialsNotDone", (tutorialsNotDone + " " + objectName).Trim());
  }
}

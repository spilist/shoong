using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectsMenu : MonoBehaviour {
  public Material inactiveObjectsMaterial;
  public AudioClip objectActiveSound;
  public AudioClip objectBuySound;
  public Color activeColor;
  public Color inactiveColor;
  public ObjectDetail objDetail;
  public GameObject emptyDescription;

  public GameObject cubeYouHave;
  public GameObject goldenCubeYouHave;
  public float rotationSpeed = 150;

  private string[] mainObjects;
  private string[] subObjects;

	void OnEnable() {
    cubeYouHave.SetActive(true);
    goldenCubeYouHave.SetActive(true);

    transform.Find("MainObjectsButton").GetComponent<MenusBehavior>().activateSelf();
    showEmptyDescription("MainObjects");

    float screenWidth = transform.parent.GetComponent<RectTransform>().rect.width;
    float screenHeight = transform.parent.GetComponent<RectTransform>().rect.height;
    GetComponent<RectTransform>().sizeDelta = new Vector2(screenWidth, screenHeight);
  }

  void OnDisable() {
    cubeYouHave.SetActive(false);
    goldenCubeYouHave.SetActive(false);
    resetAll("MainObjects");
  }

  public void resetAll(string what) {
    foreach (Transform tr in transform) {
      if (tr.tag == "ObjectsMenu" && tr.name != (what + "Button")) {
        tr.Find("Text").GetComponent<Text>().color = inactiveColor;
        if (tr.Find("SelectionCount") != null) tr.Find("SelectionCount").GetComponent<Text>().color = inactiveColor;
        if (tr.Find("SelectionLimit") != null) tr.Find("SelectionLimit").GetComponent<Text>().color = inactiveColor;
      }

      if (tr.tag == "ObjectsMenu_objects") {
        if (tr.name == what) tr.gameObject.SetActive(true);
        else tr.gameObject.SetActive(false);
      }
    }

    foreach (Transform tr in transform.Find(what)) {
      if (tr.tag == "UIObjects") {
        tr.Find("SelectionBox").GetComponent<Renderer>().enabled = false;
      }
    }

    showEmptyDescription(what);
  }

  public void showEmptyDescription(string what) {
    objDetail.gameObject.SetActive(false);
    emptyDescription.SetActive(true);
    foreach (Transform tr in emptyDescription.transform) {
      if (tr.name == what) tr.gameObject.SetActive(true);
      else tr.gameObject.SetActive(false);
    }

    if (what == "CubeCollector") {
      emptyDescription.transform.Find("CubeCollectorPercent").gameObject.SetActive(true);

      int normalCollectorLevel = DataManager.dm.getInt("NormalCollectorLevel") - 1;
      int bonusAmount = normalCollectorLevel * 5 + DataManager.dm.getInt("GoldenCollectorLevel") * 50;
      emptyDescription.transform.Find("CubeCollectorPercent").GetComponent<Text>().text = "+" + bonusAmount.ToString();
    }
  }
}

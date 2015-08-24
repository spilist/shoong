using UnityEngine;
using System.Collections;

public class ObjectsMenu : MonoBehaviour {
  public AudioClip objectActiveSound;
  public AudioClip objectBuySound;
  public Color activeColor;
  public Color inactiveColor;
  public ObjectDetail objDetail;
  public GameObject emptyDescription;

  public GameObject cubeYouHave;
  public GameObject goldenCubeYouHave;

  private string[] mainObjects;
  private string[] subObjects;

	void OnEnable() {
    cubeYouHave.SetActive(true);
    goldenCubeYouHave.SetActive(true);

    transform.Find("MainObjectsButton").GetComponent<MenusBehavior>().activateSelf();
    showEmptyDescription("MainObjects");
  }

  void OnDisable() {
    cubeYouHave.SetActive(false);
    goldenCubeYouHave.SetActive(false);
    resetAll("MainObjects");
  }

  public void resetAll(string what) {
    foreach (Transform tr in transform.Find(what)) {
      if (tr.tag == "UIObjects") {
        tr.Find("SelectionBox").GetComponent<Renderer>().enabled = false;
      }
    }
  }

  public void showEmptyDescription(string what) {
    objDetail.gameObject.SetActive(false);
    emptyDescription.SetActive(true);
    foreach (Transform tr in emptyDescription.transform) {
      if (tr.name == what) tr.gameObject.SetActive(true);
      else tr.gameObject.SetActive(false);
    }
  }
}

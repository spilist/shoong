using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectDetail : MonoBehaviour {
  public float selectedObjectRotationSpeed = 150;
  public GameObject selectButton;
  public GameObject unselectButton;
  public GameObject buyButton;

  private GameObject selected;
  private bool rotate = false;

	void Update () {
    if (rotate) {
      selected.transform.Rotate(-Vector3.forward * Time.deltaTime * selectedObjectRotationSpeed, Space.World);
    }
	}

  public void selectObject(UIObjects obj) {
    gameObject.SetActive(true);
    transform.parent.Find("EmptyDescription").gameObject.SetActive(false);

    transform.Find("Name").GetComponent<Text>().text = obj.objectName;
    transform.Find("Description").GetComponent<Text>().text = obj.description;

    if (selected != null) selected.SetActive(false);
    selected = transform.Find("Objects/" + obj.transform.parent.name).gameObject;
    selected.SetActive(true);

    checkBought(obj);
  }

  public void checkBought(UIObjects obj) {
    if ((bool)GameController.control.objects[selected.name]) {
      rotate = true;
      selected.transform.Find("Effect").gameObject.SetActive(true);
      if (obj.isActive()) {
        selectButton.SetActive(false);
        unselectButton.GetComponent<ObjectUnselectButton>().setObject(obj);
        buyButton.SetActive(false);
      } else {
        selectButton.GetComponent<ObjectSelectButton>().setObject(obj);
        unselectButton.SetActive(false);
        buyButton.SetActive(false);
      }
    } else {
      // gray color material needed
      rotate = false;
      selected.transform.Find("Effect").gameObject.SetActive(false);
      selectButton.SetActive(false);
      unselectButton.SetActive(false);
      buyButton.GetComponent<ObjectBuyButton>().setObject(obj);
    }
  }

  void OnDisable() {
    rotate = false;
    selectButton.SetActive(false);
    unselectButton.SetActive(false);
    buyButton.SetActive(false);
  }
}

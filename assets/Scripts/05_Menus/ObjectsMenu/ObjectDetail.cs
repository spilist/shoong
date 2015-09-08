using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectDetail : MonoBehaviour {
  public Material activeObjectsMaterial;
  public Material inactiveObjectsMaterial;

  public float selectedObjectRotationSpeed = 150;
  public GameObject selectButton;
  public GameObject unselectButton;
  public GameObject buyButtonByCube;
  public GameObject buyButtonByGoldencube;

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
    if (DataManager.dm.getBool(selected.name)) {
      rotate = true;
      SmallObjects smallObject = obj.transform.parent.Find("Object").GetComponent<SmallObjects>();
      smallObject.checkBought();
      selected.GetComponent<MeshFilter>().sharedMesh = smallObject.activeMesh;
      selected.GetComponent<Renderer>().sharedMaterial = smallObject.activeMaterial;

      selected.transform.Find("Effect").gameObject.SetActive(true);
      if (obj.isActive()) {
        selectButton.SetActive(false);
        unselectButton.GetComponent<ObjectUnselectButton>().setObject(obj);
        buyButtonByCube.SetActive(false);
        buyButtonByGoldencube.SetActive(false);
      } else {
        selectButton.GetComponent<ObjectSelectButton>().setObject(obj);
        unselectButton.SetActive(false);
        buyButtonByCube.SetActive(false);
        buyButtonByGoldencube.SetActive(false);
      }
    } else {
      rotate = false;
      selectButton.SetActive(false);
      unselectButton.SetActive(false);
      buyButtonByCube.GetComponent<ObjectBuyButton>().setObject(obj);
      buyButtonByGoldencube.GetComponent<ObjectBuyButton>().setObject(obj);
    }
  }

  void OnDisable() {
    rotate = false;
    selectButton.SetActive(false);
    unselectButton.SetActive(false);
    buyButtonByCube.SetActive(false);
    buyButtonByGoldencube.SetActive(false);
  }
}

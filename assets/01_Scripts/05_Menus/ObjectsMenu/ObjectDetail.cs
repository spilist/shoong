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
  public Text objLevel;
  public GameObject upgradeInfo;

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
    int level = DataManager.dm.getInt(selected.name + "Level");
    objLevel.text = "LV " + level.ToString();

    Text upgradeLevel = upgradeInfo.transform.Find("Level").GetComponent<Text>();
    Text upgradeLabel = upgradeInfo.transform.Find("Label").GetComponent<Text>();

    if (level == 0) {
      rotate = false;
      objLevel.gameObject.SetActive(false);
      upgradeInfo.SetActive(false);
      selectButton.SetActive(false);
      unselectButton.SetActive(false);
    } else {
      rotate = true;
      objLevel.gameObject.SetActive(true);
      upgradeInfo.SetActive(true);
      upgradeInfo.transform.Find("Description").GetComponent<Text>().text = obj.upgradeEffect;

      SmallObjects smallObject = obj.transform.parent.Find("Object").GetComponent<SmallObjects>();
      smallObject.checkBought();
      selected.GetComponent<MeshFilter>().sharedMesh = smallObject.activeMesh;
      selected.GetComponent<Renderer>().sharedMaterial = smallObject.activeMaterial;


      selectButton.SetActive(true);
      unselectButton.SetActive(true);

      if (obj.isActive()) {
        selectButton.SetActive(false);
        selected.transform.Find("Effect").gameObject.SetActive(true);
        unselectButton.GetComponent<ObjectUnselectButton>().setObject(obj);
      } else {
        selectButton.GetComponent<ObjectSelectButton>().setObject(obj);
        selected.transform.Find("Effect").gameObject.SetActive(false);
        unselectButton.SetActive(false);
      }
    }

    if (level >= 3) {
      upgradeLevel.text = "MAX";
      upgradeLabel.text = "upgrade end";

      buyButtonByCube.SetActive(false);
      buyButtonByGoldencube.SetActive(false);
    } else {
      upgradeLevel.text = "LV " + (level + 1).ToString();
      upgradeLabel.text = "upgrade";

      buyButtonByCube.SetActive(true);
      buyButtonByGoldencube.SetActive(true);
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

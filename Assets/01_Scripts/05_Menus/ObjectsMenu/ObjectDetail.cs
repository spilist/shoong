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
  public int oneButtonOnlyPosition = 150;
  public int cubeOriginalPosition = 62;
  public int goldOriginalPosition = 245;

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
    objLevel.transform.Find("Percent").gameObject.SetActive(false);

    Text upgradeLevel = upgradeInfo.transform.Find("Level").GetComponent<Text>();
    Text upgradeLabel = upgradeInfo.transform.Find("Label").GetComponent<Text>();

    if (level == 0) {
      rotate = false;
      objLevel.gameObject.SetActive(false);
      upgradeInfo.SetActive(false);
      selectButton.SetActive(false);
      unselectButton.SetActive(false);
    } else {
      objLevel.gameObject.SetActive(true);
      upgradeInfo.SetActive(true);
      upgradeInfo.transform.Find("Description").GetComponent<Text>().text = obj.upgradeEffect;

      SmallObjects smallObject = obj.transform.parent.Find("Object").GetComponent<SmallObjects>();
      smallObject.checkBought();
      smallObject.changeDetail(selected);

      if (!obj.isCollector) {
        selectButton.SetActive(true);
        unselectButton.SetActive(true);
      }

      if (obj.isActive()) {
        rotate = true;
        selectButton.SetActive(false);
        if (selected.transform.Find("Effect") != null) selected.transform.Find("Effect").gameObject.SetActive(true);
        unselectButton.GetComponent<ObjectUnselectButton>().setObject(obj);
      } else {
        rotate = false;
        selectButton.GetComponent<ObjectSelectButton>().setObject(obj);
        if (selected.transform.Find("Effect") != null) selected.transform.Find("Effect").gameObject.SetActive(false);
        unselectButton.SetActive(false);
      }
    }

    if (level >= obj.maxLevel) {
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

    if (obj.isCollector) {
      rotate = true;
    }

    if (obj.isGoldOnly) {
      buyButtonByCube.SetActive(false);
      buyButtonByGoldencube.GetComponent<RectTransform>().anchoredPosition = new Vector2(oneButtonOnlyPosition, buyButtonByGoldencube.GetComponent<RectTransform>().anchoredPosition.y);

      if (level > 0) {
        objLevel.text = "cube\n+50%%";
        objLevel.transform.Find("Percent").gameObject.SetActive(true);
        buyButtonByGoldencube.SetActive(false);
        upgradeInfo.SetActive(false);
      }
    } else if (obj.isCubeOnly) {
      buyButtonByGoldencube.SetActive(false);
      buyButtonByCube.GetComponent<RectTransform>().anchoredPosition = new Vector2(oneButtonOnlyPosition, buyButtonByCube.GetComponent<RectTransform>().anchoredPosition.y);
      objLevel.text = "LV " + (level - 1) + "\n+" + (level - 1) * 5 + "%%";
      objLevel.transform.Find("Percent").gameObject.SetActive(true);
      upgradeLevel.text = "LV " + level.ToString();
    } else {
      buyButtonByCube.GetComponent<RectTransform>().anchoredPosition = new Vector2(cubeOriginalPosition, buyButtonByCube.GetComponent<RectTransform>().anchoredPosition.y);
      buyButtonByGoldencube.GetComponent<RectTransform>().anchoredPosition = new Vector2(goldOriginalPosition, buyButtonByGoldencube.GetComponent<RectTransform>().anchoredPosition.y);
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

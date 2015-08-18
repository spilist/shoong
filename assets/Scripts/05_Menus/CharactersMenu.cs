using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharactersMenu : Draggable {
  public Material inactiveCharactersMaterial;
  public float selectedCharacterRotationSpeed = 30;
  public GameObject cubeYouHave;
  public GameObject goldenCubeYouHave;
  public CharacterSelectButton selectButton;
  public CharacterBuyButton buyButton;
  public Text characterName;
  public int selectWidth = 50;
  public int selectedOffset_y = 15;
  public int selectedOffset_z = 50;
  public bool openAll = false;

  void OnEnable() {
    cubeYouHave.SetActive(true);
    goldenCubeYouHave.SetActive(true);

    Vector3 prevSelected = transform.Find("Characters/" + PlayerPrefs.GetString("SelectedCharacter")).transform.localPosition;
    transform.Find("Characters").transform.localPosition = new Vector3(prevSelected.x, 0, 0);

    if (openAll) {
      Hashtable table = new Hashtable();
      foreach (DictionaryEntry pair in GameController.control.characters) {
        table.Add(pair.Key, true);
      }
      GameController.control.characters = table;
    } else {
      Hashtable table = new Hashtable();
      foreach (DictionaryEntry pair in GameController.control.characters) {
        table.Add(pair.Key, false);
      }
      GameController.control.characters = table;
      GameController.control.characters["robotcogi"] = true;
    }

    int count = 0;
    foreach (Transform character in transform.Find("Characters").transform) {
      character.transform.localPosition = new Vector3(-selectWidth * 2 * count++, 0, 0);
    }
  }

  override public float leftDragEnd() {
    Transform firstChild = transform.Find("Characters").transform.GetChild(0);
    return firstChild.localPosition.x;
  }

  override public float rightDragEnd() {
    Transform lastChild = transform.Find("Characters").transform.GetChild(transform.Find("Characters").transform.childCount - 1);
    return lastChild.localPosition.x;
  }

  void OnDisable() {
    cubeYouHave.SetActive(false);
    goldenCubeYouHave.SetActive(false);
  }
}

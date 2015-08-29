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
  public int scaleChangingSpeed = 50;
  public Text numYourCharacters;
  public Text numAllCharacters;

  public AudioClip characterSelectionSound;
  public AudioClip characterBuySound;
  private bool justOpened = true;

  void OnEnable() {
    cubeYouHave.SetActive(true);
    goldenCubeYouHave.SetActive(true);

    int charactersCount = 0;
    int yourCharactersCount = 0;
    foreach (Transform character in transform.Find("Characters").transform) {
      character.transform.localPosition = new Vector3(-selectWidth * 2 * charactersCount++, 0, 0);

      if ((bool)GameController.control.characters[character.name]) yourCharactersCount++;
    }

    numYourCharacters.text = yourCharactersCount.ToString();
    numAllCharacters.text = "/" + charactersCount.ToString();

    Vector3 prevSelected = transform.Find("Characters/" + PlayerPrefs.GetString("SelectedCharacter")).transform.localPosition;
    transform.Find("Characters").transform.localPosition = new Vector3(prevSelected.x, -20, 0);
  }

  public bool isJustOpened() {
    return justOpened;
  }

  public void setOpened() {
    justOpened = false;
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

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharactersMenu : Draggable {
  public Material activeCharactersMaterial;
  public float selectedCharacterRotationSpeed = 30;
  public CharacterSelectButton selectButton;
  public CharacterBuyButton buyButton;
  public Text characterName;
  public Text rarity;
  public Text description;
  public Color[] colorsPerRarity;
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
    int charactersCount = 0;
    int yourCharactersCount = 0;
    foreach (Transform character in transform.Find("Characters").transform) {
      character.transform.localPosition = new Vector3(-selectWidth * 2 * charactersCount++, 0, 0);

      if (DataManager.dm.getBool(character.name)) {
        yourCharactersCount++;
      }
    }

    numYourCharacters.text = yourCharactersCount.ToString();
    numAllCharacters.text = "/" + charactersCount.ToString();

    Vector3 prevSelected = transform.Find("Characters/" + PlayerPrefs.GetString("SelectedCharacter")).transform.localPosition;
    transform.Find("Characters").transform.localPosition = new Vector3(prevSelected.x, 9, 0);

    GetComponent<RectTransform>().sizeDelta = transform.parent.GetComponent<RectTransform>().sizeDelta;
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

  public void buyComplete(string transactionId, string name, bool bought) {
    string characterName = name.Replace("toy_", "");

    if (bought) {
      selectButton.gameObject.SetActive(true);
      selectButton.setCharacter(characterName);
      buyButton.gameObject.SetActive(false);
      numYourCharacters.text = (int.Parse(numYourCharacters.text) + 1).ToString();
    }

    transform.Find("Characters/" + characterName).GetComponent<UICharacters>().buyComplete(transactionId, bought);
  }

  public void allCharacters() {
    Transform characters = transform.Find("Characters");
    if (DataManager.dm.getInt("NumCharactersHave") < characters.childCount) {
      foreach (Transform tr in characters) {
        DataManager.dm.setBool(tr.name, true);
        DataManager.dm.increment("NumCharactersHave");
      }
    } else {
      foreach (Transform tr in characters) {
        if (tr.name == "robotcogi") continue;
        DataManager.dm.setBool(tr.name, false);
      }
      DataManager.dm.setInt("NumCharactersHave", 1);
    }
    DataManager.dm.save();
  }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICharacters : MenusBehavior {
  private CharactersMenu charactersMenu;
  private bool selected = false;
  private Text characterName;
  private Color originalTextColor;
  private Material originalMaterial;
  private Quaternion originalRotation;
  private bool bought;

  void Start () {
    charactersMenu = GameObject.Find("CharactersMenu").GetComponent<CharactersMenu>();
    characterName = charactersMenu.transform.Find("Names/"+name).gameObject.GetComponent<Text>();
    originalTextColor = characterName.color;
    originalMaterial = GetComponent<Renderer>().sharedMaterial;
    originalRotation = transform.localRotation;

    bought = (bool)GameController.control.characters[name];
    if (!bought) {
      characterName.color = charactersMenu.inactiveTextColor;
      GetComponent<Renderer>().sharedMaterial = charactersMenu.inactiveCharactersMaterial;
    }

    if (PlayerPrefs.GetString("SelectedCharacter") == name) {
      selected = true;
    }
	}

	void FixedUpdate () {
    if (selected) {
      transform.Rotate(-Vector3.up * Time.deltaTime * charactersMenu.selectedCharacterRotationSpeed);
    }
	}

  override public void activateSelf() {
    charactersMenu.resetAllSelection();
    selected = true;
    if (bought) {
      charactersMenu.buyButton.gameObject.SetActive(false);
      charactersMenu.selectButton.gameObject.SetActive(true);
      charactersMenu.selectButton.setCharacter(name);
    } else {
      charactersMenu.buyButton.gameObject.SetActive(true);
      charactersMenu.selectButton.gameObject.SetActive(false);
      charactersMenu.buyButton.setCharacter(name);
    }
  }

  public void unselect() {
    selected = false;
    transform.localRotation = originalRotation;
  }

  void OnDisable() {
    if (PlayerPrefs.GetString("SelectedCharacter") == name) {
      selected = true;
    } else {
      unselect();
    }
  }
}

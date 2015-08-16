using UnityEngine;
using System.Collections;

public class CharactersMenu : MonoBehaviour {
  public Color inactiveTextColor;
  public Material inactiveCharactersMaterial;
  public float selectedCharacterRotationSpeed = 30;
  public CharacterSelectButton selectButton;
  public CharacterBuyButton buyButton;

  void OnEnable() {
    selectButton.setCharacter(PlayerPrefs.GetString("SelectedCharacter"));
  }

  public void resetAllSelection() {
    foreach (GameObject character in GameObject.FindGameObjectsWithTag("UICharacters")) {
      character.GetComponent<UICharacters>().unselect();
    }
  }
}

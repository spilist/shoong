using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICharacters : MonoBehaviour {
  public string characterName;
  private CharactersMenu charactersMenu;
  private Material originalMaterial;
  private Vector3 originalPosition;
  private Quaternion originalRotation;
  private Vector3 originalScale;
  private bool bought;

  void Start () {
    charactersMenu = GameObject.Find("CharactersMenu").GetComponent<CharactersMenu>();
    originalMaterial = GetComponent<Renderer>().sharedMaterial;
    originalPosition = transform.localPosition;
    originalRotation = transform.localRotation;
    originalScale = transform.localScale;

    bought = (bool)GameController.control.characters[name];
    if (!bought) {
      GetComponent<Renderer>().sharedMaterial = charactersMenu.inactiveCharactersMaterial;
    }
	}

	void Update () {
    if (Mathf.Abs(transform.parent.localPosition.x - originalPosition.x) < charactersMenu.selectWidth) {
      select();
    } else {
      unselect();
    }
	}

  void select() {
    transform.localPosition = new Vector3(transform.parent.localPosition.x, charactersMenu.selectedOffset_y, charactersMenu.selectedOffset_z);

    charactersMenu.characterName.text = characterName;
    transform.Rotate(-Vector3.up * Time.deltaTime * charactersMenu.selectedCharacterRotationSpeed);
    transform.localScale = originalScale * 2;
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
    transform.localPosition = originalPosition;
    transform.localRotation = originalRotation;
    transform.localScale = originalScale;
  }
}

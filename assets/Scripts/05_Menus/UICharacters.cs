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
  private float scaleChanging;
  private bool bought;
  private bool soundPlayed = false;

  void Start () {
    charactersMenu = GameObject.Find("CharactersMenu").GetComponent<CharactersMenu>();
    originalMaterial = GetComponent<Renderer>().sharedMaterial;
    originalPosition = transform.localPosition;
    originalRotation = transform.localRotation;
    originalScale = transform.localScale;
    scaleChanging = transform.localScale.x;

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
    if (!soundPlayed) {
      soundPlayed = true;
      AudioSource.PlayClipAtPoint(charactersMenu.characterSelectionSound, transform.position);
    }

    transform.localPosition = new Vector3(transform.parent.localPosition.x, charactersMenu.selectedOffset_y, charactersMenu.selectedOffset_z);

    charactersMenu.characterName.text = characterName;
    transform.Rotate(-Vector3.up * Time.deltaTime * charactersMenu.selectedCharacterRotationSpeed);

    if (scaleChanging != originalScale.x * 2) {
      scaleChanging = Mathf.MoveTowards(scaleChanging, originalScale.x * 2, Time.deltaTime * charactersMenu.scaleChangingSpeed);
      transform.localScale = new Vector3(scaleChanging, scaleChanging, scaleChanging);
    }

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
    soundPlayed = false;

    transform.localPosition = originalPosition;
    transform.localRotation = originalRotation;

    if (scaleChanging != originalScale.x) {
      scaleChanging = Mathf.MoveTowards(scaleChanging, originalScale.x, Time.deltaTime * charactersMenu.scaleChangingSpeed);
      transform.localScale = new Vector3(scaleChanging, scaleChanging, scaleChanging);
    }
  }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AbilityData;

public class UICharacters : MonoBehaviour {
  public CharacterStat stat;
  private int price;

  public string description;
  public string description2;

  private CharactersMenu charactersMenu;
  private Vector3 originalPosition;
  private Quaternion originalRotation;
  private Vector3 originalScale;
  private float scaleChanging;
  private bool soundPlayed = false;

  void OnEnable() {
    if (charactersMenu == null) return;

    checkBought(false);
  }

  void Start () {
    charactersMenu = GameObject.Find("CharactersMenu").GetComponent<CharactersMenu>();
    originalPosition = transform.localPosition;
    originalRotation = transform.localRotation;
    originalScale = transform.localScale;
    scaleChanging = transform.localScale.x;
    checkBought(false);
	}

	void Update () {
    if (Mathf.Abs(transform.parent.localPosition.x - originalPosition.x) < charactersMenu.selectWidth) {
      select();
    } else {
      unselect();
    }
	}

  public void setRarity(Text text) {
    Rarity rarity = stat.rarity;

    if (rarity == Rarity.Common) {
      text.text = "Common";
      text.color = charactersMenu.colorsPerRarity[0];
    } else if (rarity == Rarity.Rare) {
      text.text = "Rare";
      text.color = charactersMenu.colorsPerRarity[1];
    } else if (rarity == Rarity.Epic) {
      text.text = "Epic";
      text.color = charactersMenu.colorsPerRarity[2];
    }
  }

  void select() {
    if (!soundPlayed) {
      soundPlayed = true;
      if (charactersMenu.isJustOpened()) {
        charactersMenu.setOpened();
      } else {
        AudioSource.PlayClipAtPoint(charactersMenu.characterSelectionSound, transform.position);
      }

      stat = CharacterManager.cm.character(name);
      price = stat.price();

      charactersMenu.characterName.text = stat.characterName;
      setRarity(charactersMenu.rarity);

      charactersMenu.description.text = description;
      if (description2 != "") charactersMenu.description.text += "\n" + description2;

      checkBought();
    }

    transform.localPosition = new Vector3(transform.parent.localPosition.x, charactersMenu.selectedOffset_y, charactersMenu.selectedOffset_z);

    transform.Rotate(-Vector3.up * Time.deltaTime * charactersMenu.selectedCharacterRotationSpeed);

    if (scaleChanging != originalScale.x * 2) {
      scaleChanging = Mathf.MoveTowards(scaleChanging, originalScale.x * 2, Time.deltaTime * charactersMenu.scaleChangingSpeed);
      transform.localScale = new Vector3(scaleChanging, scaleChanging, scaleChanging);
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

  public void checkBought(bool buttons = true) {
    if (DataManager.dm.getBool(name)) {
      if (buttons) {
        charactersMenu.selectButton.gameObject.SetActive(true);
        charactersMenu.selectButton.setCharacter(name);
        charactersMenu.buyButton.gameObject.SetActive(false);
      }
      GetComponent<Renderer>().sharedMaterial = charactersMenu.activeCharactersMaterial;
    } else {
      if (buttons) {
        charactersMenu.buyButton.gameObject.SetActive(true);
        charactersMenu.buyButton.setCharacter(name, price);
        charactersMenu.selectButton.gameObject.SetActive(false);
      }
    }
  }

  public void buy() {
    charactersMenu.selectButton.gameObject.SetActive(true);
    charactersMenu.selectButton.setCharacter(name);
    charactersMenu.buyButton.gameObject.SetActive(false);
    charactersMenu.numYourCharacters.text = (int.Parse(charactersMenu.numYourCharacters.text) + 1).ToString();
    GetComponent<Renderer>().sharedMaterial = charactersMenu.activeCharactersMaterial;

    AudioSource.PlayClipAtPoint(charactersMenu.characterBuySound, transform.position);
  }
}

﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICharacters : MonoBehaviour {
  public string characterName;
  public int price;
  private CharactersMenu charactersMenu;
  private Material originalMaterial;
  private Vector3 originalPosition;
  private Quaternion originalRotation;
  private Vector3 originalScale;
  private float scaleChanging;
  private bool soundPlayed = false;

  void Start () {
    charactersMenu = GameObject.Find("CharactersMenu").GetComponent<CharactersMenu>();
    originalMaterial = GetComponent<Renderer>().sharedMaterial;
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

  void select() {
    if (!soundPlayed) {
      soundPlayed = true;
      AudioSource.PlayClipAtPoint(charactersMenu.characterSelectionSound, transform.position);
      charactersMenu.characterName.text = characterName;
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
    if ((bool)GameController.control.characters[name]) {
      if (buttons) {
        charactersMenu.selectButton.gameObject.SetActive(true);
        charactersMenu.selectButton.setCharacter(name);
        charactersMenu.buyButton.gameObject.SetActive(false);
      }
      GetComponent<Renderer>().sharedMaterial = originalMaterial;
    } else {
      if (buttons) {
        charactersMenu.buyButton.gameObject.SetActive(true);
        charactersMenu.buyButton.setCharacter(name, price);
        charactersMenu.selectButton.gameObject.SetActive(false);
      }
      GetComponent<Renderer>().sharedMaterial = charactersMenu.inactiveCharactersMaterial;
    }
  }

  public void buy() {
    charactersMenu.selectButton.gameObject.SetActive(true);
    charactersMenu.selectButton.setCharacter(name);
    charactersMenu.buyButton.gameObject.SetActive(false);
    GetComponent<Renderer>().sharedMaterial = originalMaterial;

    AudioSource.PlayClipAtPoint(charactersMenu.characterBuySound, transform.position);
  }
}

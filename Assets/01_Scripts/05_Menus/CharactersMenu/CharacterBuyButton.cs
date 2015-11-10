using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterBuyButton : MenusBehavior {
  public Color notAffordableTextColor;
  public Text priceText;

  private string characterName;

  void Start() {
    playTouchSound = false;
  }

  public void setCharacter(string nameVal, string price) {
    characterName = nameVal;
    priceText.text = price;

    priceText.color = new Color(255, 255, 255);
  }

  override public void activateSelf() {
    transform.parent.Find("Characters/" + characterName).GetComponent<UICharacters>().buy();
  }
}

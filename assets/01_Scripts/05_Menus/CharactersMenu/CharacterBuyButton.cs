using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterBuyButton : MenusBehavior {
  public Color notAffordableTextColor;
  public Text priceText;
  public BuyButtonsCubeIconPosition icon;

  private string characterName;
  private float price;
  private bool affordable = true;

  void Start() {
    playTouchSound = false;
  }

  public void setCharacter(string nameVal, float priceVal) {
    characterName = nameVal;
    price = priceVal;
    priceText.text = price.ToString("N");
    icon.adjust(priceText);

    affordable = true;
    priceText.color = new Color(255, 255, 255);
  }

  override public void activateSelf() {
    if (!affordable) return;

    DataManager.dm.setBool(characterName, true);
    DataManager.dm.increment("NumCharactersHave");
    transform.parent.Find("Characters/" + characterName).GetComponent<UICharacters>().buy();

    DataManager.dm.save();
  }
}

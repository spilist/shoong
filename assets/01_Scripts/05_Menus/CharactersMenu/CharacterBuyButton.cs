using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterBuyButton : MenusBehavior {
  public CubesYouHave goldenCubes;
  public Color notAffordableTextColor;
  public Text priceText;
  public BuyButtonsCubeIconPosition icon;

  private string characterName;
  private int price;
  private bool affordable = true;

  void Start() {
    playTouchSound = false;
  }

  public void setCharacter(string nameVal, int priceVal) {
    characterName = nameVal;
    price = priceVal;
    priceText.text = price.ToString("N0");
    icon.adjust(priceText);

    if (goldenCubes.youHave() < price) {
      affordable = false;
      priceText.color = notAffordableTextColor;
    } else {
      affordable = true;
      priceText.color = new Color(255, 255, 255);
    }
  }

  override public void activateSelf() {
    if (!affordable) return;

    goldenCubes.buy(price);
    DataManager.dm.setBool(characterName, true);
    DataManager.dm.increment("NumCharactersHave");
    transform.parent.Find("Characters/" + characterName).GetComponent<UICharacters>().buy();

    DataManager.dm.save();
  }
}

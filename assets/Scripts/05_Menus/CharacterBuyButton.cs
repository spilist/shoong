using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterBuyButton : MenusBehavior {
  public GoldenCubesYouHave goldenCubes;
  public Material notAffordableCubeMat;
  public Material originalMat;
  public Color notAffordableTextColor;
  public Renderer goldenCubeIconRenderer;
  public Text priceText;

  private string characterName;
  private int price;
  private bool affordable = true;

  public void setCharacter(string nameVal, int priceVal) {
    characterName = nameVal;
    price = priceVal;
    priceText.text = price.ToString("N0");

    if (goldenCubes.youHave() < price) {
      affordable = false;
      goldenCubeIconRenderer.sharedMaterial = notAffordableCubeMat;
      priceText.color = notAffordableTextColor;
    } else {
      affordable = true;
      goldenCubeIconRenderer.sharedMaterial = originalMat;
      priceText.color = new Color(255, 255, 255);
    }
  }

  override public void activateSelf() {
    if (!affordable) return;

    goldenCubes.buy(price);
    GetComponent<AudioSource>().Play();
    GameController.control.characters[characterName] = true;
    transform.parent.Find("Characters/" + characterName).GetComponent<UICharacters>().buy();
  }
}

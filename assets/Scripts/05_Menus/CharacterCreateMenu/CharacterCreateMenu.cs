using UnityEngine;
using System.Collections;

public class CharacterCreateMenu : MonoBehaviour {
  public GameObject cubeYouHave;
  public GameObject goldenCubeYouHave;
  public int[] createPrice;

  void OnEnable() {
    cubeYouHave.SetActive(true);
    goldenCubeYouHave.SetActive(true);
  }

  public int price() {
    int charactersCount = 0;
    foreach (DictionaryEntry character in GameController.control.characters) {
      if ((bool) character.Value) charactersCount++;
    }

    charactersCount = Mathf.Min(charactersCount, createPrice.Length);
    return createPrice[charactersCount-1];
  }
}

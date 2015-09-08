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
    charactersCount = Mathf.Min(DataManager.dm.getInt("NumCharactersHave"), createPrice.Length);
    return createPrice[charactersCount-1];
  }
}

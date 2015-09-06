using UnityEngine;
using System.Collections;

public class CharacterCreateMenu : MonoBehaviour {
  public GameObject cubeYouHave;
  public GameObject goldenCubeYouHave;
  public int createPrice = 10000;

  void OnEnable() {
    cubeYouHave.SetActive(true);
    goldenCubeYouHave.SetActive(true);
  }

  public int price() {
    return createPrice;
  }
}

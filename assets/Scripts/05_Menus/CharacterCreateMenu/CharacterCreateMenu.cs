using UnityEngine;
using System.Collections;

public class CharacterCreateMenu : MonoBehaviour {
  public GameObject cubeYouHave;
  public GameObject goldenCubeYouHave;

  void OnEnable() {
    cubeYouHave.SetActive(true);
    goldenCubeYouHave.SetActive(true);
  }
}

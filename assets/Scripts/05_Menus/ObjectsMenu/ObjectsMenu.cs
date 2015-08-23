using UnityEngine;
using System.Collections;

public class ObjectsMenu : Draggable {
  public GameObject cubeYouHave;
  public GameObject goldenCubeYouHave;

  private string[] mainObjects;
  private string[] subObjects;

	void OnEnable() {
    cubeYouHave.SetActive(true);
    goldenCubeYouHave.SetActive(true);

    // mainObjects = PlayerPrefs.GetString("MainObjects").Split(' ');
    // foreach (string mainObject in mainObjects) {
    //   // Debug.Log("Main: " + mainObject);
    // }

    // subObjects = PlayerPrefs.GetString("SubObjects").Split(' ');
    // foreach (string subObject in subObjects) {
    //   // Debug.Log("Sub: " + subObject);
    // }
  }
}

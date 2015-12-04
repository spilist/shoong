using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowOverlayButtons : MenusBehavior {
  public List<GameObject> buttons;
  bool isOpened = false;

  public override void activateSelf ()
  {
    isOpened = !isOpened;
    GetComponent<MeshFilter>().mesh = (isOpened ? inactiveMesh : activeMesh);
    foreach (GameObject button in buttons) {
      button.SetActive(isOpened);
    }
  }
}

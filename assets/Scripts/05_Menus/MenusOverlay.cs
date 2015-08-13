using UnityEngine;
using System.Collections;

public class MenusOverlay : MonoBehaviour {

  public void toggleShow() {
    gameObject.SetActive(!gameObject.activeSelf);
  }
}

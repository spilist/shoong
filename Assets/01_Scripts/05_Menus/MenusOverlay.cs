using UnityEngine;
using System.Collections;

public class MenusOverlay : MonoBehaviour {
  void OnEnable() {
    // if (transform.parent.Find("ObjectsMenu").gameObject.activeSelf) {
      // transform.Find("ObjectsMenu").gameObject.SetActive(true);
      // transform.Find("Normal").gameObject.SetActive(false);
    // } else {
      transform.Find("Normal").gameObject.SetActive(true);
      // transform.Find("ObjectsMenu").gameObject.SetActive(false);
    // }
  }
}

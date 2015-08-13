using UnityEngine;
using System.Collections;

public class MenusController : MonoBehaviour {
  public GameObject menusOverlay;

  public string touched() {
    Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
    RaycastHit hit;
    if( Physics.Raycast( ray, out hit, 100 ) ) {
      string hitTag = hit.transform.tag;
      if (hitTag != "Ground") {
        menusOverlay.SetActive(!menusOverlay.activeSelf);
        GameObject obj = transform.Find(hitTag).gameObject;
        obj.SetActive(!obj.activeSelf);
      }
      return hitTag;
    } else {
      return "nothing";
    }
  }
}

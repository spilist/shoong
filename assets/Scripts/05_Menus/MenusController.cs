using UnityEngine;
using System.Collections;

public class MenusController : MonoBehaviour {
  public GameObject menusOverlay;
  public GameObject idleUI;
  public GameObject inGameUI;
  public GameObject gameOverUI;

  private bool notYetStarted = true;
  private bool isGameEnded = false;

  public string touched() {
    Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
    RaycastHit hit;
    if( Physics.Raycast( ray, out hit, 100 ) ) {
      string hitTag = hit.transform.tag;
      if (hitTag != "Ground") {
        if (notYetStarted) {
          idleUI.SetActive(!idleUI.activeSelf);
        } else if (isGameEnded) {
          inGameUI.SetActive(!inGameUI.activeSelf);
          gameOverUI.GetComponent<Canvas>().enabled = !gameOverUI.GetComponent<Canvas>().enabled;
        }
        menusOverlay.SetActive(!menusOverlay.activeSelf);
        GameObject obj = transform.Find(hitTag).gameObject;
        obj.SetActive(!obj.activeSelf);
      }
      return hitTag;
    } else {
      return "nothing";
    }
  }

  public void gameStart() {
    notYetStarted = false;
  }

  public void gameEnd() {
    isGameEnded = true;
  }
}

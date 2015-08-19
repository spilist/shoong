using UnityEngine;
using System.Collections;

public class MenusController : MonoBehaviour {
  public GameObject menusOverlay;
  public GameObject backButton;
  public GameObject menuButtons;
  public GameObject idleUI;
  public GameObject inGameUI;
  public GameObject gameOverUI;

  private bool notYetStarted = true;
  private bool isGameEnded = false;
  private GameObject currentlyOn;

  public string touched() {
    Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
    RaycastHit hit;
    if( Physics.Raycast( ray, out hit, 100 ) ) {
      string hitTag = hit.transform.tag;
      string layer = LayerMask.LayerToName(hit.transform.gameObject.layer);
      string menuButtons = hit.transform.parent.name;
      if (menuButtons == "MenuButtonsLeft" || menuButtons == "MenuButtonsRight") {
        currentlyOn = transform.Find(hitTag).gameObject;
        toggleMenuAndUI();
      } else if (isMenuOn() && layer == "MenusBehavior") {
        hit.transform.GetComponent<MenusBehavior>().activateSelf();
      } else if (hitTag == "BackButton") {
        toggleMenuAndUI();
      }
      return hitTag;
    } else {
      return "nothing";
    }
  }

  public void toggleMenuAndUI() {
    if (notYetStarted) {
      idleUI.SetActive(!idleUI.activeSelf);
    } else if (isGameEnded) {
      inGameUI.SetActive(!inGameUI.activeSelf);
      gameOverUI.GetComponent<Canvas>().enabled = !gameOverUI.GetComponent<Canvas>().enabled;
    }
    menusOverlay.SetActive(!menusOverlay.activeSelf);
    backButton.SetActive(!backButton.activeSelf);
    menuButtons.SetActive(!menuButtons.activeSelf);
    currentlyOn.SetActive(!currentlyOn.activeSelf);
  }

  public void gameStart() {
    notYetStarted = false;
  }

  public void gameEnd() {
    isGameEnded = true;
  }

  public bool isMenuOn() {
    return menusOverlay.activeSelf;
  }

  public GameObject draggable() {
    return currentlyOn.GetComponent<Draggable>().draggable();
  }

  public float leftDragEnd() {
    return currentlyOn.GetComponent<Draggable>().leftDragEnd();
  }

  public float rightDragEnd() {
    return currentlyOn.GetComponent<Draggable>().rightDragEnd();
  }

  public void returnToEnd(string where) {
    if (where == "left") currentlyOn.GetComponent<Draggable>().returnToLeftEnd();
    else currentlyOn.GetComponent<Draggable>().returnToRightEnd();
  }
}

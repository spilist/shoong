using UnityEngine;
using System.Collections;

public class MenusController : MonoBehaviour {
  public GameObject menusOverlay;
  public GameObject backButton;
  public GameObject menuButtonsLeft;
  public GameObject menuButtonsRight;
  public GameObject idleUI;
  public GameObject inGameUI;
  public GameObject beforeIdleUI;
  public BeforeIdle beforeIdle;

  public AudioClip UITouchSound;

  private bool menuOn = false;
  private bool notYetStarted = true;
  private GameObject currentlyOn;

  public string touched() {
    Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
    RaycastHit hit;
    if( Physics.Raycast( ray, out hit, 100 ) ) {
      string hitTag = hit.transform.tag;
      string layer = LayerMask.LayerToName(hit.transform.gameObject.layer);
      string menuButtons = hit.transform.parent.name;
      if (menuButtons == "MenuButtonsLeft" || menuButtons == "MenuButtonsRight") {
        if (hitTag == "LinkButton") {
          hit.transform.GetComponent<MenusBehavior>().activateSelf();
        } else {
          currentlyOn = transform.Find(hitTag).gameObject;
          toggleMenuAndUI();
          AudioSource.PlayClipAtPoint(UITouchSound, hit.transform.position);
        }
      } else if (hitTag == "PauseButton" || (isMenuOn() && layer == "MenusBehavior") || (ScoreManager.sm.isGameOver() && layer == "MenusBehavior")) {
        if (beforeIdle.isLoading()) return "";

        MenusBehavior mb = hit.transform.GetComponent<MenusBehavior>();
        if (mb.playTouchSound) {
          AudioSource.PlayClipAtPoint(UITouchSound, hit.transform.position);
        }
        mb.activateSelf();
      }
      return hitTag;
    } else {
      return "nothing";
    }
  }

  public void toggleMenuAndUI() {
    if (notYetStarted) {
      idleUI.SetActive(!idleUI.activeSelf);
      beforeIdleUI.SetActive(!beforeIdleUI.activeSelf);
    }
    menuOn = !menuOn;
    backButton.SetActive(!backButton.activeSelf);
    menuButtonsLeft.SetActive(!menuButtonsLeft.activeSelf);
    menuButtonsRight.SetActive(!menuButtonsRight.activeSelf);
    currentlyOn.SetActive(!currentlyOn.activeSelf);
    menusOverlay.SetActive(!menusOverlay.activeSelf);
  }

  public void gameStart() {
    notYetStarted = false;
    inGameUI.SetActive(true);
    idleUI.SetActive(false);
    menuButtonsLeft.SetActive(false);
    menuButtonsRight.SetActive(false);
  }

  public bool isMenuOn() {
    return menuOn;
  }

  public GameObject currentMenu() {
    return currentlyOn;
  }

  public void setCurrentMenu(GameObject newCurrent) {
    currentlyOn = newCurrent;
  }

  public string draggableDirection() {
    if (currentlyOn.GetComponent<Draggable>() == null) return "";
    else return currentlyOn.GetComponent<Draggable>().direction;
  }

  public GameObject draggable() {
    return currentlyOn.GetComponent<Draggable>().draggable();
  }

  public float dragEnd(string where) {
    Draggable draggable = currentlyOn.GetComponent<Draggable>();
    if (where == "left") return draggable.leftDragEnd();
    else if (where == "right") return draggable.rightDragEnd();
    else if (where == "top") return draggable.topDragEnd();
    else return draggable.bottomDragEnd();
  }

  public void returnToEnd(string where) {
    Draggable draggable = currentlyOn.GetComponent<Draggable>();

    if (where == "left") draggable.returnToLeftEnd();
    else if (where == "right") draggable.returnToRightEnd();
    else if (where == "top") draggable.returnToTopEnd();
    else draggable.returnToBottomEnd();
  }

  public bool gameStarted() {
    return !notYetStarted;
  }

  public void changeBtwInsideMenu(GameObject outer, GameObject inner) {

  }
}

using UnityEngine;
using System.Collections;

public class MenusController : MonoBehaviour {
  public ScoreManager scoreManager;
  public GameObject menusOverlay;
  public GameObject backButton;
  public GameObject menuButtons;
  public GameObject idleUI;
  public GameObject inGameUI;
  public GameObject barsCanvas;
  public GameObject title;
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
        currentlyOn = transform.Find(hitTag).gameObject;
        toggleMenuAndUI();
        AudioSource.PlayClipAtPoint(UITouchSound, hit.transform.position);
      } else if (hitTag == "PauseButton" || (isMenuOn() && layer == "MenusBehavior") || (scoreManager.isGameOver() && layer == "MenusBehavior")) {
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
    }
    menuOn = !menuOn;
    backButton.SetActive(!backButton.activeSelf);
    menuButtons.SetActive(!menuButtons.activeSelf);
    currentlyOn.SetActive(!currentlyOn.activeSelf);
    menusOverlay.SetActive(!menusOverlay.activeSelf);
    title.SetActive(!title.activeSelf);
  }

  public void gameStart() {
    notYetStarted = false;
    inGameUI.SetActive(true);
    idleUI.SetActive(false);
    menuButtons.SetActive(false);

    barsCanvas.GetComponent<Canvas>().enabled = true;
    barsCanvas.transform.Find("EnergyBar").GetComponent<EnergyBar>().startGame();
  }

  public bool isMenuOn() {
    return menuOn;
  }

  public bool isDraggable() {
    return currentlyOn.GetComponent<Draggable>() != null;
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

  public bool gameStarted() {
    return !notYetStarted;
  }
}

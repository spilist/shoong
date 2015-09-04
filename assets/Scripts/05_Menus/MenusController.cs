using UnityEngine;
using System.Collections;

public class MenusController : MonoBehaviour {
  public GameObject menusOverlay;
  public GameObject backButton;
  public GameObject menuButtons;
  public GameObject idleUI;
  public GameObject inGameUI;
  public GameObject gameOverUI;
  public GameObject barsCanvas;

  public AudioClip UITouchSound;

  private bool menuOn = false;
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
        AudioSource.PlayClipAtPoint(UITouchSound, hit.transform.position);
      } else if (hitTag == "PauseButton" || (isMenuOn() && layer == "MenusBehavior")) {
        MenusBehavior mb = hit.transform.GetComponent<MenusBehavior>();
        mb.activateSelf();
        if (mb.playTouchSound) {
          AudioSource.PlayClipAtPoint(UITouchSound, hit.transform.position);
        }
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
    menuOn = !menuOn;
    backButton.SetActive(!backButton.activeSelf);
    menuButtons.SetActive(!menuButtons.activeSelf);
    currentlyOn.SetActive(!currentlyOn.activeSelf);
    menusOverlay.SetActive(!menusOverlay.activeSelf);
  }

  public void gameStart() {
    notYetStarted = false;
    inGameUI.SetActive(true);
    idleUI.SetActive(false);
    menuButtons.SetActive(false);

    barsCanvas.GetComponent<Canvas>().enabled = true;
    barsCanvas.transform.Find("EnergyBar").GetComponent<EnergyBar>().startDecrease();
  }

  public void gameEnd() {
    isGameEnded = true;
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
}

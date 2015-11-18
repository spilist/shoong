using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class TouchInputHandler : MonoBehaviour
{
  public Transform stick;
  public Transform fingerIndicator;
  private float stickPanelSize;

  public BeforeIdle beforeIdle;
  public SpawnManager spawnManager;
  public MenusController menus;
  public PauseButton pause;

	private bool gameStarted = false;
	private bool react = true;
	private bool dragging = false;
	private Vector3 direction;
  private float lastMousePosition_y;
  private float endMousePosition_y;
  private float lastMousePosition_x;
  private float endMousePosition_x;
  private Vector3 lastDraggablePosition;
  private string controlMethod;
  private int stickFingerId = -1;

  void Update() {
		if (Application.platform == RuntimePlatform.Android) {
      if (Input.GetKeyDown(KeyCode.Escape)) {
        if (gameStarted) {
          pause.activateSelf();
        } else if (menus.isMenuOn()) {
          menus.toggleMenuAndUI();
        } else {
          Application.Quit();
        }
        return;
      }
    }

    if (reactAble() && Input.GetMouseButtonDown(0)) {
      if (pause.isResuming()) return;

      string result = menus.touched();
      if (menus.isMenuOn()) return;

			if (result != "PauseButton" && pause.isPaused()) {
        pause.resume();
        return;
      }

			if ((result == "Ground" || result == "ChangeBehavior") && !gameStarted) {

        menus.gameStart();

        TimeManager.time.startTime();
        EnergyManager.em.turnEnergy(true);
        RhythmManager.rm.startGame();
        GoldManager.gm.startGame();
        spawnManager.run();
        DataManager.dm.increment("play_" + PlayerPrefs.GetString("SelectedCharacter"));

        beforeIdle.moveTitle();
        AudioManager.am.changeVolume("Main", "Max");

				gameStarted = true;

        controlMethod = DataManager.dm.getString("ControlMethod");
        stickPanelSize = Vector3.Distance(stick.position, stick.transform.Find("End").position);
        stick.gameObject.SetActive(true);
			}

      if (controlMethod == "Touch") {
        if (result == "Ground") {
          setPlayerDirection(Player.pl.transform);
          // Player.pl.shootBooster();
        }
      }
		}

    if (controlMethod == "Stick") {
      for (var i = 0; i < Input.touchCount; ++i) {
        Touch touch = Input.GetTouch(i);
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if ( Physics.Raycast(ray, out hit) ) {
          GameObject hitObject = hit.transform.gameObject;
          if (touch.phase == TouchPhase.Began) {
            if (hitObject.tag == "StickPanel_movement" && Input.touchCount == 1) {
              stick.position = newStickPosition();
              stick.gameObject.SetActive(true);
              stickFingerId = touch.fingerId;
            } else {
              hitObject.SendMessage("OnPointerDown");
            }
          }

          if (touch.phase == TouchPhase.Moved && touch.fingerId == stickFingerId) {
            setPlayerDirection(stick, touch);
          }

          if (touch.phase == TouchPhase.Ended) {
            if (touch.fingerId == stickFingerId) {
              Player.pl.stopMoving();
              stick.gameObject.SetActive(false);
              stickFingerId = -1;
              fingerIndicator.position = stick.position;
            } else {
              hitObject.SendMessage("OnPointerUp");
            }
          }
        }
      }
    }
	}

  public void setPlayerDirection(Transform origin, Touch touch) {
    if (Player.pl.uncontrollable()) return;

    Vector2 touchPosition = touch.position;
    Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.transform.position.y));
    Vector3 originPosition = new Vector3(origin.position.x, 0, origin.position.z);
    Vector3 heading = worldTouchPosition - originPosition;
    direction = heading / heading.magnitude;

    float indicatorDistance = heading.magnitude / stickPanelSize;
    if (indicatorDistance <= 1) {
      fingerIndicator.position = newStickPosition();
    } else {
      fingerIndicator.position = new Vector3(origin.position.x + (stickPanelSize * direction).x , 0, origin.position.z + (stickPanelSize * direction).z);
    }

    Player.pl.setDirection(direction, heading.magnitude / stickPanelSize);
  }

  public Vector3 setPlayerDirection(Transform origin) {
    if (Player.pl.uncontrollable()) return Vector3.zero;

    Vector2 touchPosition;

    if (Input.touchCount > 0) {
      touchPosition = Input.GetTouch(0).position;
    } else {
      touchPosition = Input.mousePosition;
    }

    Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.transform.position.y));
    Vector3 originPosition = new Vector3(origin.position.x, 0, origin.position.z);
    Vector3 heading = worldTouchPosition - originPosition;
    direction = heading / heading.magnitude;
    Player.pl.setDirection(direction);

    return worldTouchPosition;
  }

  Vector3 newStickPosition() {
    Vector2 touchPosition = Input.GetTouch(0).position;

    return Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, stick.position.y));
  }

	void OnMouseDown() {
    if (menus.isMenuOn() && menus.draggableDirection() != "") {
      lastMousePosition_x = Input.mousePosition.x;
	    lastMousePosition_y = Input.mousePosition.y;
      lastDraggablePosition = Camera.main.WorldToScreenPoint(menus.draggable().transform.position);
      dragging = true;
		}
  }

  void OnMouseDrag() {
    if (menus.isMenuOn() && menus.draggableDirection() != "") {
      Vector3 movement = Vector3.zero;
      if (menus.draggableDirection() == "LeftRight") {
        float positionX = menus.draggable().transform.localPosition.x;
        if (positionX == menus.dragEnd("left") || positionX == menus.dragEnd("right")) {
          endMousePosition_x = Input.mousePosition.x;
        }
        if (positionX > menus.dragEnd("left") || positionX < menus.dragEnd("right")) {
          movement = new Vector3((Input.mousePosition.x - endMousePosition_x)/5f + (endMousePosition_x - lastMousePosition_x), 0, 0);
        } else {
          movement = new Vector3(Input.mousePosition.x - lastMousePosition_x, 0, 0);
        }
      } else {
        float positionY = menus.draggable().transform.localPosition.y;
        if (positionY == menus.dragEnd("top") || positionY == menus.dragEnd("bottom")) {
          endMousePosition_y = Input.mousePosition.y;
        }
        if (positionY > menus.dragEnd("top") || positionY < menus.dragEnd("bottom")) {
          movement = new Vector3(0, (Input.mousePosition.y - endMousePosition_y)/5f + (endMousePosition_y - lastMousePosition_y), 0);
        } else {
          movement = new Vector3(0, Input.mousePosition.y - lastMousePosition_y, 0);
        }
      }

      menus.draggable().transform.position = Camera.main.ScreenToWorldPoint(lastDraggablePosition + movement);
    }
  }

  void OnMouseUp() {
    if (menus.isMenuOn() && dragging) {
      dragging = false;

      if (menus.draggableDirection() == "LeftRight") {
        float positionX = menus.draggable().transform.localPosition.x;
        if (positionX > menus.dragEnd("left")) {
          menus.returnToEnd("left");
        } else if (positionX < menus.dragEnd("right")) {
          menus.returnToEnd("right");
        }
      } else {
        float positionY = menus.draggable().transform.localPosition.y;
        if (positionY > menus.dragEnd("top")) {
          menus.returnToEnd("top");
        } else if (positionY < menus.dragEnd("bottom")) {
          menus.returnToEnd("bottom");
        }
      }
    }

  }

	public void stopReact() {
		react = false;
	}

  bool reactAble() {
    return !beforeIdle.isLoading() && react;
  }
}

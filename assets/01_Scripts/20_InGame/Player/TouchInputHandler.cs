using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class TouchInputHandler : MonoBehaviour
{
  public Collider boundary;

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

    if (Input.GetAxis("Horizontal") != 0) {
      float horiz = Input.GetAxis("Horizontal");
      string dirStr = horiz < 0 ? "LRPanel_left" : "LRPanel_right";
      Player.pl.setPerpDirection(dirStr);
    } else {
      Player.pl.playerShip.tiltBack();
    }

    if (reactAble() && Input.GetMouseButtonDown(0)) {
      if (pause.isResuming()) return;

      string result = menus.touched();
      Debug.Log(result);
      if (menus.isMenuOn()) return;

			if (result == "nothing" && pause.isPaused()) {
        pause.resume();
        return;
      }

      if (Player.pl.uncontrollable()) return;

			if ((result == "nothing" || result == "ChangeBehavior") && !gameStarted) {
				TimeManager.time.startTime();
        EnergyManager.em.turnEnergy(true);
        SuperheatManager.sm.startGame();
        beforeIdle.moveTitle();
        menus.gameStart();
        spawnManager.run();
        SkillManager.sm.startGame();
        AudioManager.am.changeVolume("Main", "Max");

        DataManager.dm.increment("play_" + PlayerPrefs.GetString("SelectedCharacter"));

        boundary.enabled = true;
				gameStarted = true;
        return;
			}
    }

    for (var i = 0; i < Input.touchCount; ++i) {
      Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
      RaycastHit hit;
      if ( Physics.Raycast(ray, out hit) ) {
        GameObject hitObject = hit.transform.gameObject;

        if (Input.GetTouch(i).phase == TouchPhase.Began) {
          hitObject.SendMessage("OnPointerDown");
        }

        if (Input.GetTouch(i).phase == TouchPhase.Ended) {
          hitObject.SendMessage("OnPointerUp");
        }
      }
    }
	}

  public Vector3 setPlayerDirection(Transform origin) {
    Vector2 touchPosition;
    if (Player.pl.uncontrollable()) return Vector3.zero;

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


using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class TouchInputHandler : MonoBehaviour
{
	public PlayerMover player;
	public ParticleSystem touchEffect;

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

    if (reactAble() && Input.GetMouseButtonDown(0) && menus.touched() == "Ground" && !menus.isMenuOn()) {
			if (pause.isPaused()) {
        pause.resume();
        return;
      }

      if (player.uncontrollable()) return;

			if (!gameStarted) {
				beforeIdle.moveTitle();
        menus.gameStart();
        spawnManager.run();
        AudioManager.am.changeVolume("Main", "Max");

        DataManager.dm.increment("play_" + PlayerPrefs.GetString("SelectedCharacter"));

				gameStarted = true;
			}

			Vector3 touchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
			Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
      Vector3 heading = worldTouchPosition - player.transform.position;
			direction = heading / heading.magnitude;

      player.setDirection(direction);

      if (player.isUsingDopple()) {
        player.teleport(worldTouchPosition);
      } else {
  			player.shootBooster();
      }

      Instantiate(touchEffect, worldTouchPosition, Quaternion.Euler(90, 0, 0));
		}
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


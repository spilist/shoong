using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class TouchInputHandler : MonoBehaviour
{
	public PlayerMover player;
	public ParticleSystem touchEffect;

  public SpawnManager spawnManager;
	public MenusController menus;

	private bool gameStarted = false;
	private bool react = true;
	private bool dragging = false;
	private Vector3 direction;
  private float lastMousePosition_x;
  private float endMousePosition_x;
  private Vector3 lastDraggablePosition;

	void Update() {
		if (react && Input.GetMouseButtonDown(0) && menus.touched() == "Ground" && !menus.isMenuOn()) {
			if (player.isRebounding() || player.isUsingRainbow()) return;

			if (!gameStarted) {
				menus.gameStart();
        spawnManager.run();

				gameStarted = true;
			}

			Vector3 touchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
			Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
			Vector3 heading = worldTouchPosition - player.transform.position;
			direction = heading / heading.magnitude;

			player.shootBooster(direction);
			Instantiate(touchEffect, worldTouchPosition, Quaternion.Euler(90, 0, 0));
		}
	}

	void OnMouseDown() {
    if (menus.isMenuOn() && menus.isDraggable()) {
	    lastMousePosition_x = Input.mousePosition.x;
      lastDraggablePosition = Camera.main.WorldToScreenPoint(menus.draggable().transform.position);
      dragging = true;
		}
  }

  void OnMouseDrag() {
    if (menus.isMenuOn() && menus.isDraggable()) {
      float positionX = menus.draggable().transform.localPosition.x;
  		Vector3 movement;
    	if (positionX == menus.leftDragEnd() || positionX == menus.rightDragEnd()) {
    		endMousePosition_x = Input.mousePosition.x;
    	}
    	if (positionX > menus.leftDragEnd() || positionX < menus.rightDragEnd()) {
    		movement = new Vector3((Input.mousePosition.x - endMousePosition_x)/5f + (endMousePosition_x - lastMousePosition_x), 0, 0);
    	} else {
    		movement = new Vector3(Input.mousePosition.x - lastMousePosition_x, 0, 0);
    	}

      menus.draggable().transform.position = Camera.main.ScreenToWorldPoint(lastDraggablePosition + movement);
    }
  }

  void OnMouseUp() {
  	if (menus.isMenuOn() && dragging) {
  		dragging = false;
  		float positionX = menus.draggable().transform.localPosition.x;
  		if (positionX > menus.leftDragEnd()) {
  			menus.returnToEnd("left");
			} else if (positionX < menus.rightDragEnd()) {
				menus.returnToEnd("right");
			}
  	}
  }

	public void stopReact() {
		react = false;
	}
}


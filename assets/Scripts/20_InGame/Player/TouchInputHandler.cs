using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class TouchInputHandler : MonoBehaviour
{
	public PlayerMover player;
	public GameObject barsCanvas;
	public EnergyBar energyBar;
	public ComboBar comboBar;
	public ElapsedTime elapsedTime;
	public GameObject idleUI;
	public GameObject inGameUI;
	public ParticleSystem touchEffect;

	public GameObject objectsManager;
	private FieldObjectsManager fom;
	private ComboPartsManager cpm;
	private MonsterManager monm;
	private BlackholeManager blm;
	private CubeDispenserManager cdm;

	public MenusController menus;

	private bool gameStarted = false;
	private bool react = true;
	private bool dragging = false;
	private Vector3 direction;
  private float lastMousePosition_x;
  private float endMousePosition_x;
  private Vector3 lastDraggablePosition;

	void Start() {
		fom = objectsManager.GetComponent<FieldObjectsManager>();
		cpm = objectsManager.GetComponent<ComboPartsManager>();
		monm = objectsManager.GetComponent<MonsterManager>();
		blm = objectsManager.GetComponent<BlackholeManager>();
		cdm = objectsManager.GetComponent<CubeDispenserManager>();

		barsCanvas.GetComponent<Canvas>().enabled = false;
	}

	void Update() {
		if (react && Input.GetMouseButtonDown(0) && menus.touched() == "Ground" && !menus.isMenuOn()) {
			if (player.isRebounding()) return;

			if (!gameStarted) {
				menus.gameStart();
				fom.run();

        cpm.run();
				monm.run();
				blm.run();
				cdm.run();

        barsCanvas.GetComponent<Canvas>().enabled = true;
				energyBar.startDecrease();
				inGameUI.SetActive(true);
				elapsedTime.startTime();
				idleUI.SetActive(false);
				menus.gameObject.SetActive(false);
				gameStarted = true;
			}

			player.rotatePlayerBody();
			Vector3 touchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
			Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
			Vector3 heading = worldTouchPosition - player.transform.position;
			direction = heading / heading.magnitude;

			if (!player.isUnstoppable()) {
				energyBar.loseByShoot();
				comboBar.loseByShoot();
				player.boosterSpeedup();
			}

			player.booster.Play();
			player.booster.GetComponent<AudioSource>().Play();
			player.setDirection(direction);
			Instantiate(touchEffect, worldTouchPosition, Quaternion.identity);

			cpm.tryToGet();
		}
	}

	void OnMouseDown() {
    if (menus.isMenuOn()) {
	    lastMousePosition_x = Input.mousePosition.x;
      lastDraggablePosition = Camera.main.WorldToScreenPoint(menus.draggable().transform.position);
      dragging = true;
		}
  }

  void OnMouseDrag() {
    if (menus.isMenuOn()) {
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


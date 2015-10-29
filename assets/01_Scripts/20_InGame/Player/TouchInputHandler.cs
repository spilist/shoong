using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class TouchInputHandler : MonoBehaviour
{
  public Transform controlPanel_circle_left;
  public Transform controlPanel_circle_right;
  public Transform controlPanel_packman_left;
  public Transform controlPanel_packman_right;
  public Transform stick;
  private float stickPanelSize;

  public BeforeIdle beforeIdle;
  public SpawnManager spawnManager;
  public MenusController menus;
  public PauseButton pause;

	public GameObject touchEffect;
  public int touchEffectAmount = 30;
  public List<GameObject> touchEffectPool;

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

  private GameObject lastHitObject;

  void Awake() {
    touchEffectPool = new List<GameObject>();
    for (int i = 0; i < touchEffectAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(touchEffect);
      obj.SetActive(false);
      touchEffectPool.Add(obj);
    }
  }

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

			if (pause.isPaused()) {
        pause.resume();
        return;
      }

      if (Player.pl.uncontrollable()) return;

			if ((result == "Ground" || result == "ChangeBehavior") && !gameStarted) {
				TimeManager.time.startTime();
        EnergyManager.em.turnEnergy(true);
        // SuperheatManager.sm.startGame();
        beforeIdle.moveTitle();
        menus.gameStart();
        spawnManager.run();
        SkillManager.sm.startGame();
        GoldManager.gm.startGame();
        AudioManager.am.changeVolume("Main", "Max");

        DataManager.dm.increment("play_" + PlayerPrefs.GetString("SelectedCharacter"));

				gameStarted = true;
        controlMethod = DataManager.dm.getString("ControlMethod");
        stickPanelSize = Vector3.Distance(stick.position, stick.transform.Find("End").position);
        return;
			}

      if (controlMethod == "Touch") {
        if (result == "Ground") {
          Vector3 worldTouchPosition = setPlayerDirection(Player.pl.transform);

          if (Player.pl.isUsingDopple()) {
            Player.pl.teleport(worldTouchPosition);
          } else {
            Player.pl.shootBooster();
            spawnTouchEffect(worldTouchPosition);
          }
        } else if (result == "SkillButton") {
          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
          RaycastHit hit;
          if ( Physics.Raycast(ray, out hit) ) {
            GameObject hitObject = hit.transform.gameObject;
            hitObject.SendMessage("OnPointerDown");
          }
        }
      } else if (controlMethod == "Circle" || controlMethod == "Packman") {
        if (result == "ControlPanel_circle_left") {
          setPlayerDirection(controlPanel_circle_left);
          Player.pl.shootBooster();
        } else if (result == "ControlPanel_circle_right") {
          setPlayerDirection(controlPanel_circle_right);
          Player.pl.shootBooster();
        } else if (result == "ControlPanel_packman_left") {
          setPlayerDirection(controlPanel_packman_left);
          Player.pl.shootBooster();
        } else if (result == "ControlPanel_packman_right") {
          setPlayerDirection(controlPanel_packman_right);
          Player.pl.shootBooster();
        }
      }
		}

    if (controlMethod == "Stick" || controlMethod == "LR") {
      for (var i = 0; i < Input.touchCount; ++i) {
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
        RaycastHit hit;
        if ( Physics.Raycast(ray, out hit) ) {
          GameObject hitObject = hit.transform.gameObject;

          if (controlMethod == "Stick") {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
              if (hitObject.tag == "StickPanel_movement") {
                stick.position = newStickPosition();
              } else {
                hitObject.SendMessage("OnPointerDown");
              }
            }

            if (Input.GetTouch(i).phase == TouchPhase.Moved && (hitObject.tag == "StickPanel_movement" || hitObject.tag == "Ground")) {
              setPlayerDirection(stick);
            }

            if (Input.GetTouch(i).phase == TouchPhase.Ended) {
              if (hitObject.tag == "StickPanel_movement" || hitObject.tag == "Ground") Player.pl.stopMoving();
              else hitObject.SendMessage("OnPointerUp");
            }
          } else if (controlMethod == "LR") {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
              hitObject.SendMessage("OnPointerDown");
            }

            if (Input.GetTouch(i).phase == TouchPhase.Ended) {
              hitObject.SendMessage("OnPointerUp");
            }
          }
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
    if (controlMethod == "Stick") {
      Player.pl.setDirection(direction, heading.magnitude / stickPanelSize);
    } else {
      Player.pl.setDirection(direction);
    }

    return worldTouchPosition;
  }

  Vector3 newStickPosition() {
    Vector2 touchPosition = Input.mousePosition;

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

  void spawnTouchEffect(Vector3 pos) {
    GameObject effect = null;
    for (int i = 0; i < touchEffectPool.Count; i++) {
      if (!touchEffectPool[i].activeInHierarchy) {
        effect = touchEffectPool[i];
        break;
      }
    }

    if (effect == null) {
      effect = (GameObject) Instantiate(touchEffect);
      touchEffectPool.Add(effect);
    }

    effect.transform.position = pos;
    effect.SetActive(true);
  }
}


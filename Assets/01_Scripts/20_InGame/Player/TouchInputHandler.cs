using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class TouchInputHandler : MonoBehaviour
{
  public Transform stick;
  public Transform fingerIndicator;
  public FingerTutorialViewer fingerTutorialViewer;
  private float stickPanelSize;

  public BeforeIdle beforeIdle;
  public SpawnManager spawnManager;
  public MenusController menus;
  public PauseButton pause;
  public GameObject goToMain;
  public GameObject goToMainActivated;
  public GameObject gameOver;
  public PlayAgainButton playAgain;
  public CharacterCreateButton characterCreateButton;
  public CharacterCreateBannerButton characterCreateBannerButton;
  public BackButton backButton;
  public float LRSpeed;

  private bool gameStarted = false;
	private bool react = true;
	private bool dragging = false;
  private bool playAgainTouched = false;
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
        if (gameStarted && !pause.isPaused() && pause.gameObject.activeInHierarchy && !gameOver.activeInHierarchy) {
          pause.activateSelf();
        } else if (gameStarted && pause.isPaused() && pause.gameObject.activeInHierarchy && !gameOver.activeInHierarchy) {
          pause.resume();
        } else if (gameOver.activeInHierarchy && playAgain.gameObject.activeInHierarchy && !playAgainTouched) {
          playAgainTouched = true;
          playAgain.activateSelf();
        } else if (menus.isMenuOn()) {
          menus.toggleMenuAndUI();
        } else if (characterCreateButton.gameObject.activeInHierarchy && !characterCreateButton.running) {
          characterCreateBannerButton.goBack();
          backButton.activateSelf();
        } else if (!gameStarted) {
          Application.Quit();
        }
        return;
      }
    }

    if (reactAble() && Input.GetMouseButtonDown(0)) {
      string result = menus.touched();
      if (menus.isMenuOn()) return;

      if (!pause.isResuming() && pause.isPaused()) {
        if (result == "GoToMainButton") {
          if (goToMain.activeInHierarchy) {
            goToMain.gameObject.SetActive(false);
            goToMainActivated.gameObject.SetActive(true);
          } else {
            goToMain.gameObject.SetActive(true);
            goToMainActivated.gameObject.SetActive(false);
            pause.resumeNow();
            ScoreManager.sm.gameOver("AbondonedGame");
          }
        } else if (result != "PauseButton") {
          goToMain.gameObject.SetActive(true);
          goToMainActivated.gameObject.SetActive(false);
          pause.resume();
        }
      }

      if (!pause.isResuming() && (result == "Ground" || result == "ChangeBehavior") && !gameStarted) {

        fingerTutorialViewer.disableViewer();
        CharacterManager.cm.startGame();
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
          Player.pl.shootBooster();
        }
      }
    }

    if (reactAble() && controlMethod == "Stick") {
      for (var i = 0; i < Input.touchCount; ++i) {
        Touch touch = Input.GetTouch(i);
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
          GameObject hitObject = hit.transform.gameObject;
          if (touch.phase == TouchPhase.Began) {
            if (hitObject.tag == "StickPanel_movement") {
              // if (hitObject.tag == "StickPanel_movement" && Input.touchCount == 1) {
              // 고정스틱
              stick.position = newStickPosition();
              stick.gameObject.SetActive(true);

              stickFingerId = touch.fingerId;
            } else {
              hitObject.SendMessage("OnPointerDown");
            }
          }

          if (touch.phase == TouchPhase.Moved && touch.fingerId == stickFingerId && hitObject.tag == "StickPanel_movement") {
            setPlayerDirection(stick, touch);
          }

          if (touch.phase == TouchPhase.Ended) {
            if (touch.fingerId == stickFingerId) {
              stickFingerId = -1;

              // 고정스틱
              Player.pl.stopMoving();
              stick.gameObject.SetActive(false);
              fingerIndicator.position = stick.position;
            } else {
              hitObject.SendMessage("OnPointerUp");
            }
          }
        }
      }
    }

    if (reactAble() && controlMethod == "CenterBigStick") {
      if (!Input.touchSupported) {
        string result = menus.touched();
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) {
          if (result == "Ground") {
            setPlayerDirection(Player.pl.transform);
          }
        }
      } else {
        for (var i = 0; i < Input.touchCount; ++i) {
          Touch touch = Input.GetTouch(i);
          setPlayerDirection(Player.pl.transform, touch);
        }
      }
    }

    if (reactAble() && controlMethod == "LR") {
      if (!Input.touchSupported) {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && menus.touched() == "Ground") {
          if (Input.mousePosition.x < Screen.width / 2) {
            Player.pl.setDirection(Quaternion.AngleAxis(-LRSpeed, Vector3.up) * Player.pl.getDirection());
          } else {
            Player.pl.setDirection(Quaternion.AngleAxis(LRSpeed, Vector3.up) * Player.pl.getDirection());
          }
          Player.pl.lrSpeedModifier -= Time.deltaTime/4;
          if (Player.pl.lrSpeedModifier < Player.pl.playerSpeedModifierMin) Player.pl.lrSpeedModifier = Player.pl.playerSpeedModifierMin;
        } else {
          Player.pl.lrSpeedModifier += Time.deltaTime/2;
          if (Player.pl.lrSpeedModifier > Player.pl.playerSpeedModifierMax) Player.pl.lrSpeedModifier = Player.pl.playerSpeedModifierMax;
        }
      } else {
        for (var i = 0; i < Input.touchCount; ++i) {
          Touch touch = Input.GetTouch(i);
          if (touch.position.x < Screen.width / 2) {
            Player.pl.setDirection(Quaternion.AngleAxis(-LRSpeed, Vector3.up) * Player.pl.getDirection());
          } else {
            Player.pl.setDirection(Quaternion.AngleAxis(LRSpeed, Vector3.up) * Player.pl.getDirection());
          }
          // FIXME: if more than too touch is input, then speed goes down twice faster
          Player.pl.lrSpeedModifier -= Time.deltaTime/4;
          if (Player.pl.lrSpeedModifier < Player.pl.playerSpeedModifierMin) Player.pl.lrSpeedModifier = Player.pl.playerSpeedModifierMin;
        }
        if (Input.touchCount == 0) {
          Player.pl.lrSpeedModifier += Time.deltaTime/2;
          if (Player.pl.lrSpeedModifier > Player.pl.playerSpeedModifierMax) Player.pl.lrSpeedModifier = Player.pl.playerSpeedModifierMax;
        }
      }
    }
  }

  public void setLRSpeed(Slider speed) {
    this.LRSpeed = speed.value;
  }

  public void setPlayerDirection(Transform origin, Touch touch) {
    if (Player.pl.uncontrollable()) return;

    Vector2 touchPosition = touch.position;
    Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.transform.position.y));

    Vector3 originPosition = new Vector3(origin.position.x, 0, origin.position.z);
    Vector3 heading = worldTouchPosition - originPosition;
    if (Player.pl.isConfused()) heading = -heading;

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
    if (Player.pl.isConfused()) heading = -heading;

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
    if (beforeIdle == null)
      return react;
    else
      return !beforeIdle.isLoading() && react;
  }
}

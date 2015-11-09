using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class TutorialHandler : MonoBehaviour
{
  private int tutoStatus = 0;
  public GameObject[] tutorialScenes;
  public Collider tutoCollider;
  public GameObject tutoParts;
  private bool tutoLoaded = false;

  public Transform stickOnTuto;
  public Transform fingerIndicatorOnTuto;

  public Transform stick;
  public Transform fingerIndicator;
  private float stickPanelSize;

  public BeforeIdle beforeIdle;
  public SpawnManager spawnManager;
  public MenusController menus;
  public PauseButton pause;
  public GameObject idleUI;

  private bool gameStarted = false;
  private bool react = true;
  private Vector3 direction;
  private int stickFingerId = -1;
  public Text[] touchTexts;
  public Text[] onthebeatTexts;
  public Image[] boostImages;
  public GameObject[] fingerImages;
  private Transform origStick;
  private Transform origFingerIndicator;
  public PlayerDirectionIndicator dirIndicator;

  void Awake() {
    origStick = stick;
    origFingerIndicator = fingerIndicator;
  }

  void enableTutoCollider() {
    tutoCollider.enabled = true;
    tutoLoaded = false;
  }

  public void nextTutorial(int status) {
    tutoParts.SetActive(false);
    Player.pl.stopMoving();

    tutorialScenes[status].SetActive(false);
    tutorialScenes[status + 1].SetActive(true);
    tutoCollider.enabled = false;
    tutoStatus++;

    if (status == 1) {
      RhythmManager.rm.startBeat(touchTexts[0], onthebeatTexts[0], boostImages[0], fingerImages[0]);
    }

    Invoke("enableTutoCollider", 2);
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

      if (result != "PauseButton" && pause.isPaused()) {
        pause.resume();
        return;
      }

      if (result == "SkipButton") {
        tutoCollider.enabled = false;
        tutoLoaded = true;
        CancelInvoke();

        if (tutoStatus == 0 || tutoStatus == 1) {
          tutoStatus += (1 - tutoStatus);
          tutorialScenes[0].SetActive(false);
          spawnManager.GetComponent<FollowTarget>().enabled = true;
          stick.gameObject.SetActive(false);
          nextTutorial(1);
        } else {
          tutoStatus += (2 - tutoStatus);
          tutorialScenes[2].SetActive(false);
          RhythmManager.rm.startBeat(touchTexts[1], onthebeatTexts[1], boostImages[1], fingerImages[1]);
          spawnManager.GetComponent<FollowTarget>().enabled = true;
          Player.pl.stopMoving(false);
          nextTutorial(3);
        }
        return;
      }

      if (result == "TutorialCollider" && !tutoLoaded) {
        if (tutoStatus == 0) {
          tutoLoaded = true;
          beforeIdle.moveTitle();
          idleUI.SetActive(false);
          tutorialScenes[0].SetActive(true);
          tutoCollider.enabled = false;
          Player.pl.stopMoving();
          tutoStatus++;
          Invoke("enableTutoCollider", 2);
        } else if (tutoStatus == 1) {
          tutoLoaded = true;
          tutorialScenes[0].SetActive(false);
          tutorialScenes[1].SetActive(true);
          tutoCollider.enabled = false;
          tutoParts.SetActive(true);
          stick = stickOnTuto;
          fingerIndicator = fingerIndicatorOnTuto;

          spawnManager.GetComponent<FollowTarget>().enabled = false;

          stickPanelSize = Vector3.Distance(stick.position, stick.transform.Find("End").position);
          stick.gameObject.SetActive(true);
        } else if (tutoStatus == 2) {
          tutoLoaded = true;
          tutorialScenes[2].SetActive(false);
          tutorialScenes[3].SetActive(true);
          tutoCollider.enabled = false;
          RhythmManager.rm.startBeat(touchTexts[1], onthebeatTexts[1], boostImages[1], fingerImages[1]);
          spawnManager.GetComponent<FollowTarget>().enabled = true;
          Player.pl.stopMoving(false);
        } else if (tutoStatus == 3) {
          startGame();
        }
      }

      // if (result == "StickPanel_movement") {
      //   Vector3 worldTouchPosition = setPlayerDirection(Player.pl.transform);
      // }

      if (!gameStarted && result == "StickPanel_booster") {
        Player.pl.shootBooster();
      }
    }

    if (tutoParts.activeSelf || gameStarted) {
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
            } else hitObject.SendMessage("OnPointerUp");
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

    if (dirIndicator.gameObject.activeInHierarchy) dirIndicator.setDirection(direction);
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

    if (dirIndicator.gameObject.activeInHierarchy) dirIndicator.setDirection(direction);

    return worldTouchPosition;
  }

  Vector3 newStickPosition() {
    Vector2 touchPosition = Input.GetTouch(0).position;

    return Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, stick.position.y));
  }

  public void stopReact() {
    react = false;
  }

  bool reactAble() {
    return !beforeIdle.isLoading() && react;
  }

  void startGame() {
    tutoLoaded = true;
    tutorialScenes[4].SetActive(false);
    menus.gameStart();
    tutoCollider.enabled = false;
    Player.pl.stopMoving(false);

    gameStarted = true;

    TimeManager.time.startTime();
    EnergyManager.em.turnEnergy(true);
    RhythmManager.rm.startGame();
    GoldManager.gm.startGame();
    spawnManager.run();
    AudioManager.am.changeVolume("Main", "Max");

    stick = origStick;
    fingerIndicator = origFingerIndicator;
    stick.gameObject.SetActive(true);
    stickPanelSize = Vector3.Distance(stick.position, stick.transform.Find("End").position);
    DataManager.dm.setBool("TutorialDone", true);
  }
}


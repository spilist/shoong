﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class TutorialHandler : MonoBehaviour {
  private int tutoStatus = 0;
  public GameObject[] tutorialScenes;
  public Collider tutoCollider;
  public GameObject tutoParts;
  private bool tutoLoaded = false;
  public GameObject opening;

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
  public Transform[] rhythmRings;
  public Text[] touchTexts;
  public Text[] onthebeatTexts;
  public Image[] boostImages;
  public GameObject[] fingerImages;
  private Transform origStick;
  private Transform origFingerIndicator;
  public PlayerDirectionIndicator dirIndicator;
  private bool skipped = false;

  public Transform energyUI_1;
  public GameObject helper;
  public GameObject moveBoundary;

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
      TrackingManager.tm.firstPlayLog("4_UseBoosterStart");
      RhythmManager.rm.startBeat(touchTexts[0], onthebeatTexts[0], boostImages[0], fingerImages[0], rhythmRings[0]);
      EnergyManager.em.getFullHealth();
    } else {
      DataManager.dm.setBool("TutorialDone", true);
      TrackingManager.tm.tutorialDone(skipped);
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
        skipped = true;
        tutoCollider.enabled = false;
        tutoLoaded = true;
        CancelInvoke();

        if (tutoStatus == 0 || tutoStatus == 1) {
          tutoStatus += (1 - tutoStatus);
          tutorialScenes[0].SetActive(false);
          spawnManager.GetComponent<FollowTarget>().enabled = true;
          // stick.gameObject.SetActive(false);
          nextTutorial(1);
        } else if (tutoStatus == 2) {
          tutoStatus += (2 - tutoStatus);
          tutorialScenes[2].SetActive(false);
          RhythmManager.rm.startBeat(touchTexts[1], onthebeatTexts[1], boostImages[1], fingerImages[1], rhythmRings[1]);
          spawnManager.GetComponent<FollowTarget>().enabled = true;
          Player.pl.stopMoving(false);
          nextTutorial(3);
        } else {
          replay();
          // Application.LoadLevel(Application.loadedLevel);
        }
        return;
      }

      if (result == "TutorialCollider" && !tutoLoaded) {
        if (tutoStatus == 0) {
          TrackingManager.tm.firstPlayLog("2_TutorialStart");
          tutoLoaded = true;
          opening.SetActive(false);
          idleUI.SetActive(false);
          tutorialScenes[0].SetActive(true);
          tutoCollider.enabled = false;
          Player.pl.stopMoving();
          Player.pl.GetComponent<MeshRenderer>().enabled = true;
          Player.pl.rb.isKinematic = false;
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

          EnergyManager.em.setEnergyUI(energyUI_1);
        } else if (tutoStatus == 2) {
          tutoLoaded = true;
          tutorialScenes[2].SetActive(false);
          tutorialScenes[3].SetActive(true);
          tutoCollider.enabled = false;
          RhythmManager.rm.startBeat(touchTexts[1], onthebeatTexts[1], boostImages[1], fingerImages[1], rhythmRings[1]);
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
            if (hitObject.tag == "StickPanel_movement") {
            // if (hitObject.tag == "StickPanel_movement" && Input.touchCount == 1) {
              // stick.position = newStickPosition();
              stick.gameObject.SetActive(true);
              stickFingerId = touch.fingerId;

              // if (helper.activeInHierarchy) helper.SetActive(false);
            } else {
              hitObject.SendMessage("OnPointerDown");
            }
          }

          if (touch.phase == TouchPhase.Moved && touch.fingerId == stickFingerId && hitObject.tag == "StickPanel_movement") {
            setPlayerDirection(stick, touch);
          }

          if (touch.phase == TouchPhase.Ended) {
            if (touch.fingerId == stickFingerId) {
              // Player.pl.stopMoving();
              // stick.gameObject.SetActive(false);
              stickFingerId = -1;
              // fingerIndicator.position = stick.position;
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

    TrackingManager.tm.firstPlayLog("5_GameStart");
    Invoke("enableMoveBoundary", 0.5f);
  }

  void enableMoveBoundary() {
    moveBoundary.SetActive(true);
  }

  void replay() {
    tutoStatus = 1;
    tutorialScenes[tutorialScenes.Length - 1].SetActive(false);
    tutorialScenes[0].SetActive(true);
    tutoLoaded = true;
    tutoCollider.enabled = false;

    foreach (Transform tr in tutoParts.transform) {
      tr.gameObject.SetActive(true);
    }

    Player.pl.getPartsText.reset();
    Player.pl.useBoosterText.reset();
    RhythmManager.rm.stopBeat();

    Invoke("enableTutoCollider", 2);
  }
}

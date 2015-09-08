﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeforeIdle : MonoBehaviour {
  public SpawnManager spawnManager;
  public MeshRenderer titleFilter;
  public GameObject title;

  public GameObject character;
  private Color characterColor;
  public float characterMovingDuration = 0.6f;
  private float characterPosX;
  public float characterMoveStart = -800;
  private float characterMoveDistance;
  private bool characterMoving = false;
  public float characterStayDuration = 0.2f;
  private float characterStayCount = 0;
  private bool boosterPlayed = false;
  private float boosterStayCount = 0;

  public Text tips;
  public float tipShowAfter = 0.3f;
  public float tipAlphaChangeDuration = 0.2f;
  private float tipHideCount = 0;
  private Color tipColor;
  private bool showTip = false;

  public float stayDuration = 0.5f;
  private float stayCount = 0;

  public float filterChangingDuration = 0.3f;
  private bool filterChanging = false;
  private Color filterColor;
  private float filterChangeTo;

  public float movingDuration = 0.6f;
  private bool titleMoving = false;
  private float titlePosX;
  private float titleMoveTo;
  private float distance;

  public float boosterStayDuration = 0.3f;

	void Start() {
    filterColor = titleFilter.material.color;
    filterChanging = true;
    filterChangeTo = 0;
    stayCount = 0;

    showTip = true;
    tipColor = tips.color;
    characterColor = character.GetComponent<Renderer>().material.color;

    changeCharacter(PlayerPrefs.GetString("SelectedCharacter"));

    tips.text = tips.transform.GetChild(DataManager.dm.getInt("LastTipIndex")).GetComponent<Tip>().description;

    spawnManager.preload();
  }

  void Update() {
    if (filterChanging) {
      if (stayCount < stayDuration) {
        stayCount += Time.deltaTime;
      } else {
        filterColor.a = Mathf.MoveTowards(filterColor.a, filterChangeTo, Time.deltaTime / filterChangingDuration);
        titleFilter.material.color = filterColor;

        if (showTip) {
          tipColor.a = Mathf.MoveTowards(tipColor.a, 0, Time.deltaTime / filterChangingDuration);
          tips.color = tipColor;
        }

        if (filterChangeTo == 0) {
          characterColor.a = Mathf.MoveTowards(characterColor.a, filterChangeTo, Time.deltaTime / filterChangingDuration);
          character.GetComponent<Renderer>().material.color = characterColor;
        }

        if (filterColor.a == filterChangeTo) {
          filterChanging = false;
          showTip = false;
          if (filterChangeTo == 0) titleFilter.gameObject.SetActive(false);
        }
      }
    }

    if (titleMoving) {
      titlePosX = Mathf.MoveTowards(titlePosX, titleMoveTo, Time.deltaTime / movingDuration * distance);
      title.GetComponent<RectTransform>().anchoredPosition = new Vector2(titlePosX, title.GetComponent<RectTransform>().anchoredPosition.y);

      if (titlePosX == titleMoveTo) {
        titleMoving = false;
      }
    }

    if (characterMoving) {
      if (characterStayCount < characterStayDuration) {
        characterStayCount += Time.deltaTime;
      } else {
        if (!boosterPlayed) {
          boosterPlayed = true;
          character.transform.Find("Booster").GetComponent<AudioSource>().Play();
          character.transform.Find("Booster").GetComponent<ParticleSystem>().Play();
        }

        characterPosX = Mathf.MoveTowards(characterPosX, 0, Time.deltaTime / characterMovingDuration * characterMoveDistance);
        character.GetComponent<RectTransform>().anchoredPosition = new Vector2(characterPosX, character.GetComponent<RectTransform>().anchoredPosition.y);

        if (characterPosX == 0) {
          if (boosterStayCount < boosterStayDuration) {
            boosterStayCount += Time.deltaTime;
          } else {
            characterMoving = false;
            Application.LoadLevel(Application.loadedLevel);
          }
        }
      }

      if (tipHideCount < tipShowAfter) {
        tipHideCount += Time.deltaTime;
      } else {
        tipColor.a = Mathf.MoveTowards(tipColor.a, 1, Time.deltaTime / tipAlphaChangeDuration);
        tips.color = tipColor;
      }
    }
  }

  public bool isLoading() {
    return filterChanging || characterMoving;
  }

  public void moveTitle(bool hiding = true) {
    if (hiding) {
      titleMoveTo = 800;
      titlePosX = title.GetComponent<RectTransform>().anchoredPosition.x;
    } else {
      titleMoveTo = 0;
      titlePosX = -title.GetComponent<RectTransform>().anchoredPosition.x;
    }
    titleMoving = true;
    distance = Mathf.Abs(titlePosX - titleMoveTo);
  }

  public void playAgain() {
    titleFilter.gameObject.SetActive(true);
    filterColor = titleFilter.material.color;
    filterChanging = true;
    filterChangeTo = 1;
    moveTitle(false);

    changeCharacter(PlayerPrefs.GetString("SelectedCharacter"));
    characterMoving = true;
    characterPosX = characterMoveStart;
    characterPosX = characterMoveStart;
    characterMoveDistance = Mathf.Abs(characterMoveStart);
    character.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
    character.GetComponent<RectTransform>().anchoredPosition = new Vector2(characterPosX, character.GetComponent<RectTransform>().anchoredPosition.y);

    Tip[] availableTips = new Tip[tips.transform.childCount];
    int availableTipsCount = 0;
    foreach (Transform tr in tips.transform) {
      Tip tip = tr.GetComponent<Tip>();
      if (tip.isAvailable()) {
        availableTips[availableTipsCount++] = tip;
      }
    }

    Tip selected = availableTips[Random.Range(0, availableTipsCount)];
    tips.text = selected.description;
    DataManager.dm.setInt("LastTipIndex", int.Parse(selected.name));
  }

  void changeCharacter(string characterName) {
    GameObject play_characters = Resources.Load<GameObject>("_characters/play_characters");
    character.GetComponent<MeshFilter>().sharedMesh = play_characters.transform.FindChild(characterName).GetComponent<MeshFilter>().sharedMesh;
  }
}

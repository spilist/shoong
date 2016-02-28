﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhaseManager : MonoBehaviour {
  public static PhaseManager pm;

  public string[] textPerLevel;
  public int[] reqProgressPerLevel;

  public SmallAsteroidManager smallAsteroidM;
  public AsteroidManager asteroidM;
  public IceDebrisManager icm;
  public ComboPartsManager cpm;

  public RubberBallManager rbm;
  public RubberBallBiggerManager rbbm;
  public RainbowDonutsManager rdm;

  public PhaseMonsterManager pmm;
  public BlackMonsterManager bmm;
  public ConfusedMonsterManager cmm;

  public MeteroidManager mtm;
  // public BiggerMeteroidManager bmtm;
  // public BiggerMeteroidManager2 bmtm2;
  public SummonPartsManager spm;

  public DangerousEMPManager dem;
  public BlackholeManager blm;
  public EMPManager em;

  public AlienshipManager asm;

  public RectTransform stageIndicatorTop;
  public RectTransform stageIndicatorBottom;

  public int startTopPos = -895;
  public int slowTopPos = 5;
  public int fastTopPos = 45;
  public int endTopPos = 945;

  public int startBottomPos = 945;
  public int slowBottomPos = 45;
  public int fastBottomPos = 5;
  public int endBottomPos = -895;

  public int phaseChangingStatus = 0;
  public float indicatorPauseDuration = 0.5f;
  public float indicatorFastDuration = 0.1f;
  public float indicatorSlowDuration = 0.8f;
  private float indicatorTopXPos;
  private float indicatorBottomXPos;

  public GameObject levelClearEffect;
  public Text currentLevelText;
  public Text nextLevelText;
  private int level;

  void Awake() {
    pm = this;
  }

  void Start() {
    level = -1;
  }

  public int phase() {
    return level;
  }

	public bool nextPhase() {
    level++;
    if (level > textPerLevel.Length) {
      level = textPerLevel.Length;
      return false;
    }

    StartCoroutine("showNextLevel");
    if (level == 0) return true;

    string levelName = textPerLevel[level - 1];

    switch(levelName) {
      case "1-2":
      asteroidM.enabled = true;
      cpm.adjustForLevel(2);
      break;
      case "1-3":
      icm.enabled = true;
      cpm.adjustForLevel(3);
      break;
      case "2-1":
      rbm.enabled = true;
      rdm.enabled = true;
      rdm.adjustForLevel(1);
      break;
      case "2-2":
      rbm.enabled = false;
      TimeManager.time.startRubberBall(false);
      rbbm.enabled = true;
      rdm.adjustForLevel(2);
      break;
      case "2-3":
      rbbm.setMany(true);
      rdm.adjustForLevel(3);
      break;
      case "3-1":
      rbm.enabled = true;
      TimeManager.time.startRubberBall();
      rbbm.enabled = false;
      TimeManager.time.startRubberBallBigger(false);
      pmm.enabled = true;
      break;
      case "3-2":
      bmm.enabled = true;
      break;
      case "3-3":
      cmm.enabled = true;
      break;
      case "4-1":
      mtm.enabled = true;
      cpm.enabled = false;
      bmm.enabled = false;
      bmm.stopRespawn();
      cmm.enabled = false;
      cmm.stopRespawn();
      spm.enabled = true;
      spm.adjustForLevel(1);
      break;
      case "4-2":
      mtm.enabled = false;
      // bmtm.enabled = true;
      spm.adjustForLevel(2);
      break;
      case "4-3":
      // bmtm.enabled = true;
      // bmtm2.enabled = true;
      spm.adjustForLevel(3);
      break;
      case "5-1":
      // bmtm2.enabled = false;
      spm.enabled = false;
      em.enabled = true;
      em.adjustForLevel(1);
      dem.enabled = true;
      break;
      case "5-2":
      blm.enabled = true;
      em.adjustForLevel(2);
      break;
      case "5-3":
      dem.startLarger();
      em.adjustForLevel(3);
      break;
      case "6":
      asm.enabled = true;
      break;
    }

    return true;
  }

  private IEnumerator showNextLevel() {
    if (level > 0) {
      // EnergyManager.em.getFullHealth();
      levelClearEffect.SetActive(true);
    }

    yield return new WaitForSeconds(indicatorPauseDuration);

    phaseChangingStatus = 1;
    indicatorTopXPos = startTopPos;
    indicatorBottomXPos = startBottomPos;

    if (level > 0) {
      currentLevelText.text = levelText(level);
      nextLevelText.text = levelText(level + 1);
      TimeManager.time.resetProgressCharacter();
    }

    if (level == textPerLevel.Length) {
      stageIndicatorTop.GetComponent<Text>().text = "final";
      stageIndicatorBottom.GetComponent<Text>().text = "level";
      nextLevelText.text = "final";
    } else {
      // stageIndicatorTop.GetComponent<Text>().text = "level " + (level + 1).ToString();
      stageIndicatorBottom.GetComponent<Text>().text = levelText(level);
    }
  }

  private string levelText(int level) {
    return (level/3 + 1).ToString() + "-" + (level%3 + 1).ToString();
  }

  void Update() {
    if (phaseChangingStatus > 0) {

      if (phaseChangingStatus == 1) {
        indicatorTopXPos = Mathf.MoveTowards(indicatorTopXPos, slowTopPos, Time.deltaTime * Mathf.Abs(startTopPos - slowTopPos) / indicatorFastDuration);
        indicatorBottomXPos = Mathf.MoveTowards(indicatorBottomXPos, slowBottomPos, Time.deltaTime * Mathf.Abs(startBottomPos - slowBottomPos) / indicatorFastDuration);
        if (indicatorTopXPos == slowTopPos) {
          phaseChangingStatus++;
        }
      } else if (phaseChangingStatus == 2) {
        indicatorTopXPos = Mathf.MoveTowards(indicatorTopXPos, fastTopPos, Time.deltaTime * Mathf.Abs(fastTopPos - slowTopPos) / indicatorSlowDuration);
        indicatorBottomXPos = Mathf.MoveTowards(indicatorBottomXPos, fastBottomPos, Time.deltaTime * Mathf.Abs(fastBottomPos - slowBottomPos) / indicatorSlowDuration);

        if (indicatorTopXPos == fastTopPos) {
          phaseChangingStatus++;
        }
      } else if (phaseChangingStatus == 3) {
        indicatorTopXPos = Mathf.MoveTowards(indicatorTopXPos, endTopPos, Time.deltaTime * Mathf.Abs(fastTopPos - endTopPos) / indicatorFastDuration);
        indicatorBottomXPos = Mathf.MoveTowards(indicatorBottomXPos, endBottomPos, Time.deltaTime * Mathf.Abs(fastBottomPos - endBottomPos) / indicatorFastDuration);
        if (indicatorTopXPos == endTopPos) phaseChangingStatus = 0;
      }
      stageIndicatorTop.anchoredPosition = new Vector2(indicatorTopXPos, stageIndicatorTop.anchoredPosition.y);
      stageIndicatorBottom.anchoredPosition = new Vector2(indicatorBottomXPos, stageIndicatorBottom.anchoredPosition.y);
    }
  }
}

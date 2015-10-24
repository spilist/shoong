using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhaseManager : MonoBehaviour {
  public static PhaseManager pm;

  public GoldCubesCount gcCount;
  public IceDebrisManager icm;
  public PhaseMonsterManager pmm;
  public NormalPartsManager npm;
  public AsteroidManager atm;
  public SmallAsteroidManager ssm;
  public Renderer EMFilter;
  public float EMFilterTargetAlpha = 0.11f;
  private Color EMFilterColor;
  private float EMFilterAlpha;

  public MeteroidManager ntm;
  public Renderer meteroidFilter;
  public float meteroidFilterAlpha1 = 0.08f;
  public float meteroidFilterAlpha2 = 0.12f;
  public float meteroidFilterChangeDuration1 = 3;
  public float meteroidFilterChangeDuration2 = 2;
  private bool meteroidFilterAlphaGoingUp = true;
  private float meteroidFilterTargetAlpha;
  private float meteroidFilterChangeDuration;
  private float meteroidFilterAlpha = 0;
  private Color meteroidFilterColor;

  public DangerousEMPManager dem;
  public BlackholeManager blm;
  public AlienshipManager asm;

  public RectTransform phaseIndicator;
  public Text phaseText;
  public string[] textPerLevel;
  public float originalPosX = -220;
  public float showPosX = -480;

  public float showAfter = 1;
  public float changingDuration = 0.5f;
  public float showDuration = 5;

  private int level;
  private int status = 0;
  private float stayCount;
  private Vector2 pos;
  private float posX;
  private float diff;

  void Awake() {
    pm = this;
  }

  void Start() {
    level = 0;
    diff = Mathf.Abs(showPosX - originalPosX);
  }

  public int phase() {
    return level;
  }

	public void nextPhase() {
    level++;
    status++;
    if (level > textPerLevel.Length) {
      level = textPerLevel.Length;
    }

    string levelName = textPerLevel[level - 1];
    phaseText.text = levelName;
    pos = phaseIndicator.anchoredPosition;
    posX = originalPosX;
    stayCount = 0;

    if (levelName == "IceDebris") {
      icm.enabled = true;
    } else if (levelName == "Minimon 1") {
      pmm.enabled = true;
    } else if (levelName == "Blakchole") {
      blm.enabled = true;
    } else if (levelName == "Meteroid") {
      ntm.enabled = true;
    } else if (levelName == "Bomb") {
      dem.enabled = true;
    } else if (levelName == "UFO") {
      asm.enabled = true;
    } else if (levelName == "BigBomb") {
      dem.startLarger();
    } else if (levelName == "자기장 불안정화") {
      npm.startPhase();
      atm.startPhase();
      ssm.startPhase();
      EMFilter.gameObject.SetActive(true);
      EMFilterColor = EMFilter.material.GetColor("_TintColor");
    } else if (levelName == "유성 거대화") {
      ntm.startPhase();
      meteroidFilter.gameObject.SetActive(true);
      meteroidFilterColor = meteroidFilter.material.GetColor("_TintColor");
      meteroidFilterTargetAlpha = meteroidFilterAlpha1;
      meteroidFilterChangeDuration = meteroidFilterChangeDuration1;
    } else if (levelName == "폭발 위험 지역") {
      dem.enabled = true;
    } else if (levelName == "유성우") {
      ntm.startSecond();
      meteroidFilterTargetAlpha = meteroidFilterAlpha2;
      meteroidFilterChangeDuration = meteroidFilterChangeDuration2;
    }
  }

  void Update() {
    if (status == 1) {
      if (stayCount < showAfter) stayCount += Time.deltaTime;
      else {
        stayCount = 0;
        gcCount.add(1);
        status++;
      }
    } else if (status == 2) {
      posX = Mathf.MoveTowards(posX, showPosX, Time.deltaTime * diff / changingDuration);
      pos.x = posX;
      phaseIndicator.anchoredPosition = pos;
      if (posX == showPosX) status++;
    } else if (status == 3) {
      if (stayCount < showDuration) stayCount += Time.deltaTime;
      else {
        stayCount = 0;
        status++;
      }
    } else if (status == 4) {
      posX = Mathf.MoveTowards(posX, originalPosX, Time.deltaTime * diff / changingDuration);
      pos.x = posX;
      phaseIndicator.anchoredPosition = pos;
      if (posX == originalPosX) {
        status = 0;
      }
    }

    if (EMFilter.gameObject.activeSelf) {
      EMFilterAlpha = Mathf.MoveTowards(EMFilterAlpha, EMFilterTargetAlpha, Time.deltaTime * EMFilterTargetAlpha);
      EMFilterColor.a = EMFilterAlpha;
      EMFilter.sharedMaterial.SetColor("_TintColor", EMFilterColor);
    }

    if (meteroidFilter.gameObject.activeSelf) {
      if (meteroidFilterAlphaGoingUp) {
        meteroidFilterAlpha = Mathf.MoveTowards(meteroidFilterAlpha, meteroidFilterTargetAlpha, Time.deltaTime * meteroidFilterTargetAlpha / meteroidFilterChangeDuration);
        meteroidFilterColor.a = meteroidFilterAlpha;

        meteroidFilter.sharedMaterial.SetColor("_TintColor", meteroidFilterColor);

        if (meteroidFilterAlpha == meteroidFilterTargetAlpha) meteroidFilterAlphaGoingUp = false;
      } else {
        meteroidFilterAlpha = Mathf.MoveTowards(meteroidFilterAlpha, 0, Time.deltaTime * meteroidFilterTargetAlpha / meteroidFilterChangeDuration);
        meteroidFilterColor.a = meteroidFilterAlpha;

        meteroidFilter.sharedMaterial.SetColor("_TintColor", meteroidFilterColor);
        if (meteroidFilterAlpha == 0) meteroidFilterAlphaGoingUp = true;
      }
    }
  }

  void OnDisable() {
    if (meteroidFilter != null && EMFilter.gameObject.activeSelf) {
      EMFilterColor.a = 0;
      EMFilter.sharedMaterial.SetColor("_TintColor", EMFilterColor);
    }

    if (meteroidFilter != null && meteroidFilter.gameObject.activeSelf) {
      meteroidFilterColor.a = 0;
      meteroidFilter.sharedMaterial.SetColor("_TintColor", meteroidFilterColor);
    }
  }

  public int getPhaseBonus() {
    // int bonus = 0;
    // for (int i = 0; i <= level; i++) bonus += i;
    return level;
  }
}

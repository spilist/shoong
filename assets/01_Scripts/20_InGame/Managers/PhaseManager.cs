using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhaseManager : MonoBehaviour {
  public GoldCubesCount gcCount;
  public NormalPartsManager npm;
  public Renderer EMFilter;
  public float EMFilterTargetAlpha = 0.11f;
  private Color EMFilterColor;
  private float EMFilterAlpha;

  public MeteroidManager ntm;
  public MeteroidManager2 ntm2;
  public MeteroidManager3 ntm3;
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
    if (level > 6) {
      asm.run();
      level = 6;
    }

    phaseText.text = textPerLevel[level - 1];
    pos = phaseIndicator.anchoredPosition;
    posX = originalPosX;
    stayCount = 0;

    // if (level < 6) AudioManager.am.setPitch(level);

    if (level == 1) {
      ntm.enabled = true;
    } else if (level == 2) {
      npm.startPhase();
      EMFilter.gameObject.SetActive(true);
      EMFilterColor = EMFilter.material.GetColor("_TintColor");
    } else if (level == 3) {
      ntm.startPhase();
      meteroidFilter.gameObject.SetActive(true);
      meteroidFilterColor = meteroidFilter.material.GetColor("_TintColor");
      meteroidFilterTargetAlpha = meteroidFilterAlpha1;
      meteroidFilterChangeDuration = meteroidFilterChangeDuration1;
    } else if (level == 4) {
      dem.enabled = true;
    } else if (level == 5) {
      ntm2.enabled = true;
      ntm3.enabled = true;
      meteroidFilterTargetAlpha = meteroidFilterAlpha2;
      meteroidFilterChangeDuration = meteroidFilterChangeDuration2;
    } else if (level == 6) {
      asm.enabled = true;
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

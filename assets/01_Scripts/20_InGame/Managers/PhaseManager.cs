using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhaseManager : MonoBehaviour {
  public NormalPartsManager npm;
  public GameObject EMFilter;

  public MeteroidManager ntm;
  public MeteroidManager2 ntm2;
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

	public void nextPhase() {
    level++;
    status++;
    phaseText.text = textPerLevel[level - 1];
    pos = phaseIndicator.anchoredPosition;
    posX = originalPosX;
    stayCount = 0;

    if (level < 6) AudioManager.am.setPitch(level);

    if (level == 1) {
      npm.startPhase();
      EMFilter.SetActive(true);
    } else if (level == 2) {
      ntm.startPhase();
      meteroidFilter.gameObject.SetActive(true);
      meteroidFilterColor = meteroidFilter.material.GetColor("_TintColor");
      meteroidFilterTargetAlpha = meteroidFilterAlpha1;
      meteroidFilterChangeDuration = meteroidFilterChangeDuration1;
    } else if (level == 3) {
      dem.enabled = true;
    } else if (level == 4) {
      ntm2.enabled = true;
      meteroidFilterTargetAlpha = meteroidFilterAlpha2;
      meteroidFilterChangeDuration = meteroidFilterChangeDuration2;
    } else if (level == 5) {

    }
  }

  void Update() {
    if (status == 1) {
      if (stayCount < showAfter) stayCount += Time.deltaTime;
      else {
        stayCount = 0;
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

    if (level >= 2) {
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
    if (meteroidFilter != null && meteroidFilter.gameObject.activeSelf) {
      meteroidFilterColor.a = 0;
      meteroidFilter.material.color = meteroidFilterColor;
    }
  }
}

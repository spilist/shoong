using UnityEngine;
using System.Collections;

public class DangerousEMPMover : ObjectsMover {
  private DangerousEMPManager dem;

  Transform shellTr;

  float minScale;
  float maxScale;
  float diff;
  float scale;

  float startDuration;
  float decreaseDurationPerPulse;
  Renderer shellRenderer;
  Color color;
  Color originalColor;
  float colorCode = 0;
  float colorChangeDuration;
  bool unstable = false;
  bool scaleUp = true;

  Transform dangerousArea;

  override public string getManager() {
    return "DangerousEMPManager";
  }

  protected override void initializeRest() {
    dem = (DangerousEMPManager)objectsManager;

    canBeMagnetized = false;

    minScale = dem.minScale;
    maxScale = dem.maxScale;
    startDuration = dem.startDuration;
    decreaseDurationPerPulse = dem.decreaseDurationPerPulse;
    diff = maxScale - minScale;

    shellTr = transform.Find("Shell");
    shellRenderer = shellTr.GetComponent<Renderer>();
    originalColor = shellRenderer.sharedMaterial.GetColor("_TintColor");

    float duration = startDuration;
    while (duration > 0) {
      colorChangeDuration += duration;
      duration -= decreaseDurationPerPulse;
    }

    dangerousArea = transform.Find("DangerousArea");
  }

  protected override void afterEnable() {
    scale = minScale;
    color = originalColor;
    startDuration = dem.startDuration;
    shellRenderer.material.SetColor("_TintColor", color);
    unstable = false;
    dangerousArea.transform.eulerAngles = new Vector3(90, 0, 0);
  }

  override public void destroyObject(bool destroyEffect = false, bool byPlayer = false, bool respawn = true) {
    gameObject.SetActive(false);

    if (destroyEffect) {
      GameObject effect = dem.getPooledObj(dem.particleDestroyByPlayerPool, dem.particleDestroyByPlayer, transform.position);
      effect.SetActive(true);
    }

    if (byPlayer) {
      showDestroyEffect(true);
      player.destroyObject(tag);
    }
  }

  void Update() {
    if (!unstable) return;

    if (startDuration <= 0) {
      unstable = false;
      destroyObject(false, true);
    }

    colorCode = Mathf.MoveTowards(colorCode, 1, Time.deltaTime / colorChangeDuration);
    color.g = colorCode;
    color.b = colorCode;
    shellRenderer.material.SetColor("_TintColor", color);

    if (scaleUp) {
      scale = Mathf.MoveTowards(scale, maxScale, Time.deltaTime * diff * 2 / startDuration);
      shellTr.localScale = scale * Vector3.one;
      if (scale == maxScale) scaleUp = false;
    } else {
      scale = Mathf.MoveTowards(scale, minScale, Time.deltaTime * diff * 2 / startDuration);
      shellTr.localScale = scale * Vector3.one;
      if (scale == minScale) {
        scaleUp = true;
        startDuration -= decreaseDurationPerPulse;
      }
    }
  }

  public void unstabilize() {
    unstable = true;
  }
}

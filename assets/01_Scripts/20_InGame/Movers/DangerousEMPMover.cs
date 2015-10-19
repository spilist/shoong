using UnityEngine;
using System.Collections;

public class DangerousEMPMover : ObjectsMover {
  private SpecialPartsManager spm;
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
  float colorCode = 0;
  float colorChangeDuration;
  bool unstable = false;
  bool scaleUp = true;

  override public string getManager() {
    return "DangerousEMPManager";
  }

  protected override void initializeRest() {
    dem = (DangerousEMPManager)objectsManager;
    spm = GameObject.Find("Field Objects").GetComponent<SpecialPartsManager>();

    canBeMagnetized = false;

    minScale = dem.minScale;
    maxScale = dem.maxScale;
    startDuration = dem.startDuration;
    decreaseDurationPerPulse = dem.decreaseDurationPerPulse;
    diff = maxScale - minScale;

    scale = minScale;

    shellTr = transform.Find("Shell");
    shellRenderer = shellTr.GetComponent<Renderer>();
    color = shellRenderer.sharedMaterial.GetColor("_TintColor");

    float duration = startDuration;
    while (duration > 0) {
      colorChangeDuration += duration;
      duration -= decreaseDurationPerPulse;
    }

    unstable = true;
  }

  override public void destroyObject(bool destroyEffect = true, bool byPlayer = false) {
    foreach (Collider collider in GetComponents<Collider>()) {
      collider.enabled = false;
    }
    Destroy(gameObject);

    if (byPlayer) {
      player.destroyObject(tag, gaugeWhenDestroy());
      Instantiate(dem.particleDestroyByPlayer, transform.position, Quaternion.identity);
    } else if (destroyEffect) {
      Instantiate(objectsManager.objDestroyEffect, transform.position, transform.rotation);
    }
  }

  override public int bonusCubes() {
    return player.isUnstoppable()? (int) (cubesWhenEncounter() * spm.bonus) : 0;
  }

  void Update() {
    if (!unstable) return;

    if (startDuration <= 0) {
      unstable = false;
      destroyObject();
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

  override public int cubesWhenDestroy() {
    return cubesWhenEncounter();
  }
}

using UnityEngine;
using System.Collections;

public class DoppleMover : ObjectsMover {
  DoppleManager dpm;
  float teleportingDuration;
  int teleportingStatus = 0;
  Vector3 teleportTo;
  Color color;
  float alphaOrigin;
  float alpha = 0;
  float stayCount = 0;
  Renderer mRenderer;
  float radius;

  override protected void initializeRest() {
    dpm = (DoppleManager) objectsManager;
    canBeMagnetized = false;
    mRenderer = GetComponent<Renderer>();
    radius = dpm.blinkRadius;
    teleportingDuration = dpm.teleportingDuration;
    color = dpm.originalColor;
    alpha = color.a;
    alphaOrigin = alpha;
    blink();
  }

  override public string getManager() {
    return "DoppleManager";
  }

  override protected void afterEncounter() {
    if (player.isOnPowerBoost()) return;

    Instantiate(dpm.forceFieldPrefab, transform.position, Quaternion.identity);
    Camera.main.GetComponent<CameraMover>().setSlowly(true);
  }

  void blink() {
    stayCount = 0;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    Vector3 currentV = transform.position;
    teleportTo = new Vector3(currentV.x + randomV.x * radius, 0, currentV.z + randomV.y * radius);
    teleportingStatus = 1;
  }

  void Update() {
    if (teleportingStatus == 1) {
      alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime * alphaOrigin / teleportingDuration);
      color.a = alpha;
      mRenderer.sharedMaterial.color = color;
      if (alpha == 0) {
        transform.position = teleportTo;
        alpha = alphaOrigin / 2;
        color.a = alpha;
        mRenderer.sharedMaterial.color = color;
        AudioSource.PlayClipAtPoint(dpm.teleportSound, transform.position, dpm.teleportSoundVolume);
        teleportingStatus++;
      }
    } else if (teleportingStatus == 2) {
      if (stayCount < teleportingDuration) stayCount += Time.deltaTime;
      else {
        alpha = 0;
        color.a = alpha;
        mRenderer.sharedMaterial.color = color;
        Instantiate(dpm.forceFieldByDopplePrefab, teleportTo, Quaternion.identity);
        teleportingStatus++;
        stayCount = 0;
      }
    } else if (teleportingStatus == 3) {
      alpha = Mathf.MoveTowards(alpha, alphaOrigin, Time.deltaTime * alphaOrigin / teleportingDuration);
      color.a = alpha;
      mRenderer.sharedMaterial.color = color;
      if (alpha == alphaOrigin) teleportingStatus++;
    } else if (teleportingStatus == 4) {
      if (stayCount < dpm.blinkInterval) stayCount += Time.deltaTime;
      else {
        teleportingStatus = 0;
        blink();
      }
    }
  }
}

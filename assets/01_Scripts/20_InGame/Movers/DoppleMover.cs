using UnityEngine;
using System.Collections;

public class DoppleMover : ObjectsMover {
  DoppleManager dpm;
  float stayCount = 0;
  float radius;
  AudioSource teleportingSound;

  override protected void initializeRest() {
    dpm = (DoppleManager) objectsManager;
    radius = dpm.blinkRadius;
    teleportingSound = GetComponent<AudioSource>();
  }

  override public string getManager() {
    return "DoppleManager";
  }

  override protected void afterEncounter() {
    if (player.isOnSuperheat()) return;

    Instantiate(dpm.forceFieldPrefab, transform.position, Quaternion.identity);
    Camera.main.GetComponent<CameraMover>().setSlowly(true);
  }

  void blink() {
    stayCount = 0;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    Vector3 currentV = transform.position;
    Vector3 teleportTo = new Vector3(currentV.x + randomV.x * radius, 0, currentV.z + randomV.y * radius);
    transform.position = teleportTo;
    teleportingSound.Play();
    Instantiate(dpm.forceFieldByDopplePrefab, teleportTo, Quaternion.identity);
  }

  void Update() {
    if (stayCount < dpm.blinkInterval) {
      stayCount += Time.deltaTime;
    } else {
      blink();
    }
  }

  override public int cubesWhenDestroy() {
    return 50;
  }
}

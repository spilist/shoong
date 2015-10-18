using UnityEngine;
using System.Collections;

public class DoppleManager : ObjectsManager {
  public ParticleSystem getEnergy;
  public GameObject energyCube;
  public GameObject forceFieldPrefab;
  public GameObject forceFieldByDopplePrefab;
  public float[] forceFieldSizePerLevel;
  public float targetSize;
  public AudioClip teleportSound;
  public float teleportSoundVolume = 0.5f;
  public AudioClip cannotTeleportWarningSound;
  public float waveAwakeDuration = 0.3f;

  public float blinkInterval = 3;
  public int blinkRadius = 50;

  override public void initRest() {
  }

  override public void adjustForLevel(int level) {
    targetSize = forceFieldSizePerLevel[level];
  }

  public float getTargetSize(bool byPlayer) {
    if (byPlayer) return targetSize;
    else return forceFieldSizePerLevel[0];
  }

}

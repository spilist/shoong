using UnityEngine;
using System.Collections;

public class BlackholeManager : ObjectsManager {
  public GameObject blackholeGravitySpherePrefab;
  public GameObject blackholeGravity;

  public int gravity = 50;
  public int pullUser = 50;

  public int[] radiusPerLevel;
  public float[] lifetimePerLevel;

  public int reboundDuring = 2;
  public float shakeAmount = 3;

  override public void initRest() {
    objEncounterEffectForPlayer.transform.localScale = Vector3.one * radiusPerLevel[DataManager.dm.getInt("BlackholeLevel") - 1] / radiusPerLevel[0];
  }

  override protected void afterSpawn() {
    blackholeGravity = (GameObject) Instantiate(blackholeGravitySpherePrefab, instance.transform.position, Quaternion.Euler(90, 0, 0));
    blackholeGravity.transform.parent = transform;
  }

  public Vector3 headingToBlackhole(Transform tr) {
    Vector3 heading = instance.transform.position - tr.position;
    return heading / heading.magnitude;
  }
}

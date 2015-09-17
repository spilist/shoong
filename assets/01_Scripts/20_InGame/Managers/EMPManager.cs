using UnityEngine;
using System.Collections;

public class EMPManager : ObjectsManager {
  public int[] forceFieldRadiusPerLevel;
  public int[] cameraEnlargeSizePerLevel;

  public int targetRadius;
  public int enlargeSize;

  public float enlargeDuration = 3;
  public float stayDuration = 1;
  public float shrinkDuration = 1;

  private GameObject forceField;

  private float radius;
  private float cameraSize;
  private float targetSize;
  private int status;
  private float stayCount;

	override public void initRest() {
    int level = DataManager.dm.getInt("EMPLevel") - 1;

    level = 2;

    targetRadius = forceFieldRadiusPerLevel[level];
    enlargeSize = cameraEnlargeSizePerLevel[level];

    skipInterval = true;
    forceField = strengthenPlayerEffect;
  }

  override protected void afterSpawn() {
    status = 0;
    radius = 1;
    stayCount = 0;
    cameraSize = Camera.main.orthographicSize;
    targetSize = cameraSize + enlargeSize;
  }

  public void generateForceField() {
    status = 1;
  }

  void Update() {
    if (status == 1) {
      radius = Mathf.MoveTowards(radius, targetRadius, Time.deltaTime * targetRadius / enlargeDuration);

      forceField.transform.localScale = radius * Vector3.one;
      forceField.transform.Find("Halo").GetComponent<Light>().range = radius;

      cameraSize = Mathf.MoveTowards(cameraSize, targetSize, Time.deltaTime * enlargeSize / enlargeDuration);
      Camera.main.orthographicSize = cameraSize;

      if (radius == targetRadius) status = 2;
    } else if (status == 2) {
      if (stayCount < stayDuration) stayCount += Time.deltaTime;
      else {
        run();
        player.stopEMP();
        targetSize = cameraSize - enlargeSize;
        status = 3;
      }
    } else if (status == 3) {
      cameraSize = Mathf.MoveTowards(cameraSize, targetSize, Time.deltaTime * enlargeSize / shrinkDuration);
      Camera.main.orthographicSize = cameraSize;
      if (cameraSize == targetSize) {
        status = 0;
      }
    }
  }
}

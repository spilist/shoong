using UnityEngine;
using System.Collections;

public class EMPManager : ObjectsManager {
  public Superheat superheat;
  public int guageAmount = 20;

  public int[] forceFieldRadiusPerLevel;
  public int[] cameraEnlargeSizePerLevel;
  public float[] cameraShakeAmountPerLevel;

  public int targetRadius;
  public int enlargeSize;
  public float shakeAmount;

  public int fieldRotateSpeed = 300;
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

    targetRadius = forceFieldRadiusPerLevel[level];
    enlargeSize = cameraEnlargeSizePerLevel[level];
    shakeAmount = cameraShakeAmountPerLevel[level];

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
    Camera.main.GetComponent<CameraMover>().shakeUntilStop(shakeAmount);
    status = 1;
    superheat.addGuageWithEffect(guageAmount);
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
      if (stayCount < stayDuration) {
        stayCount += Time.deltaTime;
      } else {
        targetSize = cameraSize - enlargeSize;
        status = 3;
      }
    } else if (status == 3) {
      radius = Mathf.MoveTowards(radius, 1, Time.deltaTime * targetRadius / shrinkDuration);

      forceField.transform.localScale = radius * Vector3.one;
      forceField.transform.Find("Halo").GetComponent<Light>().range = radius;

      cameraSize = Mathf.MoveTowards(cameraSize, targetSize, Time.deltaTime * enlargeSize / shrinkDuration);
      Camera.main.orthographicSize = cameraSize;

      if (cameraSize == targetSize) {
        run();
        player.stopEMP();
        Camera.main.GetComponent<CameraMover>().stopShake();
        status = 0;
      }
    }
  }
}

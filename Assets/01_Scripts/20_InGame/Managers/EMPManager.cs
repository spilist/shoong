using UnityEngine;
using System.Collections;

public class EMPManager : ObjectsManager {
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
  private float targetCameraSize;
  private int status;
  private float stayCount;

  public int chanceBase = 200;
  public int goldenChance = 1;
  public int goldCubesGet = 1;
  public bool isGolden = false;
  private Material shellMat;

  private int levelChangeNeeded = 0;

	override public void initRest() {
    skipInterval = true;
    forceField = strengthenPlayerEffect;
    adjustForLevel(1);
    run();
  }

  override public void adjustForLevel(int level) {
    if (status > 0) {
      levelChangeNeeded = level;
      return;
    }

    levelChangeNeeded = 0;

    targetRadius = forceFieldRadiusPerLevel[level - 1];
    enlargeSize = cameraEnlargeSizePerLevel[level - 1];
    shakeAmount = cameraShakeAmountPerLevel[level - 1];
  }

  override protected void afterSpawn() {
    status = 0;
    radius = 1;
    stayCount = 0;
    cameraSize = Camera.main.orthographicSize;

    int random = Random.Range(0, chanceBase);
    if (random < goldenChance) {
      isGolden = true;

      instance.transform.Find("GoldenShell").gameObject.SetActive(true);
      instance.transform.Find("GoldenCore").gameObject.SetActive(true);
      instance.transform.Find("GoldenParticles").gameObject.SetActive(true);

      instance.transform.Find("BasicShell").gameObject.SetActive(false);
      instance.transform.Find("BasicCore").gameObject.SetActive(false);
      instance.transform.Find("BasicParticles").gameObject.SetActive(false);

      shellMat = instance.transform.Find("GoldenShell").GetComponent<Renderer>().sharedMaterial;
    } else {
      isGolden = false;

      instance.transform.Find("GoldenShell").gameObject.SetActive(false);
      instance.transform.Find("GoldenCore").gameObject.SetActive(false);
      instance.transform.Find("GoldenParticles").gameObject.SetActive(false);

      instance.transform.Find("BasicShell").gameObject.SetActive(true);
      instance.transform.Find("BasicCore").gameObject.SetActive(true);
      instance.transform.Find("BasicParticles").gameObject.SetActive(true);
      shellMat = instance.transform.Find("BasicShell").GetComponent<Renderer>().sharedMaterial;
    }
  }

  public void generateForceField() {
    Camera.main.GetComponent<CameraMover>().shakeUntilStop(shakeAmount);
    forceField.GetComponent<ForceField>().setProperty(shellMat, isGolden);
    targetCameraSize = cameraSize + enlargeSize;

    status = 1;
  }

  void Update() {
    if (status == 1) {
      radius = Mathf.MoveTowards(radius, targetRadius, Time.deltaTime * targetRadius / enlargeDuration);

      forceField.transform.localScale = radius * Vector3.one;
      forceField.transform.Find("Halo").GetComponent<Light>().range = radius;

      cameraSize = Mathf.MoveTowards(cameraSize, targetCameraSize, Time.deltaTime * enlargeSize / enlargeDuration);
      // Camera.main.orthographicSize = cameraSize;

      if (radius == targetRadius) status = 2;
    } else if (status == 2) {
      if (stayCount < stayDuration) {
        stayCount += Time.deltaTime;
      } else {
        targetCameraSize = cameraSize - enlargeSize;
        status = 3;
      }
    } else if (status == 3) {
      radius = Mathf.MoveTowards(radius, 1, Time.deltaTime * targetRadius / shrinkDuration);

      forceField.transform.localScale = radius * Vector3.one;
      forceField.transform.Find("Halo").GetComponent<Light>().range = radius;

      cameraSize = Mathf.MoveTowards(cameraSize, targetCameraSize, Time.deltaTime * enlargeSize / shrinkDuration);
      // Camera.main.orthographicSize = cameraSize;

      if (cameraSize == targetCameraSize) {
        run();
        player.stopEMP();
        Camera.main.GetComponent<CameraMover>().stopShake();
        status = 0;
        if (levelChangeNeeded != 0) {
          adjustForLevel(levelChangeNeeded);
        }
      }
    }
  }
}

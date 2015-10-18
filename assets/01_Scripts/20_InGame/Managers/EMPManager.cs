using UnityEngine;
using System.Collections;

public class EMPManager : ObjectsManager {
  public GoldCubesCount gcCount;
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

  public int chanceBase = 200;
  public int goldenChance = 1;
  public int goldCubesGet = 1;
  public int superChance = 10;
  public int guageAmountSuper = 10;
  public bool isGolden = false;
  public bool isSuper = false;
  private Material shellMat;

	override public void initRest() {
    skipInterval = true;
    forceField = strengthenPlayerEffect;
  }

  override public void adjustForLevel(int level) {
    targetRadius = forceFieldRadiusPerLevel[level];
    enlargeSize = cameraEnlargeSizePerLevel[level];
    shakeAmount = cameraShakeAmountPerLevel[level];
    if (level == 0) {
      goldenChance = 0;
      superChance = 0;
    }

    if (level == 1) {
      goldenChance = 0;
    }
  }

  override protected void afterSpawn() {
    status = 0;
    radius = 1;
    stayCount = 0;
    cameraSize = Camera.main.orthographicSize;
    targetSize = cameraSize + enlargeSize;

    int random = Random.Range(0, chanceBase);
    if (random < goldenChance) {
      isGolden = true;
      isSuper = false;
      instance.transform.Find("GoldenShell").gameObject.SetActive(true);
      instance.transform.Find("GoldenCore").gameObject.SetActive(true);
      instance.transform.Find("GoldenParticles").gameObject.SetActive(true);
      shellMat = instance.transform.Find("GoldenShell").GetComponent<Renderer>().sharedMaterial;
    } else if (random < superChance) {
      isGolden = false;
      isSuper = true;
      instance.transform.Find("HeatShell").gameObject.SetActive(true);
      instance.transform.Find("HeatCore").gameObject.SetActive(true);
      instance.transform.Find("HeatParticles").gameObject.SetActive(true);
      shellMat = instance.transform.Find("HeatShell").GetComponent<Renderer>().sharedMaterial;
    } else {
      isGolden = false;
      isSuper = false;
      instance.transform.Find("BasicShell").gameObject.SetActive(true);
      instance.transform.Find("BasicCore").gameObject.SetActive(true);
      instance.transform.Find("BasicParticles").gameObject.SetActive(true);
      shellMat = instance.transform.Find("BasicShell").GetComponent<Renderer>().sharedMaterial;
    }
  }

  public void generateForceField() {
    Camera.main.GetComponent<CameraMover>().shakeUntilStop(shakeAmount);
    status = 1;
    superheat.addGuageWithEffect(guageAmount);
    forceField.GetComponent<ForceField>().setProperty(shellMat, isSuper, isGolden);
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

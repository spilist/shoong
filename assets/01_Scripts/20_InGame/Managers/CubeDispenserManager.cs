using UnityEngine;
using System.Collections;

public class CubeDispenserManager : ObjectsManager {
  public Mesh[] brokenMeshes;
  public Mesh originalMesh;

  public int[] fullComboCountPerLevel;
  public float[] destroyAfterPerLevel;
  public int respawnInterval_min = 10;
  public int respawnInterval_max = 15;
  public float reboundDuring = 0.2f;
  public ParticleSystem destroy;
  public float pitchStart = 0.9f;
  public float pitchIncrease = 0.05f;
  public Material inactiveMat;
  public Material activeMat;
  public float shakeDurationByHit = 0.3f;
  public float shakeAmountByHit = 0.5f;
  public int originalEmissionRate = 30;

  public float decreaseEmissionAmount;
  private bool trying = false;
  public int fullComboCount;
  private float destroyAfterSeconds;

  public int chanceBase = 200;
  public Material goldenActiveMat;
  public Material goldenInactiveMat;
  public int goldenChance = 1;
  public int goldenCubeAmount = 10;
  private bool isGolden = false;

  override public void initRest() {
    skipInterval = true;
  }

  override public void adjustForLevel(int level) {
    fullComboCount = fullComboCountPerLevel[level];
    destroyAfterSeconds = destroyAfterPerLevel[level];
  }

  override protected void afterSpawn() {
    trying = false;
    decreaseEmissionAmount = objPrefab.transform.Find("BasicInside").GetComponent<ParticleSystem>().emissionRate / fullComboCount;

    int random = Random.Range(0, chanceBase);
    if (random < goldenChance) {
      isGolden = true;
      instance.GetComponent<Renderer>().sharedMaterial = goldenInactiveMat;
      instance.GetComponent<CubeDispenserMover>().setGolden();
      objEncounterEffectForPlayer = instance.transform.Find("GoldenReaction").GetComponent<ParticleSystem>();
    } else {
      isGolden = false;
      instance.GetComponent<Renderer>().sharedMaterial = inactiveMat;
      instance.GetComponent<CubeDispenserMover>().setNormal();
      objEncounterEffectForPlayer = instance.transform.Find("BasicReaction").GetComponent<ParticleSystem>();
    }
  }

  public void checkTrying() {
    if (!trying) {
      trying = true;
      skipInterval = false;

      if (isGolden) {
        instance.GetComponent<Renderer>().sharedMaterial = goldenActiveMat;
      } else {
        instance.GetComponent<Renderer>().sharedMaterial = activeMat;
      }
      player.encounterObject("CubeDispenser");
      StartCoroutine("destroyAfterTry");
    }
  }

  public void tryBreak() {
    if (!trying) {
      trying = true;
      skipInterval = false;
      if (isGolden) {
        instance.GetComponent<Renderer>().sharedMaterial = goldenActiveMat;
      } else {
        instance.GetComponent<Renderer>().sharedMaterial = activeMat;
      }
    }
  }

  IEnumerator destroyAfterTry() {
    yield return new WaitForSeconds(destroyAfterSeconds);

    if (instance == null || !instance.activeSelf) yield break;

    instance.GetComponent<ObjectsMover>().destroyObject(true, true);
  }
}

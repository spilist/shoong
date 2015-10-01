using UnityEngine;
using System.Collections;

public class CubeDispenserManager : ObjectsManager {
  public Mesh[] brokenMeshes;

  public int[] fullComboCountPerLevel;
  public float[] destroyAfterPerLevel;
  public int respawnInterval_min = 10;
  public int respawnInterval_max = 15;
  public int cubesPerContact = 10;
  public float reboundDuring = 0.2f;
	public ParticleSystem destroy;
  public float pitchStart = 0.9f;
  public float pitchIncrease = 0.05f;
  public Material activeMat;
  public float shakeDurationByHit = 0.3f;
  public float shakeAmountByHit = 0.5f;

  public float decreaseEmissionAmount;
  private bool trying = false;
  public int fullComboCount;
  private float destroyAfterSeconds;

  override public void initRest() {
    int level = DataManager.dm.getInt("CubeDispenserLevel") - 1;
    fullComboCount = fullComboCountPerLevel[level];
    destroyAfterSeconds = destroyAfterPerLevel[level];
    skipInterval = true;
  }

  override protected void afterSpawn() {
    trying = false;
    decreaseEmissionAmount = objPrefab.GetComponent<ParticleSystem>().emissionRate / fullComboCount;
    objEncounterEffectForPlayer = instance.transform.Find("Reaction").GetComponent<ParticleSystem>();
  }

  public void checkTrying() {
    if (!trying) {
      trying = true;
      skipInterval = false;
      instance.GetComponent<Renderer>().sharedMaterial = activeMat;
      player.encounterObject("CubeDispenser");
      StartCoroutine("destroyAfterTry");
    }
  }

  public void tryBreak() {
    if (!trying) {
      trying = true;
      skipInterval = false;
      instance.GetComponent<Renderer>().sharedMaterial = activeMat;
    }
  }

  IEnumerator destroyAfterTry() {
    yield return new WaitForSeconds(destroyAfterSeconds);

    if (instance == null) yield break;

    instance.GetComponent<ObjectsMover>().destroyObject(true, true);
  }
}

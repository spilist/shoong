using UnityEngine;
using System.Collections;

public class CubeDispenserManager : ObjectsManager {
  public GameObject cubeDispenserPrefab;
  public int[] fullComboCountPerLevel;
  public float[] destroyAfterPerLevel;
  public int respawnInterval_min = 10;
  public int respawnInterval_max = 15;
  public int cubesPerContact = 10;
  public float reboundDuring = 0.2f;
	public ParticleSystem destroy;

  private GameObject cubeDispenser;
  private int comboCount = 0;
  private float decreaseEmissionAmount;
  private bool notContactYet = true;
  private bool respawnRunning = false;
  private int fullComboCount;
  private float destroyAfterSeconds;

  override public void initRest() {
    int level = DataManager.dm.getInt("CubeDispenserLevel") - 1;
    fullComboCount = fullComboCountPerLevel[level];
    destroyAfterSeconds = destroyAfterPerLevel[level];
  }

  override public void run() {
    comboCount = 0;
    notContactYet = true;
    respawnRunning = false;
    decreaseEmissionAmount = cubeDispenserPrefab.GetComponent<ParticleSystem>().emissionRate / fullComboCount;

    cubeDispenser = spawnManager.spawn(cubeDispenserPrefab);
  }

	public void contact() {
    if (notContactYet) {
      notContactYet = false;
      StartCoroutine("destroyAfterTry");
    }
    comboCount++;
    cubeDispenser.GetComponent<ParticleSystem>().emissionRate -= decreaseEmissionAmount;

    if (comboCount == fullComboCountPerLevel[0]) {
      QuestManager.qm.addCountToQuest("CubeDispenser");
    }

    if (comboCount == fullComboCount) {
      QuestManager.qm.addCountToQuest("CompleteCubeDispenser");
      StartCoroutine("respawn");
    }
  }

  IEnumerator destroyAfterTry() {
    yield return new WaitForSeconds(destroyAfterSeconds);
    StartCoroutine("respawn");
  }

  public void destroyInstances() {
    StartCoroutine("respawn");
  }

  IEnumerator respawn() {
    if (!respawnRunning) {
      respawnRunning = true;
			Instantiate (destroy, cubeDispenser.transform.position, cubeDispenser.transform.rotation);
      // particle instantiate
      Destroy(cubeDispenser);
      if (!notContactYet) {
        yield return new WaitForSeconds(Random.Range(respawnInterval_min, respawnInterval_max));
      }
      run();
    }
  }
}

using UnityEngine;
using System.Collections;

public class CubeDispenserManager : MonoBehaviour {
  public FieldObjectsManager fom;

  public GameObject cubeDispenserPrefab;
	public int fullComboCount = 6;
  public int respawnInterval_min = 10;
  public int respawnInterval_max = 15;
  public float destroyAfterTry = 4;
  public int cubesPerContact = 10;

  private GameObject cubeDispenser;
  private int comboCount = 0;
  private float decreaseEmissionAmount;
  private bool notContactYet = true;

  public void run() {
    comboCount = 0;
    notContactYet = true;
    // decreaseEmissionAmount = cubeDispenserPrefab.GetComponent<ParticleSystem>().emissionRate / fullComboCount;

    cubeDispenser = fom.spawn(cubeDispenserPrefab);
  }

	public void contact() {
    if (notContactYet) {
      notContactYet = false;
      StartCoroutine("destroySelf");
    }
    comboCount++;
    cubeDispenser.GetComponent<ParticleSystem>().emissionRate -= decreaseEmissionAmount;

    if (comboCount == fullComboCount) {
      StartCoroutine("respawn");
    }
  }

  IEnumerator destroySelf() {
    yield return new WaitForSeconds(destroyAfterTry);
    StartCoroutine("respawn");
  }

  public void startRespawn() {
    StartCoroutine("respawn");
  }

  IEnumerator respawn() {
    // particle instantiate
    Destroy(cubeDispenser);
    if (!notContactYet) {
      yield return new WaitForSeconds(Random.Range(respawnInterval_min, respawnInterval_max));
    }
    run();
  }
}

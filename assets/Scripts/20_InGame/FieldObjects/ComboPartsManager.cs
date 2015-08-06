using UnityEngine;
using System.Collections;

public class ComboPartsManager : MonoBehaviour {
  public GameObject comboPartPrefab;
  public GameObject comboPartPrefab_next;
  public FieldObjectsManager fom;
  public int fullComboCount = 4;
  public int comboBonusScale = 5;
  public float radius = 20;
  public int respawnInterval_min = 10;
  public int respawnInterval_max = 15;

  private bool trying = false;
  private bool secondShot = false;
  private GameObject current;
  private GameObject next;
  private int comboCount = 0;
  private bool isSpawning = false;

  public void run() {
    comboCount = 0;
    isSpawning = false;
    current = fom.spawn(comboPartPrefab);
    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();

    Vector3 currentV = current.transform.position;

    Vector3 spawnPosition = new Vector3(currentV.x + randomV.x * radius, 0, currentV.z + randomV.y * radius);

    Quaternion spawnRotation = Quaternion.identity;
    next = (GameObject) Instantiate (comboPartPrefab_next, spawnPosition, spawnRotation);
    next.transform.parent = transform;
    next.GetComponent<OffsetFixer>().setParent(current);
  }

  public void tryToGet() {
    if (trying) {
      if (secondShot) {
        destroyInstances();
      } else {
        secondShot = true;
      }
    }
  }

  IEnumerator startSpawn() {
    if (trying) {
      yield return new WaitForSeconds(Random.Range(respawnInterval_min, respawnInterval_max));
    }
    trying = false;
    run();
  }

  public void eatenByPlayer() {
    comboCount++;
    trying = true;
    secondShot = false;

    if (comboCount == fullComboCount) {
      StartCoroutine("startSpawn");
      return;
    }

    Vector3 spawnPos = next.transform.position;
    Quaternion spawnRotation = next.transform.rotation;

    current = (GameObject) Instantiate (comboPartPrefab, spawnPos, spawnRotation);
    current.transform.parent = transform;

    Destroy(next);

    if (comboCount + 1 < fullComboCount) {
      Vector2 randomV = Random.insideUnitCircle;
      randomV.Normalize();
      Vector3 nextSpawnPos = new Vector3(spawnPos.x + randomV.x * radius, 0, spawnPos.z + randomV.y * radius);
      next = (GameObject) Instantiate (comboPartPrefab_next, nextSpawnPos, spawnRotation);
      next.transform.parent = transform;
      next.GetComponent<OffsetFixer>().setParent(current);
    }
  }

  public void destroyInstances() {
    if (isSpawning) return;

    isSpawning = true;
    Destroy(current);
    if (next != null) Destroy(next);
    StartCoroutine("startSpawn");
  }

  public int getComboCount() {
    return comboCount;
  }
}

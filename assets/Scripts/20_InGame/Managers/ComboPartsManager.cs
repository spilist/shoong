using UnityEngine;
using System.Collections;

public class ComboPartsManager : ObjectsManager {
  public GameObject comboPartPrefab;
  public GameObject comboPartPrefab_next;

  public int[] fullComboCountPerLevel;

  public int comboBonusScale = 5;
  public float radius = 20;
  public int respawnInterval_min = 10;
  public int respawnInterval_max = 15;
  public float tumble = 3;
  public float pitchStart = 0.4f;
  public float pitchIncrease = 0.05f;

  private bool trying = false;
  private bool secondShot = false;
  private GameObject current;
  private GameObject next;
  private int comboCount = 0;
  private int boosterCount = 0;
  private int fullComboCount;

  override public void initRest() {
    fullComboCount = fullComboCountPerLevel[DataManager.dm.getInt("ComboPartsLevel") - 1];
  }

  override public void run() {
    comboCount = 0;
    boosterCount = 0;
    current = spawnManager.spawn(comboPartPrefab);
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
        secondShot = false;
        destroyInstances();
      } else {
        secondShot = true;
        boosterCount++;
      }
    }
  }

  IEnumerator startSpawn() {
    if (trying) {
      trying = false;
      yield return new WaitForSeconds(Random.Range(respawnInterval_min, respawnInterval_max));
    }
    run();
  }

  public void eatenByPlayer() {
    comboCount++;
    trying = true;
    secondShot = false;

    if (comboCount == fullComboCountPerLevel[0]) {
      QuestManager.qm.addCountToQuest("ComboParts");
    }

    if (comboCount == fullComboCount) {
      QuestManager.qm.addCountToQuest("CompleteComboParts");
      player.showEffect("Great");
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
    foreach (GameObject comboPart in GameObject.FindGameObjectsWithTag("ComboPart")) {
      Destroy(comboPart);
    }

    StartCoroutine("startSpawn");
  }

  public int getComboCount() {
    return comboCount;
  }

  override public float getTumble(string objTag) {
    return tumble;
  }
}

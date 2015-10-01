using UnityEngine;
using System.Collections;

public class ComboPartsManager : ObjectsManager {
  public GameObject objPrefab_next;

  public int[] fullComboCountPerLevel;

  public int comboBonusScale = 5;
  public float radius = 20;

  public float pitchStart = 0.4f;
  public float pitchIncrease = 0.05f;

  private bool trying = false;
  private bool secondShot = false;

  public GameObject nextInstance;
  private int comboCount = 0;
  private int fullComboCount;

  override public void initRest() {
    fullComboCount = fullComboCountPerLevel[DataManager.dm.getInt("ComboPartsLevel") - 1];
    skipInterval = true;
  }

  override protected void afterSpawn() {
    trying = false;
    comboCount = 0;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    Vector3 currentV = instance.transform.position;
    Vector3 spawnPosition = new Vector3(currentV.x + randomV.x * radius, 0, currentV.z + randomV.y * radius);

    nextInstance = (GameObject) Instantiate (objPrefab_next, spawnPosition, Quaternion.identity);
    nextInstance.transform.parent = transform;
    nextInstance.GetComponent<OffsetFixer>().setParent(instance);
  }

  public void tryToGet() {
    if (trying) {
      if (secondShot) {
        secondShot = false;
        if (instance != null) instance.GetComponent<ComboPartMover>().destroyObject();
      } else {
        secondShot = true;
      }
    }
  }

  public void eatenByPlayer() {
    comboCount++;
    trying = true;
    secondShot = false;

    if (comboCount == 1) {
      player.encounterObject(tag);
    }

    if (comboCount == fullComboCountPerLevel[0]) {
      QuestManager.qm.addCountToQuest("ComboParts");
    }

    if (comboCount == fullComboCount) {
      QuestManager.qm.addCountToQuest("CompleteComboParts");
      DataManager.dm.increment("NumCompleteIllusion");
      player.showEffect("Great");
      run();
      return;
    }

    Vector3 spawnPos = nextInstance.transform.position;
    Quaternion spawnRotation = nextInstance.transform.rotation;

    instance = (GameObject) Instantiate (objPrefab, spawnPos, spawnRotation);
    instance.transform.parent = transform;

    Destroy(nextInstance);

    if (comboCount + 1 < fullComboCount) {
      Vector2 randomV = Random.insideUnitCircle;
      randomV.Normalize();
      Vector3 nextSpawnPos = new Vector3(spawnPos.x + randomV.x * radius, 0, spawnPos.z + randomV.y * radius);
      nextInstance = (GameObject) Instantiate (objPrefab_next, nextSpawnPos, spawnRotation);
      nextInstance.transform.parent = transform;
      nextInstance.GetComponent<OffsetFixer>().setParent(instance);
    }
  }

  public int getComboCount() {
    return comboCount;
  }

  override public int cubesWhenEncounter() {
    return (comboCount + 1) * comboBonusScale;
  }

  override protected float spawnInterval() {
    if (!trying) return 0;
    else return Random.Range(minSpawnInterval, maxSpawnInterval);
  }
}

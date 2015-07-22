﻿using UnityEngine;
using System.Collections;

public class GenerateNextSpecial : MonoBehaviour {
  public GameObject next_prefab;
  public GameObject energyDestroy;

  GameObject next;
  SpecialObjectsManager som;
  int maxcombo;

  int comboCount;
  bool secondShot = false;

  void Start() {
    som = GameObject.Find("Field Objects").GetComponent<SpecialObjectsManager>();
    maxcombo = GameObject.Find("Player").GetComponent<PlayerMover>().max_unstoppable_combo;
  }

  public GameObject spawnNext() {
    Vector3 currentPos = next.transform.position;
    Quaternion spawnRotation = next.transform.rotation;

    GameObject newInstance = (GameObject) Instantiate (gameObject, currentPos, spawnRotation);
    newInstance.transform.parent = som.gameObject.transform;
    newInstance.GetComponent<GenerateNextSpecial>().setComboCount(comboCount + 1);

    if (maxcombo > comboCount) {
      Vector3 spawnPosition = getNextSpawnPosition(currentPos);
      GameObject nextInstance = (GameObject) Instantiate (next_prefab, spawnPosition, spawnRotation);
      nextInstance.transform.parent = som.gameObject.transform;
      nextInstance.GetComponent<OffsetFixer>().setParent(newInstance);
      newInstance.GetComponent<GenerateNextSpecial>().setNext(nextInstance);
    }
    else {
      // 마지막 부품일 경우 효과를 다르게 줌
    }

    Destroy(next);

    return newInstance;
  }

  Vector3 getNextSpawnPosition(Vector3 pos) {
    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    return new Vector3(pos.x + randomV.x * som.radius, 0, pos.z + randomV.y * som.radius);
  }

  public void setComboCount(int val) {
    comboCount = val;
  }

  public int getComboCount() {
    return comboCount;
  }

  public void setNext(GameObject target) {
    next = target;
  }

  public GameObject getNext() {
    return next;
  }

  public void tryGetSpecial() {
    if (secondShot) {
      destroySelf(true, true, false);
    }
    else {
      secondShot = true;
    }
  }

  public void destroySelf(bool turnEffectON, bool unstoppable, bool createNew) {
    if (turnEffectON) {
      Instantiate(energyDestroy, transform.position, transform.rotation);
      if (next != null) Instantiate(energyDestroy, next.transform.position, next.transform.rotation);
    }

    if (unstoppable) {
      if (comboCount > 0) {
        GameObject.Find("Player").GetComponent<PlayerMover>().startUnstoppable(comboCount);
      }
    }

    Destroy (gameObject);
    if (next != null) Destroy(next);

    if (createNew) {
      som.run();
    }
  }
}

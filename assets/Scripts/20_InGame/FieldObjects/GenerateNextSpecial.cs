﻿using UnityEngine;
using System.Collections;

public class GenerateNextSpecial : MonoBehaviour {
  public GameObject next_prefab;

  GameObject next;
  SpecialObjectsManager som;

  int comboCount;

  void Start() {
    som = GameObject.Find("Field Objects").GetComponent<SpecialObjectsManager>();
  }

  public void spawnNext() {
    Vector3 currentPos = next.transform.position;
    Quaternion spawnRotation = next.transform.rotation;

    GameObject newInstance = (GameObject) Instantiate (gameObject, currentPos, spawnRotation);
    newInstance.transform.parent = som.gameObject.transform;

    Vector3 spawnPosition = getNextSpawnPosition(currentPos);
    GameObject nextInstance = (GameObject) Instantiate (next_prefab, spawnPosition, spawnRotation);
    nextInstance.transform.parent = som.gameObject.transform;
    nextInstance.GetComponent<OffsetFixer>().setParent(newInstance);

    newInstance.GetComponent<GenerateNextSpecial>().setNext(nextInstance);
    newInstance.GetComponent<GenerateNextSpecial>().setComboCount(comboCount + 1);

    Destroy(next);
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
}

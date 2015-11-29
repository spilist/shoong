using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoldenCubeManager : ObjectsManager {
  public NormalPartsManager npm;
  public float spawnRadius = 200;

  override public void initRest() {
    npm = GetComponent<NormalPartsManager>();
    respawn();
  }

  override protected void spawn() {
    if (player == null || ScoreManager.sm.isGameOver()) return;

    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);

    instance = getPooledObj(objPool, objPrefab, spawnPos);
    instance.SetActive(true);
  }

  override protected float spawnInterval() {
    return Random.Range(minSpawnInterval, maxSpawnInterval);
  }

  public void goldenDestroyEffect(Vector3 pos) {
    GameObject obj = getPooledObj(objDestroyEffectPool, objDestroyEffect, pos);
    obj.SetActive(true);
  }

  public void spawnGoldenCube(Vector3 pos, bool autoDestroy = false, Transform parent = null) {
    GameObject obj = getPooledObj(objPool, objPrefab, pos);
    if (parent != null) {
      obj.transform.SetParent(parent, false);
      obj.transform.localPosition = pos;
    }
    obj.SetActive(true);
    obj.GetComponent<GoldenCubeMover>().setNoRespawn(autoDestroy);
  }

  public void popCoin(Vector3 pos) {
    GameObject obj = getPooledObj(objPool, objPrefab, pos);
    obj.GetComponent<Collider>().enabled = false;
    obj.GetComponent<GoldenCubeMover>().pop(Random.Range(npm.popMinDistance, npm.popMaxDistance));
  }
}

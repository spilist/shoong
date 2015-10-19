using UnityEngine;
using System.Collections;

public class SummonedPartMover : ObjectsMover {
  SummonPartsManager summonManager;
  Renderer mRenderer;
  bool isGoldenCube = false;
  bool isSuperheatPart = false;

  override protected void initializeRest() {
    summonManager = (SummonPartsManager) objectsManager;
    mRenderer = GetComponent<Renderer>();
    StartCoroutine("destroyAfter");
  }

  public void setGolden() {
    isGoldenCube = true;
  }

  public void setSuper() {
    isSuperheatPart = true;
  }

  override public void destroyObject (bool destroyEffect = true, bool byPlayer = false) {
    foreach (Collider collider in GetComponents<Collider>()) {
      collider.enabled = false;
    }
    Destroy(gameObject);

    if (destroyEffect) {
      Instantiate(objectsManager.objDestroyEffect, transform.position, transform.rotation);
    }

    if (byPlayer) {
      summonManager.increaseSummonedPartGetcount();
      if (isGoldenCube) {
        summonManager.gcCount.add(summonManager.goldCubesGet);
      } else if (isSuperheatPart) {
        summonManager.shm.add();
      } else {
        summonManager.ptb.checkCollected(GetComponent<MeshFilter>().sharedMesh);
      }
    }
  }

  override protected void afterEncounter() {
    summonManager.increaseSummonedPartGetcount();
    if (isGoldenCube) {
      summonManager.gcCount.add(summonManager.goldCubesGet);
    } else if (isSuperheatPart) {
      summonManager.shm.add();
    } else {
      summonManager.ptb.checkCollected(GetComponent<MeshFilter>().sharedMesh);
    }

    if (player.isNearAsteroid()) {
      player.showEffect("Wow");
    }
  }

  override public string getManager() {
    return "SummonPartsManager";
  }

  IEnumerator destroyAfter() {
    yield return new WaitForSeconds(summonManager.summonedPartLifetime - summonManager.blinkBeforeDestroy);
    float duration = summonManager.blinkBeforeDestroy;
    float showDuring = summonManager.showDurationStart;
    float emptyDuring = summonManager.emptyDurationStart;
    float showDurationDecrease = summonManager.showDurationDecrease;
    float emptyDurationDecrease = summonManager.emptyDurationDecrease;

    while (duration > 0) {
      mRenderer.enabled = true;

      yield return new WaitForSeconds (showDuring);

      mRenderer.enabled = false;
      yield return new WaitForSeconds (emptyDuring);

      duration -= showDuring + emptyDuring;

      if(showDuring > 1f) showDuring -= showDurationDecrease;
      if(emptyDuring > 0.5f) emptyDuring -= emptyDurationDecrease;
    }

    destroyObject();
  }

  override public bool hasEncounterEffect() {
    return false;
  }

  override public int bonusCubes() {
    return player.isNearAsteroid()? player.nearAsteroidBonus : 0;
  }
}

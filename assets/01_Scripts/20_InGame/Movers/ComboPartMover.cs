using UnityEngine;
using System.Collections;

public class ComboPartMover : ObjectsMover {
  bool isGoldenCube = false;
  bool isSuperheatPart = false;
  bool willBeDestroyed = false;
  ComboPartsManager cpm;
  Renderer mRenderer;
  Renderer mRenderer_next;

  override protected void initializeRest() {
    cpm = (ComboPartsManager)objectsManager;
    mRenderer = GetComponent<Renderer>();
    if (cpm.nextInstance != null) mRenderer_next = cpm.nextInstance.GetComponent<Renderer>();

    if (willBeDestroyed) StartCoroutine("destroyAfter");
  }

  public void setGolden() {
    isGoldenCube = true;
    objectsManager.objDestroyEffect = cpm.goldCubeDestroyParticle;
  }

  public void setSuper() {
    isSuperheatPart = true;
  }

  public void setDestroyAfter() {
    willBeDestroyed = true;
  }

  override public string getManager() {
    return "ComboPartsManager";
  }

  override protected void afterDestroy(bool byPlayer) {
    Destroy(cpm.nextInstance);
  }

  override protected bool beforeEncounter() {
    objectsManager.objEncounterEffectForPlayer.GetComponent<AudioSource>().pitch = cpm.pitchStart + cpm.getComboCount() * cpm.pitchIncrease;

    return true;
  }

  override protected void afterEncounter() {
    if (isGoldenCube) {
      cpm.gcCount.add(cpm.goldCubesGet);
    } else if (isSuperheatPart) {
      cpm.shm.add();
    } else {
      cpm.ptb.checkCollected(GetComponent<MeshFilter>().sharedMesh);
    }
    cpm.eatenByPlayer();
  }

  override public int cubesWhenDestroy() {
    int count = 0;
    for (int i = cpm.comboCount; i < cpm.fullComboCount; i++) {
      count += (i + 1);
    }

    return count * cpm.cubesByEncounter;
  }

  override public int bonusCubes() {
    return player.isNearAsteroid()? player.nearAsteroidBonus : 0;
  }

  public IEnumerator destroyAfter() {
    Debug.Log(cpm);
    yield return new WaitForSeconds(cpm.illusionLifeTime - cpm.blinkBeforeDestroy);
    float duration = cpm.blinkBeforeDestroy;
    float showDuring = cpm.showDurationStart;
    float emptyDuring = cpm.emptyDurationStart;
    float showDurationDecrease = cpm.showDurationDecrease;
    float emptyDurationDecrease = cpm.emptyDurationDecrease;

    while (duration > 0) {
      mRenderer.enabled = true;
      mRenderer_next.enabled = true;

      yield return new WaitForSeconds (showDuring);

      mRenderer.enabled = false;
      mRenderer_next.enabled = false;

      yield return new WaitForSeconds (emptyDuring);

      duration -= showDuring + emptyDuring;

      if(showDuring > 1f) showDuring -= showDurationDecrease;
      if(emptyDuring > 0.5f) emptyDuring -= emptyDurationDecrease;
    }

    destroyObject();
  }
}

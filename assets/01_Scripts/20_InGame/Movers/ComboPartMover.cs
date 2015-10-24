using UnityEngine;
using System.Collections;

public class ComboPartMover : ObjectsMover {
  bool isGoldenCube = false;
  bool isSuperheatPart = false;
  bool willBeDestroyed = false;
  ComboPartsManager cpm;
  Renderer mRenderer;
  Renderer mRenderer_next;
  SphereCollider sCollider;

  override protected void initializeRest() {
    cpm = (ComboPartsManager)objectsManager;
    mRenderer = GetComponent<Renderer>();
    sCollider = GetComponent<SphereCollider>();
  }

  override protected void afterEnable() {
    if (cpm.nextInstance != null) mRenderer_next = cpm.nextInstance.GetComponent<Renderer>();

    if (willBeDestroyed) StartCoroutine("destroyAfter");
  }

  public void setGolden() {
    isGoldenCube = true;
    isSuperheatPart = false;
    objectsManager.objDestroyEffect = cpm.goldCubeDestroyParticle;

    mRenderer.sharedMaterial = cpm.goldenCubePrefab.GetComponent<Renderer>().sharedMaterial;
    sCollider.radius = cpm.goldenCubePrefab.GetComponent<SphereCollider>().radius;
    transform.Find("BasicEffect").gameObject.SetActive(false);
    transform.Find("GoldenEffect").gameObject.SetActive(true);
  }

  public void setSuper() {
    isGoldenCube = false;
    isSuperheatPart = true;
    objectsManager.objDestroyEffect = cpm.objDestroyEffect;

    mRenderer.sharedMaterial = cpm.superheatPartPrefab.GetComponent<Renderer>().sharedMaterial;
    sCollider.radius = cpm.superheatPartPrefab.GetComponent<SphereCollider>().radius;
    transform.Find("BasicEffect").gameObject.SetActive(true);
    transform.Find("GoldenEffect").gameObject.SetActive(false);
  }

  public void setNormal() {
    if (isGoldenCube || isSuperheatPart) {
      isGoldenCube = false;
      isSuperheatPart = false;
      objectsManager.objDestroyEffect = cpm.objDestroyEffect;

      mRenderer.sharedMaterial = cpm.objPrefab.GetComponent<Renderer>().sharedMaterial;
      sCollider.radius = cpm.objPrefab.GetComponent<SphereCollider>().radius;
      transform.Find("BasicEffect").gameObject.SetActive(true);
      transform.Find("GoldenEffect").gameObject.SetActive(false);
    }
  }

  public void setDestroyAfter() {
    willBeDestroyed = true;
  }

  override public string getManager() {
    return "ComboPartsManager";
  }

  override protected void afterDestroy(bool byPlayer) {
    willBeDestroyed = false;
    if (cpm.nextInstance != null) cpm.nextInstance.SetActive(false);
  }

  override protected bool beforeEncounter() {
    objectsManager.objEncounterEffectForPlayer.GetComponent<AudioSource>().pitch = cpm.pitchStart + cpm.getComboCount() * cpm.pitchIncrease;

    return true;
  }

  override protected void afterEncounter() {
    willBeDestroyed = false;
    if (isGoldenCube) {
      cpm.gcCount.add(cpm.goldCubesGet);
    } else if (isSuperheatPart) {
      cpm.shm.add();
    }
    // else {
    //   cpm.ptb.checkCollected(GetComponent<MeshFilter>().sharedMesh);
    // }
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
    yield return new WaitForSeconds(cpm.illusionLifeTime - cpm.blinkBeforeDestroy);
    float duration = cpm.blinkBeforeDestroy;
    float showDuring = cpm.showDurationStart;
    float emptyDuring = cpm.emptyDurationStart;
    float showDurationDecrease = cpm.showDurationDecrease;
    float emptyDurationDecrease = cpm.emptyDurationDecrease;

    while (duration > 0) {
      mRenderer.enabled = true;
      if (mRenderer_next != null) mRenderer_next.enabled = true;

      yield return new WaitForSeconds (showDuring);

      mRenderer.enabled = false;
      if (mRenderer_next != null) mRenderer_next.enabled = false;

      yield return new WaitForSeconds (emptyDuring);

      duration -= showDuring + emptyDuring;

      if(showDuring > 1f) showDuring -= showDurationDecrease;
      if(emptyDuring > 0.5f) emptyDuring -= emptyDurationDecrease;
    }

    destroyObject();
  }
}

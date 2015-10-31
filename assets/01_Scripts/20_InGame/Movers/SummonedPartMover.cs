using UnityEngine;
using System.Collections;

public class SummonedPartMover : ObjectsMover {
  SummonPartsManager summonManager;
  Renderer mRenderer;
  bool isGoldenCube = false;

  override protected void initializeRest() {
    summonManager = (SummonPartsManager) objectsManager;
    mRenderer = GetComponent<Renderer>();
  }

  override protected void afterEnable() {
    mRenderer.enabled = true;
    StartCoroutine("destroyAfter");
  }

  public void setGolden() {
    isGoldenCube = true;
    mRenderer.sharedMaterial = GoldManager.gm.goldenMat;
    transform.Find("GoldenEffect").gameObject.SetActive(true);
    transform.Find("BasicEffect").gameObject.SetActive(false);
  }

  public void setNormal() {
    isGoldenCube = false;
    mRenderer.sharedMaterial = summonManager.summonedPartPrefab.GetComponent<Renderer>().sharedMaterial;
    transform.Find("GoldenEffect").gameObject.SetActive(false);
    transform.Find("BasicEffect").gameObject.SetActive(true);
  }

  override public void destroyObject (bool destroyEffect = true, bool byPlayer = false) {

    gameObject.SetActive(false);

    if (destroyEffect) {
      if (isGoldenCube) {
        GoldManager.gm.gcm.goldenDestroyEffect(transform.position);
      } else {
        showDestroyEffect(byPlayer);
      }
    }

    if (byPlayer) {
      summonManager.increaseSummonedPartGetcount();
      if (isGoldenCube) {
        GoldManager.gm.add(transform.position, summonManager.goldCubesGet);
      }
    }
  }

  override protected void afterEncounter() {
    summonManager.increaseSummonedPartGetcount();
    if (isGoldenCube) {
      GoldManager.gm.add(transform.position, summonManager.goldCubesGet);
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
}

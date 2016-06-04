using UnityEngine;
using System.Collections;

public class ComboPartMover : ObjectsMover {
  bool willBeDestroyed = false;
  ComboPartsManager cpm;
  Renderer mRenderer;
  Renderer mRenderer_next;

  override protected void initializeRest() {
    cpm = (ComboPartsManager)objectsManager;
    mRenderer = GetComponent<Renderer>();
  }

  override protected void afterEnable() {
    mRenderer.enabled = true;
    shrinkedScale = cpm.increasedScale();
    transform.localScale = shrinkedScale * Vector3.one;

    if (cpm.nextInstance != null) {
      mRenderer_next = cpm.nextInstance.GetComponent<Renderer>();
      mRenderer_next.enabled = true;
    }

    if (willBeDestroyed) StartCoroutine("destroyAfter");
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
    cpm.eatenByPlayer();
  }

  override public int cubesWhenDestroy() {
    int count = 0;
    for (int i = cpm.comboCount; i < cpm.fullComboCount; i++) {
      count += (i + 1);
    }

    return count * cpm.cubesByEncounter;
  }

  override public int energyGets() {
    return (cpm.comboCount + 1) * cpm.energyGets;
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

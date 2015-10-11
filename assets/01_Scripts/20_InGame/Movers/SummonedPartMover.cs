using UnityEngine;
using System.Collections;

public class SummonedPartMover : ObjectsMover {
  SummonPartsManager summonManager;
  Renderer mRenderer;
  Color color_full;
  Color color_empty;
  Color outlineColor_full;
  Color outlineColor_empty;

  override protected void initializeRest() {
    summonManager = (SummonPartsManager) objectsManager;
    mRenderer = GetComponent<Renderer>();
    color_full = mRenderer.material.color;
    color_empty = new Color(color_full.r, color_full.g, color_full.b, summonManager.blinkColorAlpha);
    outlineColor_full = mRenderer.material.GetColor("_OutlineColor");
    outlineColor_empty = summonManager.blinkOutlineColor;
    StartCoroutine("destroyAfter");
  }

  override public void destroyObject (bool destroyEffect = true, bool byPlayer = false) {
    foreach (Collider collider in GetComponents<Collider>()) {
      collider.enabled = false;
    }
    Destroy(gameObject);

    if (destroyEffect) {
      Instantiate(objectsManager.objDestroyEffect, transform.position, transform.rotation);
    }

    if (byPlayer) summonManager.increaseSummonedPartGetcount();
  }

  override protected void afterEncounter() {
    summonManager.increaseSummonedPartGetcount();

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
      mRenderer.material.color = color_full;
      mRenderer.material.SetColor("_OutlineColor", outlineColor_full);

      mRenderer.material.SetFloat("_Outline", 0.75f);
      yield return new WaitForSeconds (showDuring);

      mRenderer.material.color = color_empty;
      mRenderer.material.SetColor("_OutlineColor", outlineColor_empty);
      mRenderer.material.SetFloat("_Outline", 0);
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

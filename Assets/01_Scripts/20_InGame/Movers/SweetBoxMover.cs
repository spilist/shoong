using UnityEngine;
using System.Collections;

public class SweetBoxMover : ObjectsMover {
  SweetBoxManager sbm;
  float generateInterval;

	protected override void initializeRest() {
    sbm = (SweetBoxManager)objectsManager;
    generateInterval = sbm.generatingDuration / sbm.numGeneration;
  }

  override protected void afterEnable() {
  }

  override public string getManager() {
    return "SweetBoxManager";
  }

  override protected float getSpeed() {
    return 0;
  }

  override protected void afterEncounter() {
    sbm.GetComponent<NormalPartsManager>().popSweetsByBox(sbm.numGeneration, sbm.generateAfter, generateInterval, transform.position);
    sbm.run();
  }

  override public void destroyObject(bool destroyEffect = true, bool byPlayer = false, bool respawn = true) {

    gameObject.SetActive(false);

    if (destroyEffect) {
      showDestroyEffect(byPlayer);
    }

    if (byPlayer) {
      sbm.run();
    } else {
      sbm.runImmediately();
    }
  }
}

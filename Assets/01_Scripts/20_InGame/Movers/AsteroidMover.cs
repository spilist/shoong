using UnityEngine;
using System.Collections;

public class AsteroidMover : ObjectsMover {
  AsteroidManager asm;
  private int minBrokenSpawn;
  private int maxBrokenSpawn;
  private float minBrokenSize;
  private float maxBrokenSize;
  private Animation beatAnimation;

  override public string getManager() {
    return "AsteroidManager";
  }

  override protected void initializeRest() {
    asm = (AsteroidManager)objectsManager;
    canBeMagnetized = false;
    minBrokenSpawn = asm.minBrokenSpawn;
    maxBrokenSpawn = asm.maxBrokenSpawn;
    minBrokenSize = asm.minBrokenSize;
    maxBrokenSize = asm.maxBrokenSize;
    beatAnimation = GetComponent<Animation>();
    beatAnimation.wrapMode = WrapMode.Once;
    
    RhythmManager.rm.registerCallback(GetInstanceID(), () => {
      beatAnimation.Play();
    });
  }
  

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }

  override public void showDestroyEffect(bool byPlayer) {
    for (int howMany = Random.Range(minBrokenSpawn, maxBrokenSpawn + 1); howMany > 0; howMany--) {
      GameObject broken = asm.getPooledObj(objectsManager.objDestroyEffectPool, objectsManager.objDestroyEffect, transform.position);
      broken.SetActive(true);
      broken.transform.localScale = Random.Range(minBrokenSize, maxBrokenSize) * Vector3.one;
      broken.GetComponent<MeshFilter>().sharedMesh = asm.getRandomMesh();

      if (howMany == 1) broken.GetComponent<AudioSource>().Play();
    }
  }

  override public void showEncounterEffect() {
    showDestroyEffect(true);
  }
}

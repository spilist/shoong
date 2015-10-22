using UnityEngine;
using System.Collections;

public class GoldenCubeMover : ObjectsMover {
  private GoldenCubeManager gcm;
  private bool detected = false;

  protected override void initializeRest() {
    gcm = (GoldenCubeManager)objectsManager;
  }

  protected override bool beforeCollide(ObjectsMover other) {
    if (other.tag == "Obstacle") {
      destroyObject();
      return false;
    } else {
      return true;
    }
  }

  protected override void normalMovement() {
    direction = player.transform.position - transform.position;
    float distance = direction.magnitude;
    direction /= distance;
    if (distance < gcm.detectDistance) {
      rb.velocity = -direction * gcm.speed;
      if (!detected) {
        detected = true;
        StartCoroutine("generateCube");
      }
    }
  }

  IEnumerator generateCube() {
    while (!player.scoreManager.isGameOver()) {
      yield return new WaitForSeconds(gcm.generateCubePer);
      GameObject cube = gcm.getPooledObj(gcm.cubePool, gcm.energyCubePrefab, transform.position);
      cube.GetComponent<ParticleMover>().triggerCubesGet(1, false);
    }
  }

  override protected void afterEncounter() {
    gcm.gcCount.add(gcm.cubesWhenEncounter());
    objectsManager.run();
  }

  override public string getManager() {
    return "GoldenCubeManager";
  }

  override public bool noCubesByDestroy() {
    return true;
  }

  override protected float getSpeed() {
    return 0;
  }
}

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
      GameObject cube = (GameObject) Instantiate(gcm.energyCubePrefab, transform.position, transform.rotation);
      cube.GetComponent<ParticleMover>().triggerCubesGet(1, false);
    }
  }

  override protected void afterEncounter() {
    gcm.gcCount.add(gcm.cubesWhenEncounter());
    player.showEffect("GoldenCube");
    objectsManager.run();
  }

  override public string getManager() {
    return "GoldenCubeManager";
  }
}

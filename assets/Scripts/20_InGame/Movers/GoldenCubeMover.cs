using UnityEngine;
using System.Collections;

public class GoldenCubeMover : ObjectsMover {
  private GoldenCubeManager gcm;
  private bool detected = false;

  protected override void initializeRest() {
    gcm = objectsManager.GetComponent<GoldenCubeManager>();
  }

  protected override float getSpeed() {
    return 0;
  }

  protected override float getTumble() {
    return gcm.tumble;
  }

  protected override bool beforeCollision(ObjectsMover other) {
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
    } else if (isMagnetized) {
      rb.velocity = direction * player.GetComponent<Rigidbody>().velocity.magnitude * 1.5f;
    }
  }

  IEnumerator generateCube() {
    while (!player.scoreManager.isGameOver()) {
      yield return new WaitForSeconds(gcm.generateCubePer);
      GameObject cube = (GameObject) Instantiate(gcm.energyCubePrefab, transform.position, transform.rotation);
      cube.GetComponent<ParticleMover>().triggerCubesGet(1, false);
    }
  }

  public override void destroyObject(bool destroyEffect = true) {
    Destroy(gameObject);
    if (destroyEffect) {
      Instantiate(gcm.destroyEffect, transform.position, transform.rotation);
    }

    gcm.respawn();
  }

  override public void encounterPlayer() {
    Destroy(gameObject);
    gcm.getEffect.Play();

    gcm.gcCount.add(gcm.numGoldenCubesGet);
    player.showEffect("GoldenCube");

    gcm.respawn();
  }

  override public string getManager() {
    return "GoldenCubeManager";
  }

  override public int cubesWhenEncounter() {
    return gcm.numGoldenCubesGet;
  }
}

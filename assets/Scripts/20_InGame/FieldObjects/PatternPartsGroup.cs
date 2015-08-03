using UnityEngine;
using System.Collections;

public class PatternPartsGroup : MonoBehaviour {
  private Vector3 direction;
  private float speed;
  private PatternPartsManager ppm;
  private int numPatternParts;
  private int count = 0;
  private PlayerMover player;

  void Start() {
    ppm = GameObject.Find("Field Objects").GetComponent<PatternPartsManager>();
    speed = ppm.group_speed;
    player = GameObject.Find("Player").GetComponent<PlayerMover>();
  }

  public void run() {
    Vector2 randomV = Random.insideUnitCircle;
    direction = new Vector3(randomV.x, 0, randomV.y);
    direction.Normalize();

    foreach (Transform tr in transform) {
      tr.gameObject.GetComponent<Rigidbody>().velocity = direction * speed;
      if (tr.tag == "PatternPart") numPatternParts++;
    }
    transform.Rotate(Vector3.up * Random.Range(0, 360));
  }

  public void destroyAfterCount() {
    count++;

    if (count == numPatternParts) absorbAll();

    if (count > 1) return;

    StartCoroutine("destroyAll");
  }

  public void absorbAll() {
    foreach (Transform tr in transform) {
      if (tr.tag == "PatternPart") {
        // some special effect
        for (int i = 0; i < ppm.numBonusParts; i++)
          Instantiate(ppm.particlesPrefab, tr.position, tr.rotation);
        player.GetComponent<AudioSource>().Play();
        player.getEnergyBar().getHealthbyParts();
        player.partsCount.addCount(ppm.numBonusParts);
      }
    }
    Destroy(gameObject);
  }

  IEnumerator destroyAll() {
    yield return new WaitForSeconds(ppm.destroyAfter);

    if (count < numPatternParts) Destroy(gameObject);

    ppm.spawnReady();
  }
}

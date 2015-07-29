using UnityEngine;
using System.Collections;

public class PatternPartsGroup : MonoBehaviour {
  private Vector3 direction;
  private float speed;

  public void run() {
    PatternPartsManager ppm = GameObject.Find("Field Objects").GetComponent<PatternPartsManager>();
    speed = ppm.group_speed;

    Vector2 randomV = Random.insideUnitCircle;
    direction = new Vector3(randomV.x, 0, randomV.y);
    direction.Normalize();

    foreach (Transform tr in transform) {
      tr.gameObject.GetComponent<Rigidbody>().velocity = direction * speed;
    }

    transform.Rotate(Vector3.up * Random.Range(0, 360));
  }

  public void destroyAll() {
    foreach (Transform tr in transform) {
      Destroy(tr.gameObject);
    }
    Destroy(gameObject);
  }
}

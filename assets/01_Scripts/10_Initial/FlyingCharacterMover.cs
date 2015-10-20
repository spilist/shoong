using UnityEngine;
using System.Collections;

public class FlyingCharacterMover : MonoBehaviour {
	private Rigidbody rb;
  private bool destroyByEnter = false;

  public void run(Vector3 dir, int speed, int tumble) {
    rb = GetComponent<Rigidbody>();
    rb.angularVelocity = Random.onUnitSphere * tumble;
    rb.velocity = dir * speed;
  }
}

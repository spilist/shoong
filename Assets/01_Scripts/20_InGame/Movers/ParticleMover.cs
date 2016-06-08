using UnityEngine;
using System.Collections;

public class ParticleMover : MonoBehaviour {
	public float startTime = 0.5f;
	private float time;
	public float tumble = 1;
	private Vector3 direction;
	private bool timeelapsed = false;
	private Rigidbody rb;

	void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	void OnEnable () {
		rb.angularVelocity = Random.onUnitSphere * tumble;

		Vector2 randomV = Random.insideUnitCircle;
		randomV.Normalize();
		direction = new Vector3(randomV.x, 0, randomV.y);
		rb.velocity = direction * GoldManager.gm.goldenCubeStartSpeed;

		time = startTime;
		timeelapsed = false;
	}

	void FixedUpdate () {
		if(time > 0) {
			time -= Time.deltaTime;
		} else {
			timeelapsed = true;

			Vector3 heading =  GoldManager.gm.ingameCollider.position - transform.position;
			heading /= heading.magnitude;
			rb.velocity = heading * GoldManager.gm.goldenCubeFollowSpeed;
		}
	}

	void OnTriggerEnter (Collider other) {
		if (timeelapsed && gameObject.activeInHierarchy) {
			GoldManager.gm.addCountIngame();
			gameObject.SetActive(false);
		}
	}

  void OnTriggerStay (Collider other) {
    if (timeelapsed && gameObject.activeInHierarchy) {
      GoldManager.gm.addCountIngame();
      gameObject.SetActive(false);
    }
  }
}

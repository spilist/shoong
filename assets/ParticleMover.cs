using UnityEngine;
using System.Collections;

public class ParticleMover : MonoBehaviour {

	public float speed=30;
	public float time = 1;
	public float tumble = 1;
	private float randomscale;
	private float random;
	private Vector3 direction;
	private bool timeelapsed = false;
	private PartsCollector partsCollector;

	void Start () {
		random = Random.Range (0.5f, 1.5f);
		randomscale = transform.localScale.x * random;


		transform.localScale = new Vector3(randomscale,randomscale,randomscale);

		GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

		Vector2 randomV = Random.insideUnitCircle;
		randomV.Normalize();
		direction = new Vector3(randomV.x, 0, randomV.y);
		GetComponent<Rigidbody> ().velocity = direction * speed *random;

		partsCollector = GameObject.Find("PartsCollector").GetComponent<PartsCollector>();
	}

	void Update () {
		if(time > 0) {
			// Reduce the remaining time by time passed since last update (frame)
			time -= Time.deltaTime;
		} else {
			timeelapsed = true;

			Vector3 heading =  GameObject.FindWithTag("PartCollector").transform.position - transform.position;
			heading /= heading.magnitude;
			GetComponent<Rigidbody>().velocity = heading * GameObject.Find("Player").GetComponent<Rigidbody>().velocity.magnitude * 3;

		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "PartCollector" && timeelapsed) {
			Destroy (gameObject);
			partsCollector.effect();
		}
	}

}

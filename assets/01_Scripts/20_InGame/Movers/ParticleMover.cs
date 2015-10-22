using UnityEngine;
using System.Collections;

public class ParticleMover : MonoBehaviour {
	public float startScale = 2;
	public float minScale = 2;
	public float maxScale = 3;

	public float speed = 30;
	public float startTime = 0.5f;
	private float time;
	public float tumble = 1;
	public float baseSpeed = 200;
	private bool rainbow = false;
	private float random;
	private Vector3 direction;
	private bool timeelapsed = false;
	private PartsCollector partsCollector;
	private PlayerMover player;
	private Rigidbody rb;

	private bool isTriggeringCubesGet = false;
	private bool generatedByPlayer = true;

	private int howMany = 0;

	void Awake() {
		rb = GetComponent<Rigidbody>();
		partsCollector = GameObject.Find("CubeCollector").GetComponent<PartsCollector>();
		player = GameObject.Find("Player").GetComponent<PlayerMover>();
	}

	void OnEnable () {
		random = Random.Range (minScale, maxScale);

		transform.localScale = startScale * random * Vector3.one;

		rb.angularVelocity = Random.onUnitSphere * tumble;

		Vector2 randomV = Random.insideUnitCircle;
		randomV.Normalize();
		direction = new Vector3(randomV.x, 0, randomV.y);
		rb.velocity = direction * speed * random;

		time = startTime;
		isTriggeringCubesGet = false;
		generatedByPlayer = true;
		timeelapsed = false;
		rainbow = false;
	}

	void FixedUpdate () {
		if(time > 0) {
			time -= Time.deltaTime;
		} else {
			timeelapsed = true;

			Vector3 heading =  partsCollector.transform.position - transform.position;
			heading /= heading.magnitude;
			if (rainbow && player.isUsingRainbow()) {
				rb.velocity = heading * player.getSpeed() * 3;
			} else {
				rb.velocity = heading * (baseSpeed + player.getSpeed() * 2) ;
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "PartCollector" && timeelapsed) {
			gameObject.SetActive(false);
			partsCollector.effect(isTriggeringCubesGet, howMany, generatedByPlayer);
		}
	}

	public void triggerCubesGet(int count, bool playerGeneration = true) {
		isTriggeringCubesGet = true;
		generatedByPlayer = playerGeneration;
		howMany = count;
	}

	public void setRainbow() {
		rainbow = true;
	}
}

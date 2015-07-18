using UnityEngine;
using System.Collections;

public class BigObstacleMover : MonoBehaviour {

	public ParticleSystem boosterPs;
	public ParticleSystem firePs;
	public EnergyBar energyBar;
	public SphereMover sphereMover;
	Vector3 direction;
	Vector3 normal;
	public Rigidbody rb;
	public float speed;
	const float pi = 3.1415f;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();

		float seed = Random.Range (0.0f, 1.0f) * 2 * pi;

		direction = new Vector3 (Mathf.Cos(seed),
		                         0.0f,
		                         Mathf.Sin(seed));
		direction.Normalize ();

		rb.AddForce (direction * speed);
	}

	// Update is called once per frame
	void Update () {

	}


	void FixedUpdate () {
		GetComponent<Rigidbody> ().velocity = speed * direction;
	}



	void OnCollisionEnter(Collision collision)
	{

		if (collision.collider.tag == "Obstacle_big") {


			processCollision(collision);

		} else if (collision.collider.tag == "Obstacle_middle") {

			// processCollision(collision);

		}else if (collision.collider.tag == "Obstacle_small") {

			// processCollision(collision);

		}else if (collision.collider.tag == "Energy") {


			processCollision(collision);
			//Destroy(collision.collider.gameObject);

		}else if (collision.collider.tag == "Part") {

			Destroy(collision.collider.gameObject);

		}



	}


	public void processCollision(Collision collision) {
		ContactPoint contact = collision.contacts[0];
		normal = contact.normal;
		direction = Vector3.Reflect(direction, -normal).normalized;
		direction.y = 0;
		direction.Normalize();
	}


}

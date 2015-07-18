using UnityEngine;
using System.Collections;

public class SphereMover : MonoBehaviour {
	public ParticleSystem boosterPs;
	public ParticleSystem firePs;
	public ParticleSystem full;
	public ParticleSystem explo;
	public EnergyBar energyBar;
	public PartsCount partsCount;
	public GameController gameController;

	public float torque;
	public Rigidbody rb;

	private int score = 0;
	public int scored;

	public int tumble = 10;

	Vector3 torquee;

	Vector3 direction;
	Vector3 normal;
	public float velocity=20.0f;
	bool isHold = false;
	SphereSpeedModel speedModel;
	const float pi = 3.1415f;
	bool fixCollisionStay = false;

	void Update(){

		scored = score;
	}

	void Start() {
		rb = GetComponent<Rigidbody>();

	}

	public void run() {
		//ps = GetComponentInChildren<ParticleSystem> ();
		speedModel = new SphereSpeedModel (20, 2, 3, 100);
		float seed = Random.Range (0.0f, 1.0f) * 2 * pi;
		direction = new Vector3 (
			Mathf.Cos(seed),
			0.0f,
			Mathf.Sin(seed));
		direction.Normalize();

		torquee= new Vector3 (Mathf.Cos(seed),
		                      0.0f,
		                      Mathf.Sin(seed));
		rb.AddTorque(torquee);

		GetComponent<SphereCollider> ().contactOffset = 0.005f;
		Rigidbody sphereBody = GetComponent<Rigidbody> ();
		// sphereBody.AddForce (velocity * direction);
		sphereBody.velocity = new Vector3(1, 1, 1) * speedModel.getSpeed ();

		rotateSphereBody();

		//sphereBody.AddTorque (200 * velocity * -direction, ForceMode.VelocityChange);
		GameInfoMaker.instance.ballVelocity = (int) velocity;
		GameInfoMaker.notifyUpdate();
		//ps.enableEmission = false;
		boosterPs.Play();
		//ps.enableEmission = true;
	}

	public void rotateSphereBody() {
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
	}

	public void hold() {
		isHold = true;
	}

	public void unhold() {
		isHold = false;
	}

	public void setDirection(Vector3 value) {
		direction = value;
	}

	public void setRotation(Vector3 value) {
		direction = value;
	}

	void FixedUpdate() {
		if (isHold == false) {
			GetComponent<Rigidbody> ().velocity = velocity * direction;
		} else {
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}
	}
	//Obstacle_smaller
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Obstacle_big") {
			//explo.Play ();
			gameController.GameOver();

		}else if (other.tag == "Obstacle_small") {
			//explo.Play ();
			gameController.GameOver();
		} else if (other.tag == "Obstacle_middle") {
			//explo.Play ();
			gameController.GameOver();
		}   else if (other.tag == "Obstacle_smaller") {
			//explo.Play ();
			gameController.GameOver();
		}else if (other.tag == "Energy") {

			energyBar.getHealth ();
			full.Play ();
			GetComponent<AudioSource>() .Play ();

			Destroy (other.gameObject);
		} else if (other.tag == "Part") {
			energyBar.getHealthbyParts();
			score += 1;
			full.Play ();
			GetComponent<AudioSource>() .Play ();
			Destroy (other.gameObject);
			partsCount.addCount();
		}
	}


	public void processCollision(Collision collision) {
		ContactPoint contact = collision.contacts[0];
		normal = contact.normal;
		Debug.DrawRay(contact.point, velocity * normal, Color.green, 1.0f);
		Debug.DrawRay (contact.point, velocity * direction, Color.red, 1.0f);
		direction = Vector3.Reflect(direction, -normal).normalized;
		direction.y = 0;
		direction.Normalize();
		Debug.DrawRay(contact.point, velocity * direction, Color.blue, 1.0f);
	}

	public class SphereSpeedModel {
		private float baseVal, xMin, xVal, multVal, speed;
		public SphereSpeedModel(float baseVal, float xVal, float xMin, float multVal) {
			this.baseVal = baseVal;
			this.xMin = xMin;
			this.xVal = xVal;
			this.multVal = multVal;
			speed = Mathf.Log(xVal, baseVal) * multVal;
		}
		public float moveX(float dx) {
			xVal += dx;
			if (xVal < xMin)
				xVal = xMin;
			speed = Mathf.Log(xVal, baseVal) * multVal;
			return speed;
		}
		public float getSpeed() {
			return speed;
		}
	}

}

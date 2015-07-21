using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour {
	public ParticleSystem getEnergy;
	public ParticleSystem booster;
	public EnergyBar energyBar;
	public ComboBar comboBar;
	public PartsCount partsCount;
	public GameOver gameOver;
	public Transform energyDestroy;

	public float speed;
  public float tumble;

	private Vector3 direction;

	void Start () {
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);
    GetComponent<Rigidbody> ().velocity = direction * speed;
	}

	void Update () {
		speed = comboBar.moverspeed;
	}

	void FixedUpdate () {
    GetComponent<Rigidbody> ().velocity = direction * speed;

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Obstacle") {
			gameOver.run();
		} else if (other.tag == "Part") {
			GetComponent<AudioSource>().Play ();
			getEnergy.Play ();
			Instantiate(energyDestroy, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);

			energyBar.getHealthbyParts();
			partsCount.addCount();

			comboBar.addCombo();
		} else if (other.tag == "SpecialPart") {
			other.gameObject.GetComponent<GenerateNextSpecial>().spawnNext();
			partEncounter(other);
		}
	}

	void partEncounter(Collider part) {
		GetComponent<AudioSource>().Play ();
		getEnergy.Play ();
		Instantiate(energyDestroy, part.transform.position, part.transform.rotation);
		Destroy (part.gameObject);

		energyBar.getHealthbyParts();
		partsCount.addCount();

		comboBar.addCombo();
	}

	public void rotatePlayerBody() {
		GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;
	}

	public void setDirection(Vector3 value) {
    direction = value;
  }
}

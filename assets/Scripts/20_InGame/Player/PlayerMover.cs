using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour {
	public ParticleSystem getEnergy;
	public ParticleSystem booster;
	public ParticleSystem unstoppableEffect;
	public ParticleSystem unstoppableEffect_two;
	public EnergyBar energyBar;
	public ComboBar comboBar;
	public PartsCount partsCount;
	public GameOver gameOver;
	public Transform energyDestroy;
	public GameObject obstacleDestroy;
	public GameObject unstoppableSphere;

	public float speed;
  public float tumble;

	private Vector3 direction;
	private bool unstoppable = false;
	private float unstoppable_during = 0;
	public int unstoppable_speed = 150;
	public float unstoppable_time_scale = 1;
	public int max_unstoppable_combo = 10;

	GameObject nextSpecialTry;

	void Start () {
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);
    GetComponent<Rigidbody> ().velocity = direction * speed;
	}

	void Update () {
		if (unstoppable) {
			speed = unstoppable_speed;
		}
		else {
			speed = comboBar.moverspeed;
		}
	}

	void FixedUpdate () {
    GetComponent<Rigidbody> ().velocity = direction * speed;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Obstacle") {
			if (unstoppable) {
				Instantiate(obstacleDestroy, other.transform.position, other.transform.rotation);
				Destroy(other.gameObject);
			} else {
				gameOver.run();
			}
		} else if (other.tag == "Part") {
			goodPartsEncounter();
			Instantiate(energyDestroy, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
		} else if (other.tag == "SpecialPart") {
			GenerateNextSpecial gns = other.gameObject.GetComponent<GenerateNextSpecial>();
			if (gns.getComboCount() == (max_unstoppable_combo - 1)) {
				startUnstoppable(max_unstoppable_combo);
			} else {
				nextSpecialTry = gns.spawnNext();
			}
			goodPartsEncounter();
      gns.destroySelf(true, false, false);
		}
	}

  private void goodPartsEncounter() {
    GetComponent<AudioSource>().Play ();
    getEnergy.Play ();
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

  public GameObject getNextSpecialTry() {
  	return nextSpecialTry;
  }

  public void startUnstoppable(int comboCount) {
  	unstoppable = true;
  	unstoppable_during = comboCount * unstoppable_time_scale;
  	unstoppableEffect.Play();
		unstoppableEffect_two.Play();
  	energyBar.startUnstoppable();
  	unstoppableSphere.SetActive(true);
  	StartCoroutine("stopUnstoppable");
  }

  IEnumerator stopUnstoppable() {
  	yield return new WaitForSeconds(unstoppable_during);
  	unstoppable = false;
  	unstoppableEffect.Stop();
		unstoppableEffect_two.Stop();
  	energyBar.stopUnstoppable();
  	unstoppableSphere.SetActive(false);
		GameObject.Find("Field Objects").GetComponent<SpecialObjectsManager>().run();
  }

  public bool isUnstoppable() {
    return unstoppable;
  }
}

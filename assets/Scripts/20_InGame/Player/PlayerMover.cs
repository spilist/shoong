using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour {
	public PartsCount partsCount;
  public GameOver gameOver;
  public Transform energyDestroy;
  public GameObject obstacleDestroy;
  public GameObject unstoppableSphere;

	public float speed;
  public float tumble;
	private Vector3 direction;

  private Canvas barsCanvas;
  private EnergyBar energyBar;
  private ComboBar comboBar;
  public ParticleSystem booster;
  public ParticleSystem getEnergy;
  public ParticleSystem unstoppableEffect;
  public ParticleSystem unstoppableEffect_two;

	private bool unstoppable = false;
	private float unstoppable_during = 0;
  public float unstoppable_minbonus = 0.5f;
  public float unstoppable_end_soon_during = 1;
  public float unstoppable_blinkingSeconds = 0.2f;
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

    barsCanvas = transform.Find("Bars Canvas").GetComponent<Canvas>();
    energyBar = transform.Find("Bars Canvas/EnergyBar").GetComponent<EnergyBar>();
    comboBar = transform.Find("Bars Canvas").GetComponent<ComboBar>();
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
  	unstoppable_during = comboCount * unstoppable_time_scale + unstoppable_minbonus;
  	unstoppableEffect.Play();
		unstoppableEffect_two.Play();
  	energyBar.startUnstoppable();
  	unstoppableSphere.SetActive(true);
  	StartCoroutine("stopUnstoppable");
  }

  IEnumerator stopUnstoppable() {
    yield return new WaitForSeconds(unstoppable_during - unstoppable_end_soon_during);

    float duration = unstoppable_end_soon_during;
    while(duration > 0f) {
      duration -= unstoppable_blinkingSeconds;
      GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
      // GetComponent<TrailRenderer>().enabled = !GetComponent<TrailRenderer>().enabled;
      barsCanvas.enabled = !barsCanvas.enabled;

      yield return new WaitForSeconds(unstoppable_blinkingSeconds);
    }
    GetComponent<Renderer>().enabled = true;
    // GetComponent<TrailRenderer>().enabled = true;
    barsCanvas.enabled = true;

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

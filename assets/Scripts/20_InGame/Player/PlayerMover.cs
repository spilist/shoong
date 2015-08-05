using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour {
	public PartsCount partsCount;
  public GameOver gameOver;
  public Transform energyDestroy;
  public GameObject obstacleDestroy;
  public GameObject unstoppableSphere;
  public int cubesWhenDestroyBigObstacle = 50;
  public int cubesWhenDestroySmallObstacle = 20;
  private Hashtable cubesWhenDestroy;

  public float speed;
	private float boosterspeed;
  public float tumble;
  private Vector3 direction;

  private EnergyBar energyBar;
  private ComboBar comboBar;
  private UnstoppableComboBar uComboBar;
  private SpecialPartIndicator spIndicator;

  public ParticleSystem booster;
  public float boosterSpeedUpAmount = 120f;
  public ParticleSystem getEnergy;
  public ParticleSystem unstoppableEffect;
  public ParticleSystem unstoppableEffect_two;
  public ParticleSystem getSpecialEnergyEffect;

	public GameObject particles;

	private bool unstoppable = false;
	private float unstoppable_during = 0;
  public float unstoppable_minbonus = 0.5f;
  public float unstoppable_end_soon_during = 1;
  public float unstoppable_blinkingSeconds = 0.2f;
  public float[] unstoppable_respawn;
	public int unstoppable_speed = 150;
	public float unstoppable_time_scale = 1;
	public int max_unstoppable_combo = 10;

	GameObject nextSpecialTry;

  // experiments
  private bool stopNow = false;
  Vector3 moveToHere;

	void Start () {
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);
    GetComponent<Rigidbody> ().velocity = direction * speed;

    energyBar = transform.Find("Bars Canvas/EnergyBar").GetComponent<EnergyBar>();
    comboBar = transform.Find("Bars Canvas").GetComponent<ComboBar>();
    uComboBar = transform.Find("Bars Canvas/UnstoppableComboBar").GetComponent<UnstoppableComboBar>();
    spIndicator = GameObject.Find("SpecialPart Indicator").GetComponent<SpecialPartIndicator>();

    cubesWhenDestroy = new Hashtable();
    cubesWhenDestroy.Add("Obstacle_big", cubesWhenDestroyBigObstacle);
    cubesWhenDestroy.Add("Obstacle", cubesWhenDestroySmallObstacle);
	}

  public void moveTo(Vector3 touchPosition) {
    moveToHere = touchPosition;
  }

	void FixedUpdate () {
		if (unstoppable) {
			speed = unstoppable_speed;
		}
		else {
			speed = comboBar.moverspeed+boosterspeed;
		}

		if (boosterspeed > 0) {
			boosterspeed -= speed / 70.0f + 20 * Time.deltaTime;
		} else if (boosterspeed < 0){
			boosterspeed = 0;
		}

    // if (!unstoppable & (Vector3.Distance(moveToHere, transform.position) < 3)) {
      // GetComponent<Rigidbody> ().velocity = Vector3.zero;
    // } else {
  		GetComponent<Rigidbody> ().velocity = direction * speed;
    // }
	}

	public void boosterSpeedup(){
		boosterspeed += boosterSpeedUpAmount;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Obstacle" || other.tag == "Obstacle_big") {
			if (unstoppable) {
				Instantiate(obstacleDestroy, other.transform.position, other.transform.rotation);
        for (int k = 0; k < (int)cubesWhenDestroy[other.tag]; k++) {
          Instantiate(particles, other.transform.position, other.transform.rotation);
        }
        energyBar.getHealthbyParts((int)cubesWhenDestroy[other.tag]/2);
        partsCount.addCount((int)cubesWhenDestroy[other.tag]);
        other.GetComponent<AudioSource>().Play();
        Destroy(other.gameObject);
			} else {
				gameOver.run();
			}
		} else if (other.tag == "Part") {
      goodPartsEncounter(other.transform);
      getEnergy.Play ();
      Destroy(other.gameObject);
		} else if (other.tag == "SpecialPart") {
			GenerateNextSpecial gns = other.gameObject.GetComponent<GenerateNextSpecial>();
			if (gns.getComboCount() == (max_unstoppable_combo - 1)) {
				startUnstoppable(max_unstoppable_combo);
			} else {
				nextSpecialTry = gns.spawnNext();
			}
			goodPartsEncounter(other.transform);
      gns.destroySelf(true, false, false);
      uComboBar.addCombo();
		} else if (other.tag == "PatternPart") {
      PatternPartsMover mover = other.gameObject.GetComponent<PatternPartsMover>();
      if (!mover.isActive()) {
        goodPartsEncounter(other.transform);
        mover.becomeActive();
      }
    }
	}

  private void goodPartsEncounter(Transform tr) {
    for (int e = 0; e < comboBar.getComboRatio(); e++) {
      Instantiate(particles, tr.position, tr.rotation);
    }
    GetComponent<AudioSource>().Play ();
    partsCount.addCount();
    energyBar.getHealthbyParts(comboBar.getComboRatio());
    comboBar.addCombo();
    stopNow = true;
  }

	public void rotatePlayerBody() {
		GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;
    stopNow = false;
	}

	public void setDirection(Vector3 value) {
    direction = value;
  }

  public Vector3 getDirection() {
    return direction;
  }

  public GameObject getNextSpecialTry() {
  	return nextSpecialTry;
  }

  public void startUnstoppable(int comboCount) {
  	if (gameOver.isOver()) return;

    unstoppable = true;
  	unstoppable_during = comboCount * unstoppable_time_scale + unstoppable_minbonus;
  	unstoppableEffect.Play();
		unstoppableEffect_two.Play();
  	energyBar.startUnstoppable();
    uComboBar.startUnstoppable();
    unstoppableSphere.SetActive(true);
    spIndicator.stopIndicate();
  	StartCoroutine("stopUnstoppable");
  }

  IEnumerator stopUnstoppable() {
    yield return new WaitForSeconds(unstoppable_during - unstoppable_end_soon_during);

    float duration = unstoppable_end_soon_during;
    while(duration > 0f) {
      duration -= unstoppable_blinkingSeconds;
      unstoppableEffect.enableEmission = !unstoppableEffect.enableEmission;

      yield return new WaitForSeconds(unstoppable_blinkingSeconds);
    }
    unstoppableEffect.enableEmission = true;

    unstoppable = false;
  	unstoppableEffect.Stop();
		unstoppableEffect_two.Stop();
  	energyBar.stopUnstoppable();
  	unstoppableSphere.SetActive(false);


    yield return new WaitForSeconds(Random.Range(unstoppable_respawn[0], unstoppable_respawn[1]));
    GameObject.Find("Field Objects").GetComponent<SpecialObjectsManager>().run();
  }

  public bool isUnstoppable() {
    return unstoppable;
  }

  public void getSpecialEnergyPlay() {
    getSpecialEnergyEffect.Play();
  }

  public EnergyBar getEnergyBar() {
    return energyBar;
  }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour {
	public PartsCount partsCount;
  public GameOver gameOver;
  public Transform energyDestroy;
  public GameObject obstacleDestroy;
  public GameObject unstoppableSphere;
  public int cubesWhenDestroyBigObstacle = 30;
  public int cubesWhenDestroySmallObstacle = 15;
  public int cubesWhenDestroyMonster = 50;
  private Hashtable cubesWhenDestroy;

  public float speed;
	private float boosterspeed;
  public float tumble;
  private Vector3 direction;

  public FieldObjectsManager fom;
  public ComboPartsManager cpm;
  public MonsterManager monm;

  private EnergyBar energyBar;
  private ComboBar comboBar;
  private UnstoppableComboBar uComboBar;

  public ParticleSystem booster;
  public float boosterSpeedUpAmount = 120f;
  public ParticleSystem getEnergy;
  public ParticleSystem unstoppableEffect;
  public ParticleSystem unstoppableEffect_two;
  public ParticleSystem getSpecialEnergyEffect;

	public GameObject particles;

	private bool unstoppable = false;
	public float unstoppable_during = 8f;
  public float unstoppable_end_soon_during = 1;
  public float unstoppable_blinkingSeconds = 0.2f;
  public float[] unstoppable_respawn;
	public int unstoppable_speed = 150;

	void Start () {
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);
    GetComponent<Rigidbody> ().velocity = direction * speed;

    energyBar = transform.Find("Bars Canvas/EnergyBar").GetComponent<EnergyBar>();
    comboBar = transform.Find("Bars Canvas").GetComponent<ComboBar>();
    uComboBar = transform.Find("Bars Canvas/UnstoppableComboBar").GetComponent<UnstoppableComboBar>();

    cubesWhenDestroy = new Hashtable();
    cubesWhenDestroy.Add("Obstacle_big", cubesWhenDestroyBigObstacle);
    cubesWhenDestroy.Add("Obstacle", cubesWhenDestroySmallObstacle);
    cubesWhenDestroy.Add("Monster", cubesWhenDestroyMonster);
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

		GetComponent<Rigidbody> ().velocity = direction * speed;
	}

	public void boosterSpeedup(){
		boosterspeed += boosterSpeedUpAmount;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Obstacle" || other.tag == "Obstacle_big" || other.tag == "Monster") {
			if (unstoppable) {
				Instantiate(obstacleDestroy, other.transform.position, other.transform.rotation);
        goodPartsEncounter(other.transform, (int)cubesWhenDestroy[other.tag], false);
        // other.GetComponent<AudioSource>().Play();
			} else {
				gameOver.run();
			}
		} else if (other.tag == "Part") {
      goodPartsEncounter(other.transform, comboBar.getComboRatio(), true);
		} else if (other.tag == "SpecialPart") {
      goodPartsEncounter(other.transform, comboBar.getComboRatio(), true);
      getSpecialEnergyEffect.Play();
      startUnstoppable();
		} else if (other.tag == "ComboPart") {
      cpm.eatenByPlayer();
      goodPartsEncounter(other.transform, cpm.getComboCount() * cpm.comboBonusScale, true);
    }
	}

  private void goodPartsEncounter(Transform tr, int howMany, bool audio) {
    for (int e = 0; e < howMany; e++) {
      Instantiate(particles, tr.position, tr.rotation);
    }
    partsCount.addCount(howMany);
    energyBar.getHealthbyParts(howMany);
    getEnergy.Play ();
    comboBar.addCombo();
    if (audio) GetComponent<AudioSource>().Play ();

    Destroy(tr.gameObject);
  }

	public void rotatePlayerBody() {
		GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;
	}

	public void setDirection(Vector3 value) {
    direction = value;
  }

  public Vector3 getDirection() {
    return direction;
  }

  public void startUnstoppable() {
  	if (gameOver.isOver()) return;

    unstoppable = true;
  	unstoppableEffect.Play();
		unstoppableEffect_two.Play();
  	energyBar.startUnstoppable();
    uComboBar.startUnstoppable();
    unstoppableSphere.SetActive(true);
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
    fom.spawnSpecial();
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

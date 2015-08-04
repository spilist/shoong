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
	private float boosterspeed;
  public float tumble;
  private Vector3 direction;

  private EnergyBar energyBar;
  private ComboBar comboBar;
  private UnstoppableComboBar uComboBar;

  public ParticleSystem booster;
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

	void Start () {
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);
    GetComponent<Rigidbody> ().velocity = direction * speed;

    energyBar = transform.Find("Bars Canvas/EnergyBar").GetComponent<EnergyBar>();
    comboBar = transform.Find("Bars Canvas").GetComponent<ComboBar>();
    uComboBar = transform.Find("Bars Canvas/UnstoppableComboBar").GetComponent<UnstoppableComboBar>();
	}

	void Update () {
	
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
			Debug.Log (boosterspeed);
		} else if (boosterspeed < 0){
			boosterspeed = 0;
		}
		GetComponent<Rigidbody> ().velocity = direction * speed;
	}

	public void boosterSpeedup(){
		boosterspeed += 60.0f;
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

  public Vector3 getDirection() {
    return direction;
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

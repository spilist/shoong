﻿using UnityEngine;
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

  public BlackholeManager blm;
  private GameObject blackhole;
  private bool isInsideBlackhole = false;
  private bool exitedBlackhole = false;
  private bool rebounding = false;

  private EnergyBar energyBar;
  private ComboBar comboBar;
  private StrengthenTimeBar stBar;

  public ParticleSystem booster;
  public float boosterSpeedUpAmount = 60;
  public ParticleSystem getEnergy;
  public ParticleSystem unstoppableEffect;
  public ParticleSystem unstoppableEffect_two;
  public ParticleSystem getSpecialEnergyEffect;
	public ParticleSystem getComboParts;

	public GameObject particles;

  public int strengthen_during = 8;
	public float strengthen_speed = 120;

  private bool unstoppable = false;
  public float[] unstoppable_respawn;

	void Start () {
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);
    GetComponent<Rigidbody> ().velocity = direction * speed;

    energyBar = transform.Find("Bars Canvas/EnergyBar").GetComponent<EnergyBar>();
    comboBar = transform.Find("Bars Canvas").GetComponent<ComboBar>();
    stBar = transform.Find("Bars Canvas/StrengthenTimeBar").GetComponent<StrengthenTimeBar>();

    cubesWhenDestroy = new Hashtable();
    cubesWhenDestroy.Add("Obstacle_big", cubesWhenDestroyBigObstacle);
    cubesWhenDestroy.Add("Obstacle", cubesWhenDestroySmallObstacle);
    cubesWhenDestroy.Add("Monster", cubesWhenDestroyMonster);
	}

	void FixedUpdate () {
    if (rebounding) {
      speed = blm.reboundSpeed;
    } else if (unstoppable || exitedBlackhole) {
      speed = strengthen_speed;
    } else {
			speed = comboBar.moverspeed;
    }

    speed += boosterspeed;

    if (boosterspeed > 0) {
			boosterspeed -= speed / 70.0f + 20 * Time.deltaTime;
		} else if (boosterspeed <= 0){
			boosterspeed = 0;
		}

    if (isInsideBlackhole) {
      Vector3 heading = blackhole.transform.position - transform.position;
      heading /= heading.magnitude;
      GetComponent<Rigidbody> ().AddForce(heading * blm.pullUser, ForceMode.VelocityChange);
    }
    GetComponent<Rigidbody> ().velocity = direction * speed;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Obstacle" || other.tag == "Obstacle_big" || other.tag == "Monster") {
			if (unstoppable) {
				Instantiate(obstacleDestroy, other.transform.position, other.transform.rotation);
        goodPartsEncounter(other.transform, (int)cubesWhenDestroy[other.tag]);
			} else if (exitedBlackhole && other.tag == "Monster") {
        Debug.Log("I'm on a monster");
        Destroy(other.gameObject);
      } else {
				gameOver.run();
			}
		} else if (other.tag == "Part") {
      goodPartsEncounter(other.transform, comboBar.getComboRatio());
			GetComponent<AudioSource>().Play ();
		} else if (other.tag == "SpecialPart") {
      goodPartsEncounter(other.transform, comboBar.getComboRatio());
      getSpecialEnergyEffect.Play();
      // what if player is exited blackhole?
      startUnstoppable();
		} else if (other.tag == "ComboPart") {
      cpm.eatenByPlayer();
			getComboParts.Play();
			getComboParts.GetComponent<AudioSource>().Play ();
      goodPartsEncounter(other.transform, cpm.getComboCount() * cpm.comboBonusScale);
    }
	}

  private void goodPartsEncounter(Transform tr, int howMany) {
    for (int e = 0; e < howMany; e++) {
      Instantiate(particles, tr.position, tr.rotation);
    }
    partsCount.addCount(howMany);
    energyBar.getHealthbyParts(howMany);
    getEnergy.Play ();
    comboBar.addCombo();

    Destroy(tr.gameObject);
  }

  IEnumerator strengthen() {
    if (gameOver.isOver()) yield break;

    int effectDuration;
    if (rebounding) {
      effectDuration = blm.reboundDuring;
    } else {
      effectDuration = strengthen_during;
    }

    if (unstoppable) {
      unstoppableEffect.Play();
      unstoppableEffect.GetComponent<AudioSource>().Play ();
      unstoppableEffect_two.Play();
      energyBar.startUnstoppable();
    }

    if (exitedBlackhole) {
      unstoppableSphere.SetActive(true);
    }

    stBar.startStrengthen(effectDuration);

    yield return new WaitForSeconds(effectDuration);

    if (unstoppable) {
      fom.spawnSpecial(Random.Range(unstoppable_respawn[0], unstoppable_respawn[1]));
    }

    rebounding = false;
    unstoppable = false;
    unstoppableEffect.Stop();
    unstoppableEffect_two.Stop();
    energyBar.stopUnstoppable();
    exitedBlackhole = false;
    unstoppableSphere.SetActive(false);
  }

  public void startUnstoppable() {
    unstoppable = true;
  	StopCoroutine("strengthen");
    StartCoroutine("strengthen");
  }

  public void insideBlackhole() {
    isInsideBlackhole = true;
    blackhole = blm.getBlackhole();
  }

  public void outsideBlackhole() {
    isInsideBlackhole = false;
    exitedBlackhole = true;
    Destroy(blackhole);

    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
  }

  public void contactBlackholeWhileUnstoppable(Collision collision) {
    rebounding = true;
    isInsideBlackhole = false;

    ContactPoint contact = collision.contacts[0];
    Vector3 normal = contact.normal;
    direction = Vector3.Reflect(direction, -normal).normalized;
    direction.y = 0;
    direction.Normalize();

    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
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

  public bool isUnstoppable() {
    return unstoppable;
  }

  public void getSpecialEnergyPlay() {
    getSpecialEnergyEffect.Play();
  }

  public EnergyBar getEnergyBar() {
    return energyBar;
  }

  public bool isRebounding() {
    return rebounding;
  }

  public void boosterSpeedup(){
    boosterspeed += boosterSpeedUpAmount;
  }
}

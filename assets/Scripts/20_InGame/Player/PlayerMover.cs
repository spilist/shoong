using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour {
  public SpawnManager spawnManager;

  public CubesCount cubesCount;
  public GameOver gameOver;
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

  public ComboPartsManager cpm;
  public MonsterManager monm;
  public RainbowDonutsManager rdm;

  public BlackholeManager blm;
  private GameObject blackhole;
  private bool isInsideBlackhole = false;
  private bool exitedBlackhole = false;
  private bool reboundingByBlackhole = false;

  private bool reboundingByDispenser = false;
  private float reboundingByDispenserDuring = 0;

  private EnergyBar energyBar;
  private ComboBar comboBar;
  private StrengthenTimeBar stBar;

  public Transform playerParticlesParent;
  public ParticleSystem booster;
  public ParticleSystem getEnergy;
  public ParticleSystem unstoppableEffect;
  public ParticleSystem unstoppableEffect_two;
  public ParticleSystem getSpecialEnergyEffect;
	public ParticleSystem getComboParts;
  public ParticleSystem getBlackhole;
  public ParticleSystem rainbowEffect;

  public float boosterSpeedUpAmount = 60;
  public float maxBoosterSpeed = 100;

	public GameObject particles;

  public int strengthen_during = 8;
	public float strengthen_speed = 120;
  public float reboundSpeed = 300;

  private bool unstoppable = false;
  public float[] unstoppable_respawn;

  public Mesh monsterMesh;
  public Material monsterMaterial;
  public ParticleSystem ridingEffect;
  private bool ridingMonster = false;
  private Mesh originalMesh;
  private Material originalMaterial;

  private bool isRotatingByRainbow = false;
  private bool isRidingRainbowRoad = false;
  private Vector3 rainbowPosition;

	void Start () {
    changeCharacter(PlayerPrefs.GetString("SelectedCharacter"));

    originalMesh = GetComponent<MeshFilter>().sharedMesh;
    originalMaterial = GetComponent<Renderer>().sharedMaterial;

    rotatePlayerBody();

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
    if (isRidingRainbowRoad) {
      speed = rdm.ridingSpeed;
    } else if (isRebounding()) {
      speed = reboundSpeed;
    } else if (unstoppable || exitedBlackhole || ridingMonster) {
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

    if (isInsideBlackhole && !isUsingRainbow()) {
      Vector3 heading = blackhole.transform.position - transform.position;
      heading /= heading.magnitude;
      GetComponent<Rigidbody> ().AddForce(heading * blm.pullUser, ForceMode.VelocityChange);
    }
    GetComponent<Rigidbody> ().velocity = direction * speed;
	}

	void OnTriggerEnter(Collider other) {
		ObjectsMover mover = other.gameObject.GetComponent<ObjectsMover>();

    if (other.tag == "Obstacle" || other.tag == "Obstacle_big") {
      if (unstoppable || ridingMonster || isUsingRainbow()) {
        goodPartsEncounter(mover, (int)cubesWhenDestroy[other.tag]);
      } else {
        gameOver.run();
      }
    } else if (other.tag == "Monster") {
			if (unstoppable || isUsingRainbow()) {
        goodPartsEncounter(mover, (int)cubesWhenDestroy[other.tag]);
			} else if (exitedBlackhole || other.gameObject.GetComponent<MonsterMover>().isWeak()) {
        startRiding(mover);
      } else {
				gameOver.run();
			}
		} else if (other.tag == "Part") {
      goodPartsEncounter(mover, comboBar.getComboRatio());
			GetComponent<AudioSource>().Play ();
		} else if (other.tag == "SpecialPart") {
      goodPartsEncounter(mover, comboBar.getComboRatio());
      getSpecialEnergyEffect.Play();
      startUnstoppable();
		} else if (other.tag == "ComboPart") {
      cpm.eatenByPlayer();
			getComboParts.Play();
			getComboParts.GetComponent<AudioSource>().Play ();
      goodPartsEncounter(mover, cpm.getComboCount() * cpm.comboBonusScale);
    } else if (other.tag == "RainbowDonut") {
      rainbowPosition = other.transform.position;
      goodPartsEncounter(mover, rdm.cubesPerRide, true);
    }
	}

  public void goodPartsEncounter(ObjectsMover mover, int howMany, bool rainbow = false) {
    for (int e = 0; e < howMany; e++) {
      GameObject cube = (GameObject) Instantiate(particles, mover.transform.position, mover.transform.rotation);
      if (rainbow) {
        cube.GetComponent<Renderer>().material.SetColor("_TintColor", rdm.rainbowColors[e]);
      }
    }
    cubesCount.addCount(howMany);
    energyBar.getHealthbyParts(howMany);
    getEnergy.Play ();
    comboBar.addCombo();

    mover.encounterPlayer();
  }

  public void contactCubeDispenser(Transform tr, int howMany, Collision collision, float reboundDuring) {
    for (int e = 0; e < howMany; e++) {
      Instantiate(particles, tr.position, tr.rotation);
    }
    cubesCount.addCount(howMany);
    energyBar.getHealthbyParts(howMany);
    getEnergy.Play ();
    comboBar.addCombo();
    processCollision(collision);
    reboundingByDispenser = true;
    reboundingByDispenserDuring = reboundDuring;
    StartCoroutine("stopReboundingByDispenser");
  }

  IEnumerator stopReboundingByDispenser() {
    yield return new WaitForSeconds(reboundingByDispenserDuring);
    reboundingByDispenser = false;
  }

  IEnumerator strengthen() {
    if (gameOver.isOver()) yield break;

    int effectDuration;
    if (reboundingByBlackhole) {
      effectDuration = blm.reboundDuring;
      unstoppableSphere.SetActive(true);
    } else {
      effectDuration = strengthen_during;
    }

    if (unstoppable) {
      unstoppableEffect.Play();
      unstoppableEffect.GetComponent<AudioSource>().Play ();
      unstoppableEffect_two.Play();
    }

    if (exitedBlackhole) {
      unstoppableSphere.SetActive(true);
      getBlackhole.Play();
    }

    if (ridingMonster) {
      energyBar.getFullHealth();
      changeCharacter(monsterMesh, monsterMaterial);
    }

    stBar.startStrengthen(effectDuration);

    yield return new WaitForSeconds(effectDuration);

    if (unstoppable) {
      spawnManager.runManager("SpecialParts");
      unstoppable = false;
      unstoppableEffect.Stop();
      unstoppableEffect_two.Stop();
    }

    if (reboundingByBlackhole) {
      reboundingByBlackhole = false;
      unstoppableSphere.SetActive(false);
      if (isRidingRainbowRoad) {
        isRidingRainbowRoad = false;
        rainbowEffect.Stop();
      }
    }

    if (exitedBlackhole) {
      exitedBlackhole = false;
      unstoppableSphere.SetActive(false);
      blm.run();
    }

    if (ridingMonster) {
      energyBar.getFullHealth();
      ridingMonster = false;
      changeCharacter(originalMesh, originalMaterial);
      ridingEffect.Play();
      monm.monsterFilter.SetActive(false);
      monm.run();
    }
  }

  void startRiding(ObjectsMover obMover) {
    ridingMonster = true;
    monm.monsterFilter.SetActive(true);
    ridingEffect.Play();
    Destroy(obMover.gameObject);
    monm.stopWarning();
    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
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
    reboundingByBlackhole = true;
    isInsideBlackhole = false;
    processCollision(collision);

    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
  }

  public void contactBlackholeWhileRainbow(Collision collision) {
    reboundingByBlackhole = true;
    isInsideBlackhole = false;
    processCollision(collision);
    rdm.destroyInstances();

    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
  }

  public void processCollision(Collision collision) {
    ContactPoint contact = collision.contacts[0];
    Vector3 normal = contact.normal;
    direction = Vector3.Reflect(direction, -normal).normalized;
    direction.y = 0;
    direction.Normalize();
  }

  public void rotatePlayerBody() {
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;
  }

  public Vector3 getDirection() {
    return direction;
  }

  public bool isUnstoppable() {
    return unstoppable;
  }

  public bool isRiding() {
    return ridingMonster;
  }

  public bool isExitedBlackhole() {
    return exitedBlackhole;
  }

  public void getSpecialEnergyPlay() {
    getSpecialEnergyEffect.Play();
  }

  public EnergyBar getEnergyBar() {
    return energyBar;
  }

  public bool isRebounding() {
    return reboundingByBlackhole || reboundingByDispenser;
  }

  public void shootBooster(Vector3 dir){
    if (!unstoppable) {
      energyBar.loseByShoot();
      comboBar.loseByShoot();
    }

    rotatePlayerBody();

    booster.Play();
    booster.GetComponent<AudioSource>().Play();

    direction = dir;

    boosterspeed += boosterSpeedUpAmount;
    if (boosterspeed > maxBoosterSpeed) boosterspeed = maxBoosterSpeed;

    cpm.tryToGet();
  }

  public void changeCharacter(Mesh mesh, Material material) {
    GetComponent<MeshFilter>().sharedMesh = mesh;
    GetComponent<Renderer>().sharedMaterial = material;
  }

  public void changeCharacter(string characterName) {
    GameObject play_characters = Resources.Load<GameObject>("_characters/play_characters");
    GetComponent<MeshFilter>().sharedMesh = play_characters.transform.FindChild(characterName).GetComponent<MeshFilter>().sharedMesh;

    booster = Instantiate(Resources.Load(characterName + "/Booster", typeof(ParticleSystem))) as ParticleSystem;
    booster.transform.parent = playerParticlesParent;
    booster.transform.localScale = Vector3.one;
    booster.transform.localPosition = Vector3.zero;
    booster.transform.localRotation = Quaternion.identity;
  }

  public void setRotateByRainbow(bool val) {
    if (val) GetComponent<Rigidbody>().isKinematic = true;
    else GetComponent<Rigidbody>().isKinematic = false;
    isRotatingByRainbow = val;
  }

  void Update() {
    if (isRotatingByRainbow) {
      Vector3 dir = (rainbowPosition - transform.position).normalized;
      transform.Translate(dir * Time.deltaTime * 30, Space.World);
      transform.Rotate(-Vector3.forward * Time.deltaTime * rdm.rotateAngularSpeed, Space.World);
    }
  }

  public void setRidingRainbowRoad(bool val) {
    isRidingRainbowRoad = val;
  }

  public bool isUsingRainbow() {
    return isRotatingByRainbow || isRidingRainbowRoad;
  }

  public void setDirection(Vector3 dir) {
    direction = dir;
    rotatePlayerBody();
  }

}

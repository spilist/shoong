using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour {
  public SpawnManager spawnManager;

  public CubesCount cubesCount;
  public ScoreManager scoreManager;
  public GameObject obstacleDestroy;
  public GameObject unstoppableSphere;
  public int cubesWhenDestroyBigObstacle = 30;
  public int cubesWhenDestroySmallObstacle = 15;
  public int cubesWhenDestroyMonster = 50;
  public int cubesAsItIsUntill = 20;
  public int restCubesChangePer = 5;
  private Hashtable cubesWhenDestroy;

  private float speed;
	private float boosterspeed = 0;
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
  public ParticleSystem chargedEffect;

  public float boosterSpeedUpAmount = 60;
  public float maxBoosterSpeed = 100;
  public float boosterSpeedDecreaseBase = 70;
  public float boosterSpeedDecreasePerTime = 20;

	public GameObject particles;

	public float unstoppable_speed = 120;
  public int strengthen_during = 8;
  public float reboundSpeed = 300;

  private bool unstoppable = false;
  public float[] unstoppable_respawn;

  public Mesh monsterMesh;
  public Material monsterMaterial;
  public ParticleSystem ridingEffect;
  public ParticleSystem monsterEffect;
  private bool ridingMonster = false;
  private Mesh originalMesh;
  private Material originalMaterial;

  private bool isRotatingByRainbow = false;
  private bool isRidingRainbowRoad = false;
  private Vector3 rainbowPosition;
  private Rigidbody rb;

  private int nearAsteroidCounter = 0;

	void Start () {
    changeCharacter(PlayerPrefs.GetString("SelectedCharacter"));

    originalMesh = GetComponent<MeshFilter>().sharedMesh;
    originalMaterial = GetComponent<Renderer>().sharedMaterial;
    energyBar = transform.parent.Find("Bars Canvas/EnergyBar").GetComponent<EnergyBar>();
    comboBar = transform.parent.Find("Bars Canvas").GetComponent<ComboBar>();
    stBar = transform.parent.Find("Bars Canvas/StrengthenTimeBar").GetComponent<StrengthenTimeBar>();

    rotatePlayerBody();

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);
    speed = comboBar.getSpeed();

    cubesWhenDestroy = new Hashtable();
    cubesWhenDestroy.Add("Obstacle_big", cubesWhenDestroyBigObstacle);
    cubesWhenDestroy.Add("Obstacle", cubesWhenDestroySmallObstacle);
    cubesWhenDestroy.Add("Monster", cubesWhenDestroyMonster);

    rb = GetComponent<Rigidbody>();
    rb.velocity = direction * speed;
	}

	void FixedUpdate () {
    if (isRidingRainbowRoad) {
      speed = rdm.ridingSpeed;
    } else if (isRebounding()) {
      speed = reboundSpeed;
    } else if (unstoppable) {
      speed = unstoppable_speed;
    } else {
			speed = comboBar.getSpeed();
    }

    speed += boosterspeed;

    if (boosterspeed > 0) {
			boosterspeed -= speed / boosterSpeedDecreaseBase + boosterSpeedDecreasePerTime * Time.deltaTime;
		} else if (boosterspeed <= 0){
			boosterspeed = 0;
		}

    if (isInsideBlackhole && !isUsingRainbow()) {
      Vector3 heading = blackhole.transform.position - transform.position;
      heading /= heading.magnitude;
      rb.AddForce(heading * blm.pullUser, ForceMode.VelocityChange);
    }
    rb.velocity = direction * speed;
	}

	void OnTriggerEnter(Collider other) {
		ObjectsMover mover = other.gameObject.GetComponent<ObjectsMover>();

    if (other.tag == "Obstacle" || other.tag == "Obstacle_big") {
      if (unstoppable || ridingMonster || isUsingRainbow()) {
        goodPartsEncounter(mover, (int)cubesWhenDestroy[other.tag]);
      } else {
        scoreManager.gameOver();
      }
    } else if (other.tag == "Monster") {
			if (unstoppable || isUsingRainbow()) {
        goodPartsEncounter(mover, (int)cubesWhenDestroy[other.tag]);
        if (isUsingRainbow()) {
          QuestManager.qm.addCountToQuest("DestroyMonsterByRainbow");
        }
			} else if (exitedBlackhole || other.gameObject.GetComponent<MonsterMover>().isWeak()) {
        startRiding(mover);
      } else {
				scoreManager.gameOver();
			}
		} else if (other.tag == "Part") {
      goodPartsEncounter(mover, comboBar.getComboRatio());
			GetComponent<AudioSource>().Play ();

      if (nearAsteroidCounter > 0) {
        QuestManager.qm.addCountToQuest("GetPartsNearAsteroid");
      }

      if (isUsingRainbow()) {
        QuestManager.qm.addCountToQuest("GetPartsOnRainbow");
      }

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
    int instantiateCount;
    if (howMany < cubesAsItIsUntill) {
      instantiateCount = howMany;
    } else {
      instantiateCount = cubesAsItIsUntill + Mathf.RoundToInt((howMany - cubesAsItIsUntill) / (float) restCubesChangePer);
    }

    for (int e = 0; e < instantiateCount; e++) {
      GameObject cube = (GameObject) Instantiate(particles, mover.transform.position, mover.transform.rotation);
      if (e == 0) {
        cube.GetComponent<ParticleMover>().triggerCubesGet(howMany);
      }
      if (rainbow) {
        cube.GetComponent<Renderer>().material.SetColor("_TintColor", rdm.rainbowColors[e]);
      }
    }
    cubesCount.addCount(howMany);
    energyBar.getHealthbyParts(howMany);
    getEnergy.Play ();
    comboBar.addCombo();

    mover.encounterPlayer();
    QuestManager.qm.addCountToQuest("GetCube", howMany);
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
    QuestManager.qm.addCountToQuest("GetCube", howMany);
    StartCoroutine("stopReboundingByDispenser");
  }

  IEnumerator stopReboundingByDispenser() {
    yield return new WaitForSeconds(reboundingByDispenserDuring);
    reboundingByDispenser = false;
  }

  IEnumerator strengthen() {
    if (scoreManager.isGameOver()) yield break;

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
      unstoppableEffect_two.GetComponent<AudioSource>().Play ();
    }

    if (exitedBlackhole) {
      unstoppableSphere.SetActive(true);
      getBlackhole.Play();
      getBlackhole.GetComponent<AudioSource>().Play();
    }

    if (ridingMonster) {
      energyBar.getFullHealth();
      changeCharacter(monsterMesh, monsterMaterial);
      monsterEffect.Play();
      monsterEffect.GetComponent<AudioSource>().Play();
    }

    stBar.startStrengthen(effectDuration);

    yield return new WaitForSeconds(effectDuration);

    if (unstoppable) {
      spawnManager.runManager("SpecialParts");
      unstoppable = false;
      unstoppableEffect.Stop();
      unstoppableEffect_two.Stop();
      unstoppableEffect_two.GetComponent<AudioSource>().Stop ();
      QuestManager.qm.addCountToQuest("DestroyAsteroidsBeforeUnstoppableEnd", 0);
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
      monsterEffect.Stop();
      monsterEffect.GetComponent<AudioSource>().Stop();
      monm.monsterFilter.SetActive(false);
      monm.run();
    }
  }

  void startRiding(ObjectsMover obMover) {
    QuestManager.qm.addCountToQuest("RideMonster");

    if (energyBar.currentEnergy() <= 30) {
      QuestManager.qm.addCountToQuest("RideMonsterWithLowEnergy");
    }

    if (exitedBlackhole) {
      QuestManager.qm.addCountToQuest("RideMonsterByBlackhole");
    }

    ridingMonster = true;
    monm.monsterFilter.SetActive(true);
    ridingEffect.Play();
    ridingEffect.GetComponent<AudioSource>().Play();
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
    QuestManager.qm.addCountToQuest("ExitBlackhole");

    isInsideBlackhole = false;
    exitedBlackhole = true;
    Destroy(blackhole);

    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
  }

  public void contactBlackholeWhileUnstoppable(Collision collision) {
    QuestManager.qm.addCountToQuest("ReboundByBlackhole");

    reboundingByBlackhole = true;
    isInsideBlackhole = false;
    processCollision(collision);

    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
  }

  public void contactBlackholeWhileRainbow(Collision collision) {
    QuestManager.qm.addCountToQuest("ReboundByBlackhole");

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

  public bool isRidingMonster() {
    return ridingMonster;
  }

  public bool isUsingBlackhole() {
    return exitedBlackhole || isInsideBlackhole;
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
    QuestManager.qm.addCountToQuest("NoBooster", 0);
    QuestManager.qm.addCountToQuest("UseBooster");

    if (!unstoppable) {
      energyBar.loseByShoot();
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

  public void nearAsteroid(bool enter = true) {
    if (enter) nearAsteroidCounter++;
    else nearAsteroidCounter--;
  }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Superheat : MonoBehaviour {
  public bool forceSuperheat = false;
  public PartsToBeCollected ptb;
  public StrengthenTimeBar stb;
  public PowerBoostBackground powerBoostBackground;
  public RectTransform superImage;
  public RectTransform heatImage;
  public PlayerMover player;
  public GameObject energyCube;
  public GameObject afterImagePrefab;
  public Color[] afterImageMainColors;
  public Color[] afterImageEmissiveColors;
  private ParticleSystem superheatParticle;

  public int startSuperPos = -895;
  public int slowSuperPos = 5;
  public int fastSuperPos = 45;
  public int endSuperPos = 945;

  public int startHeatPos = 945;
  public int slowHeatPos = 45;
  public int fastHeatPos = 5;
  public int endHeatPos = -895;

  private int transformStatus = 0;
  public float superheatFastDuration = 0.1f;
  public float superheatSlowDuration = 0.8f;
  private float superXPos;
  private float heatXPos;

  public float shakeDuration = 0.5f;
  public float shakeAmount = 3f;

  public float sizeChangeInterval = 0.1f;
  public int middleSize = 3;
  public int bigSize = 6;
  private bool isTransforming = false;
  public int boostDuration = 8;

  public float generatePer = 0.5f;
  public float afterImageDuration = 1;

  public float baseSpeed = 200;
  public float boosterSpeedUpAmount = 250;
  public float maxBoosterSpeed = 400;
  private bool superheatRunning = false;
  private Vector3 direction;
  public int rotatingSpeed = 1000;

  private int superHeatCount = 0;

  public float guageAlphaUp = 1;
  public float guageAlphaDown = 0.2f;
  public float guageAlphaChangeDuration = 1;
  private Color guageColor;
  private bool guageAlphaGoingUp = true;
  private float guageAlpha;
  private bool guageTurnedOn = false;

  public float iconAlphaStayDuration = 2;
  public float iconAlphaStayCount;

  public float guageScaleUp = 4;
  public Image guageIcon;
  public Image guage;
  public Image guageShell;

  public float maxGuage = 1000;
  public float guagePerCube = 0.5f;
  public float guageSpeedStandard = 200;
  private float targetGuage;
  public int showHeatWhenLargerThan = 100;

	void Start() {
    superheatParticle = transform.Find("Particle").GetComponent<ParticleSystem>();

    guageColor = guage.color;
    guageAlpha = guageAlphaDown;
  }

  public void startGame() {
    GetComponent<MeshFilter>().sharedMesh = player.GetComponent<MeshFilter>().sharedMesh;
    afterImagePrefab.GetComponent<MeshFilter>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
  }

  void Update() {
    transform.position = player.transform.position;
    transform.Rotate(direction * Time.deltaTime * rotatingSpeed);

    if (transformStatus > 0) {
      if (transformStatus == 1) {
        superXPos = Mathf.MoveTowards(superXPos, slowSuperPos, Time.deltaTime * Mathf.Abs(startSuperPos - slowSuperPos) / superheatFastDuration);
        heatXPos = Mathf.MoveTowards(heatXPos, slowHeatPos, Time.deltaTime * Mathf.Abs(startHeatPos - slowHeatPos) / superheatFastDuration);
        if (superXPos == slowSuperPos) transformStatus++;
      } else if (transformStatus == 2) {
        superXPos = Mathf.MoveTowards(superXPos, fastSuperPos, Time.deltaTime * Mathf.Abs(fastSuperPos - slowSuperPos) / superheatSlowDuration);
        heatXPos = Mathf.MoveTowards(heatXPos, fastHeatPos, Time.deltaTime * Mathf.Abs(fastHeatPos - slowHeatPos) / superheatSlowDuration);
        if (superXPos == fastSuperPos) transformStatus++;
      } else if (transformStatus == 3) {
        superXPos = Mathf.MoveTowards(superXPos, endSuperPos, Time.deltaTime * Mathf.Abs(fastSuperPos - endSuperPos) / superheatFastDuration);
        heatXPos = Mathf.MoveTowards(heatXPos, endHeatPos, Time.deltaTime * Mathf.Abs(fastHeatPos - endHeatPos) / superheatFastDuration);
        if (superXPos == endSuperPos) transformStatus = 0;
      }
      superImage.anchoredPosition = new Vector2(superXPos, 0);
      heatImage.anchoredPosition = new Vector2(heatXPos, 0);
    }

    if (superheatRunning) {
      guage.fillAmount = Mathf.MoveTowards(guage.fillAmount, 0, Time.deltaTime / boostDuration);
    } else {
      guage.fillAmount = Mathf.MoveTowards(guage.fillAmount, targetGuage, Time.deltaTime / (maxGuage / guageSpeedStandard));

      if (guageTurnedOn) {
        if (iconAlphaStayCount > 0) {
          iconAlphaStayCount -= Time.deltaTime;
        } else {
          guageTurnedOn = false;
          guageColor.a = guageAlphaDown;
          guage.color = guageColor;
          guageIcon.color = guageColor;
          guageAlphaGoingUp = true;
          guageIcon.transform.Find("Particle").gameObject.SetActive(false);
        }
      } else {
        if (guageAlphaGoingUp) {
          guageAlpha = Mathf.MoveTowards(guageAlpha, guageAlphaUp, Time.deltaTime * (guageAlphaUp - guageAlphaDown) / guageAlphaChangeDuration);
          guageColor.a = guageAlpha;
          guage.color = guageColor;
          guageIcon.color = guageColor;
          if (guageAlpha == guageAlphaUp) guageAlphaGoingUp = false;
        } else {
          guageAlpha = Mathf.MoveTowards(guageAlpha, guageAlphaDown, Time.deltaTime * (guageAlphaUp - guageAlphaDown) / guageAlphaChangeDuration);
          guageColor.a = guageAlpha;
          guage.color = guageColor;
          guageIcon.color = guageColor;
          if (guageAlpha == guageAlphaDown) guageAlphaGoingUp = true;
        }
      }
    }
  }

  public void addGuageWithEffect(float val) {
    guageTurnedOn = true;
    guageColor.a = 1;
    guage.color = guageColor;
    guageIcon.color = guageColor;
    guageIcon.transform.Find("Particle").gameObject.SetActive(true);
    iconAlphaStayCount = iconAlphaStayDuration;

    addGuage(val);
    if (val >= showHeatWhenLargerThan) {
      player.showEffect("Heat");
    }
  }

  public void addGuage(float val) {
    if (superheatRunning) return;
    targetGuage += val / maxGuage;

    if (!superheatRunning && guage.fillAmount == 1) startSuperheat();
  }

  void startSuperheat() {
    if (isTransforming) return;

    superHeatCount++;
    guage.fillAmount = 1;
    stb.stop();
    ptb.show(false);

    guageIcon.transform.localScale = guageScaleUp * Vector3.one;
    guage.transform.localScale = guageScaleUp * Vector3.one;
    guageShell.transform.localScale = guageScaleUp * Vector3.one;
    guageColor.a = guageAlphaUp;
    guageIcon.color = guageColor;
    guage.color = guageColor;
    guageIcon.transform.Find("Particle").gameObject.SetActive(true);

    DataManager.dm.increment("TotalSuperheats");

    player.startPowerBoost();
    isTransforming = true;
    transformStatus = 1;
    superXPos = superImage.anchoredPosition.x;
    heatXPos = heatImage.anchoredPosition.x;

    GetComponent<Renderer>().enabled = true;
    GetComponent<Collider>().enabled = true;

    player.GetComponent<Rigidbody>().isKinematic = true;
    powerBoostBackground.startPowerBoost();

    StartCoroutine("superHeat");
  }

  IEnumerator superHeat() {
    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one * middleSize;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one * middleSize;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one * middleSize;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one * bigSize;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one * middleSize;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one * bigSize;

    player.GetComponent<Rigidbody>().isKinematic = false;
    setDir(player.getDirection());

    isTransforming = false;
    superheatRunning = true;
    AudioManager.am.startPowerBoost();
    superheatParticle.Play();
    StartCoroutine("generateAfterImage");
  }

  void stopSuperheat() {
    ptb.show(true);
    player.stopPowerBoost();
    GetComponent<Renderer>().enabled = false;
    GetComponent<Collider>().enabled = false;
    superheatRunning = false;
    targetGuage = 0;

    superheatParticle.Stop();
    powerBoostBackground.stopPowerBoost();
    AudioManager.am.stopPowerBoost();

    guageIcon.transform.localScale = Vector3.one;
    guage.transform.localScale = Vector3.one;
    guageShell.transform.localScale = Vector3.one;
    guageIcon.transform.Find("Particle").gameObject.SetActive(false);
    guageColor.a = guageAlphaDown;
    guage.color = guageColor;
    guageIcon.color = guageColor;
    guageAlphaGoingUp = true;

    transform.localScale = Vector3.one;
    superImage.anchoredPosition = new Vector2(startSuperPos, 0);
    heatImage.anchoredPosition = new Vector2(startHeatPos, 0);
  }

  IEnumerator generateAfterImage() {
    int index = 0;

    while (guage.fillAmount > 0) {
      PowerBoostAfterImageMover afterImage = ((GameObject)Instantiate(afterImagePrefab, transform.position, transform.rotation)).GetComponent<PowerBoostAfterImageMover>();

      if (index >= afterImageMainColors.Length) index = 0;

      afterImage.run(afterImageDuration, afterImageMainColors[index], afterImageEmissiveColors[index], transform.localScale.x);
      index++;

      yield return new WaitForSeconds(generatePer);
    }

    stopSuperheat();
  }

  public void setDir(Vector3 dir) {
    direction = dir;
  }

  void OnTriggerEnter(Collider other) {
    ObjectsMover mover = other.GetComponent<ObjectsMover>();
    if (mover == null) return;

    for (int e = 0; e < mover.cubesWhenEncounter(); e++) {
      GameObject cube = (GameObject) Instantiate(energyCube, other.transform.position, other.transform.rotation);
      if (e == 0) {
        cube.GetComponent<ParticleMover>().triggerCubesGet(mover.cubesWhenEncounter());
        player.addCubeCount(mover.cubesWhenEncounter());
      }
    }

    mover.destroyObject(true, true);
  }

  public bool isOnSuperheat() {
    return superheatRunning || isTransforming;
  }

  void OnDisable() {
    DataManager.dm.setBestInt("BestNumSuperheats", superHeatCount);
  }
}

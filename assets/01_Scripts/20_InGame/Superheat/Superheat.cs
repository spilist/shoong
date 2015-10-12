using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Superheat : MonoBehaviour {
  public bool forceSuperheat = false;
  public bool forceSuperheatWhenCollect = false;
  public StrengthenTimeBar stb;
  public PhaseManager phaseManager;
  public PowerBoostBackground powerBoostBackground;
  public RectTransform superImage;
  public RectTransform heatImage;

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

  public GameObject energyCube;
  public GameObject afterImagePrefab;
  public Color[] afterImageMainColors;
  public Color[] afterImageEmissiveColors;
  private ParticleSystem powerBoostParticle;

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
  private bool powerBoostRunning = false;
  private Vector3 direction;
  public int rotatingSpeed = 1000;

  private int superHeatCount = 0;

  public Transform toBeCollected;
  public int partsRotatingSpeed = 5;

  public PlayerMover player;
  public Transform UInormalParts;
  public Material inactiveMat;
  public Material activeMat;
  public Color inactiveGuageColor;
  public Color activeGuageColor;
  public int[] numGuages;
  public int numPartsToCollect;

  public float guageScaleUp = 4;
  public Image guageIcon;
  public Image guageShell;
  public Image guage;
  public Image guageBlinkingEnds;
  public Image guageBlinkingMiddle;
  private Image guageBlinking;
  public Sprite[] shells;
  public Sprite[] glows;
  public Sprite[] middles;
  public float guageAlphaChangeDuration = 0.5f;
  private Color guageColor;
  private bool guageAlphaGoingUp = true;
  private float guageAlpha;

  public int speedIncreasePerCollect = 1;
  public float pitchIncreasePerCollect = 0.1f;
  public AudioSource collectSound;
  public AudioSource collectCompleteSound;

  private Mesh[] partsMeshes;
  private int collectCount = 0;
  private int currentGuageNum = 0;
  private int phaseLevel = 0;
  private int phaseBonus = 0;

	void Start() {
    powerBoostParticle = transform.Find("Particle").GetComponent<ParticleSystem>();

    partsMeshes = new Mesh[UInormalParts.childCount];
    int count = 0;
    foreach (Transform tr in UInormalParts) {
      partsMeshes[count++] = tr.GetComponent<MeshFilter>().sharedMesh;
    }

    guageColor = inactiveGuageColor;
    guageAlpha = guageColor.a;
    guageBlinking = guageBlinkingEnds;
  }

  public void startGame() {
    generateNewCollects();
  }

  void Update() {
    transform.position = player.transform.position;
    transform.Rotate(direction * Time.deltaTime * rotatingSpeed);

    if (toBeCollected.gameObject.activeSelf) {
      toBeCollected.Rotate(0, 0, Time.deltaTime * partsRotatingSpeed);
    }

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

    if (powerBoostRunning) {
      guage.fillAmount = Mathf.MoveTowards(guage.fillAmount, 0, Time.deltaTime / boostDuration);
    } else {
      if (guageAlphaGoingUp) {
        guageAlpha = Mathf.MoveTowards(guageAlpha, activeGuageColor.a, Time.deltaTime * (activeGuageColor.a - inactiveGuageColor.a) / guageAlphaChangeDuration);
        guageColor.a = guageAlpha;
        guageBlinking.color = guageColor;
        if (guageAlpha == activeGuageColor.a) guageAlphaGoingUp = false;
      } else {
        guageAlpha = Mathf.MoveTowards(guageAlpha, inactiveGuageColor.a, Time.deltaTime * (activeGuageColor.a - inactiveGuageColor.a) / guageAlphaChangeDuration);
        guageColor.a = guageAlpha;
        guageBlinking.color = guageColor;
        if (guageAlpha == inactiveGuageColor.a) guageAlphaGoingUp = true;
      }
    }
  }

  public void generateNewCollects() {
    toBeCollected.gameObject.SetActive(true);

    int count = 0;
    foreach (Transform tr in toBeCollected.Find("Parts")) {
      if (count < numPartsToCollect) {
        tr.GetComponent<MeshFilter>().sharedMesh = getRandomMesh();
        tr.GetComponent<Renderer>().sharedMaterial = inactiveMat;
        tr.GetComponent<ParticleSystem>().Stop();
      } else {
        tr.gameObject.SetActive(false);
      }
      count++;
    }

    collectCount = 0;
  }

  Mesh getRandomMesh() {
    return partsMeshes[Random.Range(0, partsMeshes.Length)];
  }

  public void checkCollected(Mesh mesh) {
    if (forceSuperheat && !powerBoostRunning) {
      startSuperheat();
      return;
    }

    foreach (Transform tr in toBeCollected.Find("Parts")) {
      if (!tr.gameObject.activeSelf) continue;

      if (tr.GetComponent<Renderer>().sharedMaterial == inactiveMat && tr.GetComponent<MeshFilter>().sharedMesh == mesh) {
        tr.GetComponent<Renderer>().sharedMaterial = activeMat;
        tr.GetComponent<ParticleSystem>().Play();
        addCollect();
        return;
      }
    }
  }

  void addCollect() {
    collectCount++;
    if (collectCount < numPartsToCollect) {
      collectSound.Play();
      collectSound.pitch += pitchIncreasePerCollect;
    } else {
      collectCompleteSound.Play();
      completeCollect();
    }
  }

  void completeCollect() {
    currentGuageNum++;

    if (numGuages[phaseLevel] == currentGuageNum || forceSuperheatWhenCollect) {
      startSuperheat();
    } else {
      if (currentGuageNum + 1 == numGuages[phaseLevel]) {
        guageBlinkingEnds.fillClockwise = false;
        guageBlinking = guageBlinkingEnds;
        guageBlinkingEnds.gameObject.SetActive(true);
        guageBlinkingMiddle.gameObject.SetActive(false);
      } else {
        if (guageBlinkingMiddle.gameObject.activeSelf) {
          guageBlinkingMiddle.transform.localEulerAngles -= new Vector3(0, 0, 360.0f / numGuages[phaseLevel]);
        } else {
          guageBlinking = guageBlinkingMiddle;
          guageBlinkingEnds.gameObject.SetActive(false);
          guageBlinkingMiddle.gameObject.SetActive(true);
        }
      }

      guage.fillAmount += 1.0f / numGuages[phaseLevel];
      player.baseSpeed += speedIncreasePerCollect;
      generateNewCollects();
    }

    collectSound.pitch -= pitchIncreasePerCollect * 5;
  }

  void startSuperheat() {
    if (isTransforming) return;

    superHeatCount++;
    guage.fillAmount = 1;
    stb.stop();

    ElapsedTime.time.stopCountLimit(false);

    guageIcon.transform.localScale = guageScaleUp * Vector3.one;
    guage.transform.localScale = guageScaleUp * Vector3.one;
    guageShell.transform.localScale = guageScaleUp * Vector3.one;
    guageIcon.transform.Find("Particle").gameObject.SetActive(true);

    DataManager.dm.increment("TotalSuperheats");

    guageBlinkingEnds.gameObject.SetActive(false);
    toBeCollected.gameObject.SetActive(false);
    GetComponent<MeshFilter>().sharedMesh = player.GetComponent<MeshFilter>().sharedMesh;
    afterImagePrefab.GetComponent<MeshFilter>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;

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
    powerBoostRunning = true;
    AudioManager.am.startPowerBoost();
    powerBoostParticle.Play();
    StartCoroutine("generateAfterImage");
  }

  void stopSuperheat() {
    player.stopPowerBoost();
    GetComponent<Renderer>().enabled = false;
    GetComponent<Collider>().enabled = false;
    powerBoostRunning = false;

    powerBoostParticle.Stop();
    powerBoostBackground.stopPowerBoost();
    AudioManager.am.stopPowerBoost();

    toBeCollected.gameObject.SetActive(true);
    guageIcon.transform.localScale = Vector3.one;
    guage.transform.localScale = Vector3.one;
    guageShell.transform.localScale = Vector3.one;
    guageIcon.transform.Find("Particle").gameObject.SetActive(false);

    transform.localScale = Vector3.one;
    superImage.anchoredPosition = new Vector2(startSuperPos, 0);
    heatImage.anchoredPosition = new Vector2(startHeatPos, 0);

    currentGuageNum = 0;

    if (phaseLevel < numGuages.Length - 1) {
      phaseLevel++;
    }
    phaseManager.nextPhase();
    guageShell.sprite = shells[phaseLevel];
    guage.sprite = glows[phaseLevel];
    guageBlinkingEnds.sprite = glows[phaseLevel];
    guageBlinkingEnds.fillAmount = 1.0f / numGuages[phaseLevel];
    guageBlinkingEnds.fillClockwise = true;
    guageBlinkingEnds.gameObject.SetActive(true);
    guageBlinkingMiddle.sprite = middles[phaseLevel];
    guageBlinkingMiddle.transform.localEulerAngles = Vector3.zero;

    generateNewCollects();
    phaseBonus += phaseLevel;

    ElapsedTime.time.resetCount();
  }

  IEnumerator generateAfterImage() {
    int index = 0;
    float count = 0;

    while (count < boostDuration) {
      PowerBoostAfterImageMover afterImage = ((GameObject)Instantiate(afterImagePrefab, transform.position, transform.rotation)).GetComponent<PowerBoostAfterImageMover>();

      if (index >= afterImageMainColors.Length) index = 0;

      afterImage.run(afterImageDuration, afterImageMainColors[index], afterImageEmissiveColors[index], transform.localScale.x);
      index++;

      yield return new WaitForSeconds(generatePer);
      count += generatePer;
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

  public bool isOnPowerBoost() {
    return powerBoostRunning;
  }

  void OnDisable() {
    DataManager.dm.setBestInt("BestNumSuperheats", superHeatCount);
  }

  public int getPhaseBonus() {
    return phaseBonus;
  }
}

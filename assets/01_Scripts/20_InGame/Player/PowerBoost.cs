using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class PowerBoost : MonoBehaviour {
  public PhaseManager phaseManager;
  public PowerBoostBackground powerBoostBackground;
  public RectTransform superImage;
  public RectTransform heatImage;

  public int startSuperPos;
  public int slowSuperPos;
  public int fastSuperPos;
  public int endSuperPos;

  public int startHeatPos;
  public int slowHeatPos;
  public int fastHeatPos;
  public int endHeatPos;

  private int transformStatus = 0;
  public float superheatFastDuration = 0.1f;
  public float superheatSlowDuration = 0.8f;
  private float superXPos;
  private float heatXPos;

  public float shakeDuration = 0.5f;
  public float shakeAmount = 3f;

  public Image powerBoostGuage;
  public Image powerBoostText;
  private ParticleSystem powerBoostParticle;

  public GameObject energyCube;
  public PlayerMover player;
  public ComboBar comboBar;

  public GameObject afterImagePrefab;
  public Color[] afterImageMainColors;
  public Color[] afterImageEmissiveColors;

  public float sizeChangeInterval = 0.1f;
  public int middleSize = 4;
  public int bigSize = 7;
  private bool isTransforming = false;

  public float maxGuage = 1000;
  public float guagePerCube = 0.5f;
  public int boostDuration = 8;

  public float guageAlphaUpDuration = 0.3f;
  public float guageAlphaDownDuration = 0.6f;
  public float guageTargetAlphaUp = 0.67f;
  public float guageTargetAlphaDown = 0.5f;
  private Color guageColor;
  private bool guageAlphaGoingUp = true;
  private float guageAlpha = 0.5f;

  public float alphaStayAtUpOnPowerBoost = 0.2f;
  public float alphaChangeOnPowerBoost = 0.1f;
  public float alphaStayAtDownOnPowerBoost = 0.1f;
  public float alphaDownOnPowerBoost = 0.3f;

  public float boostTextAlphaChangeDuration = 0.1f;
  public float boostTextAlphaStayDuration = 0.3f;
  private Color textColor;
  private float textAlpha;
  private float alphaStayCount;
  private int alphaChangeStatus = 0;

  public float generatePer = 0.5f;
  public float appearAfter = 0.05f;
  public float afterImageDuration = 1;

  public float baseSpeed = 200;
  public float boosterSpeedUpAmount = 150;
  public float maxBoosterSpeed = 400;

  private bool powerBoostRunning = false;
  private bool changeTextAlpha = false;

  private Vector3 direction;
  public int rotatingSpeed = 100;

  private int superHeatCount = 0;

	void Start () {
    powerBoostParticle = transform.Find("Particle").GetComponent<ParticleSystem>();
    guageColor = powerBoostGuage.material.GetColor("_TintColor");
    guageAlpha = guageColor.a;
  }

  public void addGuageWithEffect(float val) {
    if (!changeTextAlpha) {
      textAlpha = 0;
      alphaChangeStatus = 1;
      alphaStayCount = 0;
      textColor = powerBoostText.color;
      changeTextAlpha = true;
    }
    addGuage(val);
  }

  public void addGuage(float val) {
    if (powerBoostRunning) return;

    powerBoostGuage.fillAmount += val / maxGuage;
    if (powerBoostGuage.fillAmount == 1) startPowerBoost();
  }

  void startPowerBoost() {
    if (isTransforming) return;

    superHeatCount++;

    QuestManager.qm.addCountToQuest("DoSuperHeat");
    DataManager.dm.increment("TotalSuperheats");

    GetComponent<MeshFilter>().sharedMesh = player.GetComponent<MeshFilter>().sharedMesh;
    afterImagePrefab.GetComponent<MeshFilter>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;

    player.startPowerBoost();
    isTransforming = true;
    transformStatus = 1;
    superXPos = superImage.anchoredPosition.x;
    heatXPos = heatImage.anchoredPosition.x;

    comboBar.GetComponent<Canvas>().enabled = false;
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
    // player.rotatePlayerBody();

    isTransforming = false;
    powerBoostRunning = true;
    textColor = powerBoostText.color;
    textColor.a = 1;
    powerBoostText.color = textColor;
    textAlpha = 1;
    alphaChangeStatus = 1;
    alphaStayCount = 0;
    AudioManager.am.startPowerBoost();
    powerBoostParticle.Play();
    StartCoroutine("generateAfterImage");
  }

  void stopPowerBoost() {
    player.stopPowerBoost();
    comboBar.GetComponent<Canvas>().enabled = true;
    GetComponent<Renderer>().enabled = false;
    GetComponent<Collider>().enabled = false;
    powerBoostRunning = false;

    textColor = powerBoostText.color;
    textColor.a = 0;
    powerBoostText.color = textColor;
    powerBoostParticle.Stop();
    powerBoostBackground.stopPowerBoost();
    AudioManager.am.stopPowerBoost();

    transform.localScale = Vector3.one;
    superImage.anchoredPosition = new Vector2(startSuperPos, 0);
    heatImage.anchoredPosition = new Vector2(startHeatPos, 0);

    phaseManager.nextPhase();
  }

  IEnumerator generateAfterImage() {
    int index = 0;
    float count = 0;

    while (count < boostDuration) {
      PowerBoostAfterImageMover afterImage = ((GameObject)Instantiate(afterImagePrefab, transform.position, transform.rotation)).GetComponent<PowerBoostAfterImageMover>();

      if (index >= afterImageMainColors.Length) index = 0;

      afterImage.run(appearAfter, afterImageDuration, afterImageMainColors[index], afterImageEmissiveColors[index], transform.localScale.x);
      index++;

      yield return new WaitForSeconds(generatePer);
      count += generatePer;
    }

    stopPowerBoost();
  }

  public void setDir(Vector3 dir) {
    direction = dir;
  }

  void Update() {
    transform.position = player.transform.position;
    // transform.rotation = player.transform.rotation;
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

    if (guageAlphaGoingUp) {
      guageAlpha = Mathf.MoveTowards(guageAlpha, guageTargetAlphaUp, Time.deltaTime * (guageTargetAlphaUp - guageTargetAlphaDown) / guageAlphaUpDuration);
      guageColor.a = guageAlpha;
      powerBoostGuage.material.SetColor("_TintColor", guageColor);
      if (guageAlpha == guageTargetAlphaUp) guageAlphaGoingUp = false;
    } else {
      guageAlpha = Mathf.MoveTowards(guageAlpha, guageTargetAlphaDown, Time.deltaTime * (guageTargetAlphaUp - guageTargetAlphaDown) / guageAlphaDownDuration);
      guageColor.a = guageAlpha;
      powerBoostGuage.material.SetColor("_TintColor", guageColor);
      if (guageAlpha == guageTargetAlphaDown) guageAlphaGoingUp = true;
    }

    if (changeTextAlpha) {
      if (alphaChangeStatus == 1) {
        textAlpha = Mathf.MoveTowards(textAlpha, 1, Time.deltaTime / boostTextAlphaChangeDuration);
        textColor.a = textAlpha;
        powerBoostText.color = textColor;
        if (textAlpha == 1) alphaChangeStatus++;
      } else if (alphaChangeStatus == 2) {
        if (alphaStayCount < boostTextAlphaStayDuration) alphaStayCount += Time.deltaTime;
        else alphaChangeStatus++;
      } else if (alphaChangeStatus == 3) {
        textAlpha = Mathf.MoveTowards(textAlpha, 0, Time.deltaTime / boostTextAlphaChangeDuration);
        textColor.a = textAlpha;
        powerBoostText.color = textColor;
        if (textAlpha == 0) changeTextAlpha = false;
      }
    }

    if (powerBoostRunning) {
      powerBoostGuage.fillAmount = Mathf.MoveTowards(powerBoostGuage.fillAmount, 0, Time.deltaTime / boostDuration);

      if (alphaChangeStatus == 1) {
        if (alphaStayCount < alphaStayAtUpOnPowerBoost) alphaStayCount += Time.deltaTime;
        else {
          alphaStayCount = 0;
          alphaChangeStatus++;
        }
      } else if (alphaChangeStatus == 2) {
        textAlpha = Mathf.MoveTowards(textAlpha, alphaDownOnPowerBoost, Time.deltaTime * (1 - alphaDownOnPowerBoost) / alphaChangeOnPowerBoost);
        textColor.a = textAlpha;
        powerBoostText.color = textColor;
        if (textAlpha == alphaDownOnPowerBoost) alphaChangeStatus++;
      } else if (alphaChangeStatus == 3) {
        if (alphaStayCount < alphaStayAtDownOnPowerBoost) alphaStayCount += Time.deltaTime;
        else {
          alphaStayCount = 0;
          alphaChangeStatus++;
        }
      } else if (alphaChangeStatus == 4) {
        textAlpha = Mathf.MoveTowards(textAlpha, 1, Time.deltaTime * (1 - alphaDownOnPowerBoost) / alphaChangeOnPowerBoost);
        textColor.a = textAlpha;
        powerBoostText.color = textColor;
        if (textAlpha == 1) alphaChangeStatus = 1;
      }
    }
  }

  void OnTriggerEnter(Collider other) {
    ObjectsMover mover = other.GetComponent<ObjectsMover>();
    for (int e = 0; e < mover.cubesWhenEncounter(); e++) {
      GameObject cube = (GameObject) Instantiate(energyCube, other.transform.position, other.transform.rotation);
      if (e == 0) {
        cube.GetComponent<ParticleMover>().triggerCubesGet(mover.cubesWhenEncounter());
        player.addCubeCount(mover.cubesWhenEncounter());
      }
    }

    comboBar.addCombo();

    if (mover.tag == "Obstacle_big" || mover.tag == "Obstacle_small") {
      QuestManager.qm.addCountToQuest("DestroyAsteroid");
    } else if (mover.tag == "Monster") {
      QuestManager.qm.addCountToQuest("DestroyMonster");
    } else if (mover.tag == "Obstacle") {
      QuestManager.qm.addCountToQuest("DestroyFallingStar");
    }

    mover.destroyObject(true, true);
  }

  public bool isOnPowerBoost() {
    return powerBoostRunning;
  }

  void OnDisable() {
    DataManager.dm.setBestInt("BestNumSuperheats", superHeatCount);
    guageColor.a = guageTargetAlphaUp;
    powerBoostGuage.material.SetColor("_TintColor", guageColor);
  }
}

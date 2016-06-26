using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RainbowDonutsManager : ObjectsManager {
  public int chanceBase = 200;
  public Material goldenRainbowMat;
  public int goldenChance = 1;
  public bool isGolden = false;

  public LayerMask blackholeGravityMask;

  public GameObject rainbowRoadPrefab;
  public List<GameObject> roadPool;
  public int cookiesPerRoad = 6;

  public int[] numRoadRidesPerLevel;
  public int[] speedPerRide;
  public int numRoadRides;
  public int nextDonutRadius = 100;
  public float rotateDuring = 0.2f;
  public int rotateAngularSpeed = 50;
  public int ridingSpeed = 200;
  public Color[] rainbowColors;
  public float pitchStart = 0.9f;
  public float pitchIncrease = 0.1f;

  private int currentRoadRide;
  private int rideCount = 0;
  private bool drawingRainbowRoad = false;
  private bool erasingRainbowRoad = false;
  private LineRenderer rainbowRoad;
  private Vector3 origin;
  private Vector3 destination;
  private float drawingDistance;

  private NormalPartsManager npm;
  private GoldenCubeManager gcm;
  private Mesh[] cookieMeshes;
  private float cookieDistance;

  override public void initRest() {
    skipInterval = true;

    roadPool = new List<GameObject>();
    for (int i = 0; i < objAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(rainbowRoadPrefab);
      obj.SetActive(false);
      roadPool.Add(obj);
    }

    npm = GetComponent<NormalPartsManager>();
    gcm = GetComponent<GoldenCubeManager>();
    cookieMeshes = new Mesh[npm.meshes.childCount];
    int count = 0;
    foreach (Transform tr in npm.meshes) {
      cookieMeshes[count++] = tr.GetComponent<MeshFilter>().sharedMesh;
    }

    adjustForLevel(1);
    run();
  }

  override public void adjustForLevel(int level) {
    numRoadRides = numRoadRidesPerLevel[level - 1];
  }

  override public bool checkAlreadyRunning() {
    if (player.isUsingRainbow()) {
      StopCoroutine("respawnRoutine");
      StartCoroutine("respawnRoutine");
      return true;
    }

    return false;
  }

  override protected void beforeSpawn() {
    StopCoroutine("rideRainbow");
    erasingRainbowRoad = false;
    drawingRainbowRoad = false;
    if (rainbowRoad != null) rainbowRoad.gameObject.SetActive(false);
    if (instance != null) instance.SetActive(false);
    objEncounterEffectForPlayer.Stop();
  }

  override protected void afterSpawn() {
    rideCount = 0;

    int random = Random.Range(0, chanceBase);
    if (DataManager.dm.isBonusStage || random < goldenChance) {
      isGolden = true;
      instance.GetComponent<Renderer>().sharedMaterial = goldenRainbowMat;
      instance.transform.Find("GoldenEffect").gameObject.SetActive(true);
    } else {
      isGolden = false;
      instance.GetComponent<Renderer>().sharedMaterial = objPrefab.GetComponent<Renderer>().sharedMaterial;
      instance.transform.Find("GoldenEffect").gameObject.SetActive(false);
    }
  }

  public void startRidingRainbow() {
    //SkillManager.sm.stopSkills();
    SkillManager.sm.stopSkills("Metal");
    SkillManager.sm.stopSkills("Blink");
    erasingRainbowRoad = false;
    if (rainbowRoad != null && rainbowRoad.gameObject.activeSelf) rainbowRoad.gameObject.SetActive(false);

    if (rideCount == 0) {
      currentRoadRide = numRoadRides;
    }

    player.setRidingRainbowRoad(false);

    if (rideCount < currentRoadRide) {

      ridingSpeed = speedPerRide[rideCount];

      if (SkillManager.sm.skillRunning() && SkillManager.sm.isFever())
        ridingSpeed = (int) (ridingSpeed * 1.5f);

      objEncounterEffectForPlayer.Play();
      objEncounterEffectForPlayer.GetComponent<AudioSource>().pitch = pitchStart + rideCount * pitchIncrease;
      objEncounterEffectForPlayer.GetComponent<AudioSource>().Play();

      rideCount++;

      cookieDistance = 0;

      //AudioManager.am.main.movePitch(pitchIncrease, nextDonutRadius / ridingSpeed);
      StartCoroutine("rideRainbow");
    } else {
      //AudioManager.am.main.movePitch(-pitchIncrease * rideCount, 0.5f);
      objEncounterEffectForPlayer.Stop();
      player.afterStrengthenStart();
      // player.rotatePlayerBody();

      spawnedByTransform = false;
      run();
    }
  }

  IEnumerator rideRainbow() {
    player.setRotateByRainbow(true);

    rainbowRoad = getPooledObj(roadPool, rainbowRoadPrefab).GetComponent<LineRenderer>();
    rainbowRoad.gameObject.SetActive(true);
    origin = instance.transform.position;
    rainbowRoad.SetPosition(0, origin);
    drawingDistance = 0;

    Vector3 dir;
    do {
      dir = getRandomDirection();
      destination = origin + dir * nextDonutRadius;
    } while(Physics.OverlapSphere(destination, 50, blackholeGravityMask).Length > 0);

    drawingRainbowRoad = true;

    yield return new WaitForSeconds(rotateDuring);

    player.setRotateByRainbow(false);
    player.setDirection(dir);
    player.setRidingRainbowRoad(true);
    erasingRainbowRoad = true;
  }

  Vector3 getRandomDirection() {
    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    return new Vector3(randomV.x, 0, randomV.y);
  }

  void Update() {
    if (drawingRainbowRoad) {

      drawingDistance = Mathf.MoveTowards(drawingDistance, nextDonutRadius, Time.deltaTime * ridingSpeed);
      Vector3 nextPos = drawingDistance * Vector3.Normalize(destination - origin) + origin;
      rainbowRoad.SetPosition(1, nextPos);

      cookieDistance = Mathf.MoveTowards(cookieDistance, (float)nextDonutRadius / (cookiesPerRoad + 1), Time.deltaTime * ridingSpeed);
      if (cookieDistance >= (float)nextDonutRadius / (cookiesPerRoad + 1)) {
        cookieDistance = 0;
        if (isGolden) {
          gcm.spawnGoldenCube(nextPos);
        } else {
          npm.spawnNormal(nextPos);
        }
      }

      if (drawingDistance == nextDonutRadius) {
        drawingRainbowRoad = false;
        instance = getPooledObj(objPool, objPrefab, destination);
        instance.SetActive(true);
        if (isGolden) {
          instance.GetComponent<Renderer>().sharedMaterial = goldenRainbowMat;
          instance.transform.Find("GoldenEffect").gameObject.SetActive(true);
        } else {
          instance.GetComponent<Renderer>().sharedMaterial = objPrefab.GetComponent<Renderer>().sharedMaterial;
          instance.transform.Find("GoldenEffect").gameObject.SetActive(false);
        }
      }
    }

    if (erasingRainbowRoad) {
      rainbowRoad.SetPosition(0, player.transform.position);
    }
  }

  public bool moreRide() {
    return rideCount < currentRoadRide;
  }

  public void destroyInstances() {
    StopCoroutine("rideRainbow");
    erasingRainbowRoad = false;
    drawingRainbowRoad = false;
    if (rainbowRoad != null) rainbowRoad.gameObject.SetActive(false);
    if (instance != null) instance.SetActive(false);
    objEncounterEffectForPlayer.Stop();
    run();
  }

  public int gerRideCount() {
    return rideCount;
  }
}

using UnityEngine;
using System.Collections;

public class RainbowDonutsManager : ObjectsManager {
  public LayerMask blackholeGravityMask;

  public GameObject rainbowRoadPrefab;

  public int[] numRoadRidesPerLevel;
  public int[] speedPerRide;
  private int numRoadRides;
  public int nextDonutRadius = 100;
  public float rotateDuring = 0.2f;
  public int rotateAngularSpeed = 50;
  public int ridingSpeed = 200;
  public int cubesPerRide = 7;
  public Color[] rainbowColors;
  public float pitchStart = 0.9f;
  public float pitchIncrease = 0.1f;

  private int rideCount = 0;
  private bool drawingRainbowRoad = false;
  private bool erasingRainbowRoad = false;
  private LineRenderer rainbowRoad;
  private Vector3 origin;
  private Vector3 destination;
  private float drawingDistance;

  override public void initRest() {
    numRoadRides = numRoadRidesPerLevel[DataManager.dm.getInt("RainbowDonutsLevel") - 1];
    skipInterval = true;
  }

  override protected void afterSpawn() {
    rideCount = 0;
  }

  public void startRidingRainbow() {
    erasingRainbowRoad = false;
    if (rainbowRoad != null) Destroy(rainbowRoad.gameObject);

    if (rideCount == 0) {
      QuestManager.qm.addCountToQuest("RideRainbow");
      QuestManager.qm.addCountToQuest("RainbowDonuts");
    }

    if (rideCount < numRoadRides) {
      ridingSpeed = speedPerRide[rideCount];

      objEncounterEffectForPlayer.Play();
      objEncounterEffectForPlayer.GetComponent<AudioSource>().pitch = pitchStart + rideCount * pitchIncrease;
      objEncounterEffectForPlayer.GetComponent<AudioSource>().Play();

      rideCount++;
      StartCoroutine("rideRainbow");
    } else {
      objEncounterEffectForPlayer.Stop();
      player.afterStrengthenStart();
      run();
    }
  }

  IEnumerator rideRainbow() {
    player.setRotateByRainbow(true);

    rainbowRoad = ((GameObject) Instantiate(rainbowRoadPrefab)).GetComponent<LineRenderer>();
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

    yield return new WaitForSeconds((float)nextDonutRadius / (float)ridingSpeed);

    player.setRidingRainbowRoad(false);
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

      if (drawingDistance == nextDonutRadius) {
        drawingRainbowRoad = false;
        instance = (GameObject) Instantiate(objPrefab, destination, Quaternion.identity);
        instance.transform.parent = transform;
      }
    }

    if (erasingRainbowRoad) {
      rainbowRoad.SetPosition(0, player.transform.position);
    }
  }

  public bool moreRide() {
    return rideCount < numRoadRides;
  }

  public void destroyInstances() {
    StopCoroutine("rideRainbow");
    erasingRainbowRoad = false;
    drawingRainbowRoad = false;
    if (rainbowRoad != null) Destroy(rainbowRoad.gameObject);
    if (instance != null) Destroy(instance);
    objEncounterEffectForPlayer.Stop();
    run();
  }

  public int gerRideCount() {
    return rideCount;
  }

  override public int cubesWhenEncounter() {
    return cubesPerRide;
  }
}

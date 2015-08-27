using UnityEngine;
using System.Collections;

public class RainbowDonutsManager : ObjectsManager {
  public GameObject rainbowDonutPrefab;
  public GameObject rainbowRoadPrefab;
  public float tumble = 5;
  public float minSpawnInterval = 8;
  public float maxSpawnInterval = 12;

  public int numRoadRides = 3;
  public int nextDonutRadius = 100;
  public float rotateDuring = 0.2f;
  public int rotateAngularSpeed = 50;
  public float rideDuring = 0.8f;
  public int ridingSpeed = 200;
  public int cubesPerRide = 7;

  private int rideCount = 0;
  private GameObject rainbowDonut;
  private bool drawingRainbowRoad = false;
  private bool erasingRainbowRoad = false;
  private LineRenderer rainbowRoad;
  private Vector3 origin;
  private Vector3 destination;
  private float drawingDistance;

  override public float getTumble(string objTag) {
    return tumble;
  }

  override public void run() {
    rideCount = 0;
    rainbowDonut = spawnManager.spawn(rainbowDonutPrefab);
  }

  IEnumerator respawn() {
    float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
    yield return new WaitForSeconds(interval);

    rideCount = 0;
    rainbowDonut = spawnManager.spawn(rainbowDonutPrefab);
  }

  public void startRidingRainbow() {
    erasingRainbowRoad = false;
    if (rainbowRoad != null) Destroy(rainbowRoad.gameObject);

    if (rideCount < numRoadRides) {
      rideCount++;
      StartCoroutine("rideRainbow");
    } else {
      StartCoroutine("respawn");
    }
  }

  IEnumerator rideRainbow() {
    player.setRotateByRainbow(true);

    Vector3 dir = getRandomDirection();

    rainbowRoad = ((GameObject) Instantiate(rainbowRoadPrefab)).GetComponent<LineRenderer>();
    origin = rainbowDonut.transform.position;
    rainbowRoad.SetPosition(0, origin);
    destination = rainbowDonut.transform.position + dir * nextDonutRadius;
    drawingDistance = 0;

    drawingRainbowRoad = true;

    yield return new WaitForSeconds(rotateDuring);

    player.setRotateByRainbow(false);
    player.setDirection(dir);
    player.setRidingRainbowRoad(true);
    erasingRainbowRoad = true;

    yield return new WaitForSeconds(rideDuring);

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
      rainbowRoad.SetPosition(1, drawingDistance * Vector3.Normalize(destination - origin) + origin);

      if (drawingDistance == nextDonutRadius) {
        drawingRainbowRoad = false;
        rainbowDonut = (GameObject) Instantiate(rainbowDonutPrefab, destination, Quaternion.identity);
        rainbowDonut.transform.parent = gameObject.transform;
      }
    }

    if (erasingRainbowRoad) {
      rainbowRoad.SetPosition(0, player.transform.position);
    }
  }

  public bool moreRide() {
    return rideCount < numRoadRides;
  }
}

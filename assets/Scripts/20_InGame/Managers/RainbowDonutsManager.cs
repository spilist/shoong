using UnityEngine;
using System.Collections;

public class RainbowDonutsManager : ObjectsManager {
  public LayerMask blackholeGravityMask;
  public GameObject rainbowDonutPrefab;
  public GameObject rainbowRoadPrefab;
  public float tumble = 5;
  public float minSpawnInterval = 8;
  public float maxSpawnInterval = 12;

  public int numRoadRides = 3;
  public int nextDonutRadius = 100;
  public float rotateDuring = 0.2f;
  public int rotateAngularSpeed = 50;
  public int ridingSpeed = 200;
  public int cubesPerRide = 7;
  public Color[] rainbowColors;

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
      player.rainbowEffect.Play();
      player.rainbowEffect.GetComponent<AudioSource>().Play();
      StartCoroutine("rideRainbow");
    } else {
      player.rainbowEffect.Stop();
      StartCoroutine("respawn");
    }
  }

  IEnumerator rideRainbow() {
    player.setRotateByRainbow(true);

    rainbowRoad = ((GameObject) Instantiate(rainbowRoadPrefab)).GetComponent<LineRenderer>();
    origin = rainbowDonut.transform.position;
    rainbowRoad.SetPosition(0, origin);
    drawingDistance = 0;

    Vector3 dir;
    do {
      dir = getRandomDirection();
      destination = rainbowDonut.transform.position + dir * nextDonutRadius;
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

  public void destroyInstances() {
    StopCoroutine("rideRainbow");
    erasingRainbowRoad = false;
    drawingRainbowRoad = false;
    Destroy(rainbowRoad.gameObject);
    Destroy(rainbowDonut);
    StartCoroutine("respawn");
  }
}

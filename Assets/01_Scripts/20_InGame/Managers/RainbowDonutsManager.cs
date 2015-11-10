﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RainbowDonutsManager : ObjectsManager {
  public int chanceBase = 200;
  public Material goldenRainbowMat;
  public int goldenChance = 1;
  public int goldCubesGet = 1;
  public bool isGolden = false;

  public LayerMask blackholeGravityMask;

  public GameObject rainbowRoadPrefab;
  public List<GameObject> roadPool;

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

  override public void initRest() {
    skipInterval = true;

    roadPool = new List<GameObject>();
    for (int i = 0; i < objAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(rainbowRoadPrefab);
      obj.SetActive(false);
      roadPool.Add(obj);
    }

    adjustForLevel(1);
    run();
  }

  override public void adjustForLevel(int level) {
    numRoadRides = numRoadRidesPerLevel[level - 1];
  }

  override protected void afterSpawn() {
    rideCount = 0;

    int random = Random.Range(0, chanceBase);
    if (random < goldenChance) {
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
    erasingRainbowRoad = false;
    if (rainbowRoad != null && rainbowRoad.gameObject.activeSelf) rainbowRoad.gameObject.SetActive(false);

    if (rideCount == 0) {
      player.encounterObject("RainbowDonut");
      currentRoadRide = numRoadRides;
    }

    player.setRidingRainbowRoad(false);

    if (rideCount < currentRoadRide) {
      ridingSpeed = speedPerRide[rideCount];

      objEncounterEffectForPlayer.Play();
      objEncounterEffectForPlayer.GetComponent<AudioSource>().pitch = pitchStart + rideCount * pitchIncrease;
      objEncounterEffectForPlayer.GetComponent<AudioSource>().Play();

      rideCount++;

      StartCoroutine("rideRainbow");
    } else {
      objEncounterEffectForPlayer.Stop();
      player.afterStrengthenStart();
      player.rotatePlayerBody();
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

    if (isGolden) {
      GoldManager.gm.add(origin, goldCubesGet, false);
    }

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
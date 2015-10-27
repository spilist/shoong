using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnManager : MonoBehaviour {
  public int minSpawnRadius = 200;
  public int maxSpawnRadius = 400;

  public void Start() {
    GetComponent<NormalPartsManager>().enabled = true;
  }

  public void run() {
    GetComponent<FollowTarget>().enabled = false;

    // start spawn obstacles
    GetComponent<AsteroidManager>().enabled = true;
    GetComponent<SmallAsteroidManager>().enabled = true;

    // spawn GoldenCube
    GetComponent<GoldenCubeManager>().enabled = true;

    // spawn SuperheatPart
    GetComponent<SuperheatPartManager>().enabled = true;

    string subObjectsString = PlayerPrefs.GetString("SubObjects").Trim();
    if (subObjectsString != "") {
      string[] subObjects = subObjectsString.Split(' ');
      foreach (string subObject in subObjects) {
        runManager(subObject);
      }
    }
  }

  public void runManager(string objName) {
    (GetComponent(objName + "Manager") as MonoBehaviour).enabled = true;
    ObjectsManager obm = (ObjectsManager)GetComponent(objName + "Manager");
    obm.run();
  }

  public void runManagerAt(string objName, Vector3 pos, int level) {
    (GetComponent(objName + "Manager") as MonoBehaviour).enabled = true;
    ObjectsManager obm = (ObjectsManager)GetComponent(objName + "Manager");
    obm.runByTransform(pos, level);
  }

  public Vector3 getSpawnPosition(GameObject target) {
    float screenX, screenY;
    Vector3 spawnPosition;
    int count = 0;

    LayerMask mask = (int) Mathf.Pow(2, target.gameObject.layer);
    float radius = target.GetComponent<ObjectsMover>().getBoundingSize();

    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();

    do {
      screenX = Random.Range(minSpawnRadius, maxSpawnRadius);
      screenY = Random.Range(minSpawnRadius, maxSpawnRadius);
      spawnPosition = new Vector3(screenPos.x * screenX + Player.pl.transform.position.x, Player.pl.transform.position.y, screenPos.y * screenY + Player.pl.transform.position.z);
    } while(Physics.OverlapSphere(spawnPosition, radius, mask).Length > 0 && count++ < 100);

    if (count >= 100) Debug.Log(target.name + " is overlapped");

    return spawnPosition;
  }
}

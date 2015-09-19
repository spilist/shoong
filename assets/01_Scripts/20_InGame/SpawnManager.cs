using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnManager : MonoBehaviour {
  public int showHiddensInHowManyGames = 100;
  public float generateSpaceRadius = 0.9f;
  public float generateOffset = 0.2f;
  public int overlapDistance = 50;
  public LayerMask mask;

  public void Start() {
    GetComponent<NormalPartsManager>().enabled = true;
  }

  public void run() {
    GetComponent<FollowTarget>().enabled = false;

    // start spawn obstacles
    GetComponent<AsteroidManager>().enabled = true;
    GetComponent<MeteroidManager>().enabled = true;

    // spawn GoldenCube
    GetComponent<GoldenCubeManager>().enabled = true;

    runManager("EMP");
    runManager("Jetpack");

    // spawn selected objects
    string mainObjectsString = PlayerPrefs.GetString("MainObjects").Trim();
    if (mainObjectsString != "") {
      string[] mainObjects = mainObjectsString.Split(' ');
      foreach (string mainObject in mainObjects) {
        runManager(mainObject);
      }
    }

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

  public GameObject spawn(GameObject target) {
    GameObject newInstance = (GameObject) Instantiate (target, getSpawnPosition(target), Quaternion.identity);
    newInstance.transform.parent = gameObject.transform;
    return newInstance;
  }

  public void spawnRandom(GameObject[] targets, int numSpawn) {
    for (int i = 0; i < numSpawn; i++) {
      spawn(targets[Random.Range(0, targets.Length)]);
    }
  }

  public Vector3 getSpawnPosition(GameObject target) {
    float screenX, screenY;
    Vector3 spawnPosition;
    int count = 0;

    do {
      do {
        screenX = Random.Range(-generateSpaceRadius, 1 + generateSpaceRadius);
        screenY = Random.Range(-generateSpaceRadius, 1 + generateSpaceRadius);
      } while(-generateOffset < screenX && screenX < generateOffset + 1 && -generateOffset < screenY && screenY < generateOffset + 1);

      spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(screenX, screenY, Camera.main.transform.position.y));
    } while(Physics.OverlapSphere(spawnPosition, overlapDistance, mask).Length > 0 && count++ < 100);

    if (count >= 100) Debug.Log("An object is overlapped");

    return spawnPosition;
  }
}

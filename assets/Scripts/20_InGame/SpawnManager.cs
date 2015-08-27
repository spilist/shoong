using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnManager : MonoBehaviour {
  public float generateSpaceRadius = 0.9f;
  public float generateOffset = 0.2f;
  public int overlapDistance = 50;
  public string[] layersShouldNotOverlap;
  public LayerMask mask;

  public void run() {
    gameObject.SetActive(true);

    // spawn which doesn't need settings
    gameObject.GetComponent<BasicObjectsManager>().run();
    gameObject.GetComponent<RainbowDonutsManager>().run();

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
    ObjectsManager obm = (ObjectsManager)gameObject.GetComponent(objName + "Manager");
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

    bool overlap = true;
    if (layersShouldNotOverlap.Contains(LayerMask.LayerToName(target.layer))) {
      overlap = false;
    }

    do {
      do {
        screenX = Random.Range(-generateSpaceRadius, 1 + generateSpaceRadius);
        screenY = Random.Range(-generateSpaceRadius, 1 + generateSpaceRadius);
      } while(-generateOffset < screenX && screenX < generateOffset + 1 && -generateOffset < screenY && screenY < generateOffset + 1);

      spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(screenX, screenY, Camera.main.transform.position.y));
    } while(!overlap && Physics.OverlapSphere(spawnPosition, overlapDistance, mask).Length > 0 && count++ < 100);

    if (count >= 100) Debug.Log("An object is overlapped");

    return spawnPosition;
  }
}

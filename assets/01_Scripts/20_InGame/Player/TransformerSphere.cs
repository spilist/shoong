using UnityEngine;
using System.Collections;

public class TransformerSphere : MonoBehaviour {
  public TransformerManager tfm;
  private GameObject transformParticle;
  private GameObject transformLaser;

  public string[] subs;
  public string[] mains;
  private string[] subsSpawn;
  private string[] mainsSpawn;

  private int subRatio;
  private int mainRatio;
  private int level;
  private float transformDuration;
  private float laserShootDuration;

	void OnEnable() {
    subRatio = tfm.subRatio;
    mainRatio = tfm.mainRatio;
    transform.localScale = tfm.areaRadius * Vector3.one;

    string subObjectsString = PlayerPrefs.GetString("SubObjects").Trim();
    int selectedCount = (subObjectsString == "") ? 0 : subObjectsString.Split(' ').Length;
    subsSpawn = new string[subs.Length - selectedCount];
    int count = 0;
    foreach (string obj in subs) {
      if (!subObjectsString.Contains(obj)) subsSpawn[count++] = obj;
    }

    string mainObjectsString = PlayerPrefs.GetString("MainObjects").Trim();
    selectedCount = (mainObjectsString == "") ? 0 : mainObjectsString.Split(' ').Length;
    mainsSpawn = new string[mains.Length - selectedCount];
    count = 0;
    foreach (string obj in mains) {
      if (!mainObjectsString.Contains(obj)) mainsSpawn[count++] = obj;
    }

    level = tfm.level;
    transformDuration = tfm.transformDuration;
    laserShootDuration = tfm.laserShootDuration;
    transformParticle = tfm.transformParticle;
    transformLaser = tfm.transformLaser;
  }

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Obstacle_big" || other.tag == "Obstacle_small") {
      ObjectsMover mover = other.GetComponent<ObjectsMover>();

      mover.transformed(transform.position, transformLaser, laserShootDuration, transformDuration, transformParticle, transformResult(), level);
    }
  }

  string transformResult() {
    int random = Random.Range(0, 100);
    string result = "";
    if (random < mainRatio) {
      result = mainsSpawn[Random.Range(0, mainsSpawn.Length)];
    } else if (random < mainRatio + subRatio) {
      result = subsSpawn[Random.Range(0, subsSpawn.Length)];
    }
    return result;
  }
}

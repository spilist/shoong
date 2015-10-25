using UnityEngine;
using System.Collections;

public class TransformerSphere : MonoBehaviour {
  public Skill_Transformer tfm;

  public string[] subs;
  public string[] mains;
  private string[] subsSpawn;

  private int subRatio;
  private int level;

	void OnEnable() {
    subRatio = tfm.subRatio;

    string subObjectsString = PlayerPrefs.GetString("SubObjects").Trim();
    int selectedCount = (subObjectsString == "") ? 0 : subObjectsString.Split(' ').Length;
    subsSpawn = new string[subs.Length - selectedCount];
    int count = 0;
    foreach (string obj in subs) {
      if (!subObjectsString.Contains(obj)) subsSpawn[count++] = obj;
    }

    level = tfm.level;
  }

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Obstacle_big" || other.tag == "Obstacle_small") {
      ObjectsMover mover = other.GetComponent<ObjectsMover>();

      mover.transformed(transform.position, transformResult(), level);
    }
  }

  string transformResult() {
    int random = Random.Range(0, 100);
    string result = "";
    if (random < subRatio) {
      result = subsSpawn[Random.Range(0, subsSpawn.Length)];
    }
    return result;
  }
}

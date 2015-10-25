using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill : MonoBehaviour {
  public GameObject skillObject;
  public GameObject useSkillEffect;
  public float duration;
  public float coolTime;
  public int level;
  private float originalDuration;
  private float originalCoolTime;

  void Awake() {
    originalDuration = duration;
    originalCoolTime = coolTime;
  }

  void Start() {
    level = DataManager.dm.getInt(name + "Level") - 1;
    if (level < 0) level = 0;
    adjustForLevel(level);
    afterEnable();
  }

  public void moreDuration(int val) {
    duration += val;
  }

  public void lessCoolTime(int val) {
    coolTime -= val;
  }

  virtual public void resetAbility() {
    duration = originalDuration;
    coolTime = originalCoolTime;
  }

  public GameObject getPooledObj(List<GameObject> list, GameObject prefab, Vector3 pos) {
    GameObject obj = getPooledObj(list, prefab);
    obj.transform.position = pos;
    return obj;
  }

  public GameObject getPooledObj(List<GameObject> list, GameObject prefab) {
    for (int i = 0; i < list.Count; i++) {
      if (!list[i].activeInHierarchy) {
        return list[i];
      }
    }

    GameObject obj = (GameObject) Instantiate(prefab);
    obj.transform.parent = transform;
    list.Add(obj);
    return obj;
  }

  virtual public void adjustForLevel(int level) {}

  virtual public void afterEnable() {}

  public void activate(bool val) {
    skillObject.SetActive(val);
    // useSkillEffect.transform.rotation = Quaternion.identity;
    useSkillEffect.SetActive(val);
    Player.pl.effectedBy(name, val);
    afterActivate(val);
  }

  virtual public void afterActivate(bool val) {}
}

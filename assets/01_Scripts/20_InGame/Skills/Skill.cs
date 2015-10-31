using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill : MonoBehaviour {
  public GameObject skillObject;
  public GameObject useSkillEffect;
  public int normalRing;
  public int skillRing;
  public float duration;
  private bool activated;

  void Start() {
    afterStart();
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


  virtual public void afterStart() {}

  public void activate(bool val) {
    activated = val;

    if (skillObject != null) skillObject.SetActive(val);
    if (useSkillEffect != null) useSkillEffect.SetActive(val);

    if (val && duration > 0) Invoke("inactivate", duration);
    Player.pl.effectedBy(name, val);

    afterActivate(val);
  }

  virtual public void afterActivate(bool val) {}

  public bool isActivated() {
    return activated;
  }

  public void extendDuration() {
    CancelInvoke();
    Invoke("inactivate", duration);
  }

  void inactivate() {
    activate(false);
  }

  void Disable() {
    CancelInvoke();
  }
}

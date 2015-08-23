using UnityEngine;
using System.Collections;

public class ObjectsManager : MonoBehaviour {
  protected SpawnManager spawnManager;

  void OnEnable() {
    spawnManager = gameObject.GetComponent<SpawnManager>();
  }

  virtual public void run() {

  }

  virtual public float getSpeed(string objTag) {
    return 0;
  }

  virtual public float getTumble(string objTag) {
    return 0;
  }
}

using UnityEngine;
using System.Collections;

public class ObjectsManager : MonoBehaviour {
  protected SpawnManager spawnManager;
  protected PlayerMover player;
  protected bool skipInterval = false;
  public float strength = 0;

  void OnEnable() {
    spawnManager = gameObject.GetComponent<SpawnManager>();
    player = GameObject.Find("Player").GetComponent<PlayerMover>();
  }

  virtual public void run() {

  }

  virtual public float getSpeed(string objTag) {
    return 0;
  }

  virtual public float getTumble(string objTag) {
    return 0;
  }

  virtual public void skipRespawnInterval() {
    skipInterval = true;
  }
}

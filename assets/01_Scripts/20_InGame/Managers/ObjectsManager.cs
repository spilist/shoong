using UnityEngine;
using System.Collections;

public class ObjectsManager : MonoBehaviour {
  protected SpawnManager spawnManager;
  protected PlayerMover player;
  protected CharacterChangeManager changeManager;
  protected bool skipInterval = false;
  protected ComboBar comboBar;

  public float strength = 0;

  void OnEnable() {
    spawnManager = gameObject.GetComponent<SpawnManager>();
    player = GameObject.Find("Player").GetComponent<PlayerMover>();
    changeManager = player.GetComponent<CharacterChangeManager>();
    comboBar = GameObject.Find("Bars Canvas").GetComponent<ComboBar>();

    initRest();
  }

  virtual public void initRest() {

  }

  virtual public void run() {

  }

  virtual public float getSpeed(string objTag) {
    return 0;
  }

  virtual public float getTumble(string objTag) {
    return 0;
  }

  public void skipRespawnInterval() {
    skipInterval = true;
  }

  virtual public int cubesWhenEncounter() {
    return comboBar.getComboRatio();
  }

}

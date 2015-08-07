using UnityEngine;
using System.Collections;

public class Blackhole : MonoBehaviour {
  public ParticleSystem obstacleDestroy;

  public FieldObjectsManager fom;
  public ComboPartsManager cpm;
  public GameOver gameOver;

  void Start () {
    // fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    // cpm = GameObject.Find("Field Objects").GetComponent<ComboPartsManager>();
    // gameOver = GameObject.Find("GameOver").GetComponent<GameOver>();
	}

  void OnTriggerEnter(Collider other) {
    // Debug.Log(other.tag + "is in blackhole");
    if (other.tag == "SpecialPart") {
      fom.spawnSpecial();
    } else if (other.tag == "ComboPart") {
      cpm.destroyInstances();
    } else if (other.tag == "Player" ) {
      gameOver.run();
    } else if (other.tag == "Obstacle" || other.tag == "Obstacle_big" || other.tag == "Monster") {
      Destroy(other.gameObject);
    } else {
      Destroy(other.gameObject);
    }
  }
}

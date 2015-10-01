using UnityEngine;
using System.Collections;

public class DangerousForceField : MonoBehaviour {
  PlayerMover player;

	void Start () {
    player = GameObject.Find("Player").GetComponent<PlayerMover>();
	}

	void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
      if (!player.isInvincible()) player.scoreManager.gameOver("Dopple");
    }

    if (other.tag == "Dopple") return;

    other.GetComponent<ObjectsMover>().destroyObject();
  }
}

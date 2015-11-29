using UnityEngine;
using System.Collections;

public class DangerousForceField : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
      if (!Player.pl.isInvincible()) {
        ScoreManager.sm.gameOver("Dopple");
        return;
      }
    }

    if (other.tag == "Dopple") return;

    other.GetComponent<ObjectsMover>().destroyObject();
  }
}
